using System.Globalization;
using ClosedXML.Excel;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

namespace Ecom.Infrastructure.Services;

public class ExcelImportService : IExcelImportService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExcelImportService(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> ImportProductsAsync(Stream fileStream, string fileName)
    {
        if (fileStream.Length == 0)
        {
            throw new InvalidOperationException("Файл пустой");
        }

        var extension = Path.GetExtension(fileName);

        if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Можно загружать только файлы с расширением .xlsx");
        }

        using var workbook = new XLWorkbook(fileStream);

        var worksheet = workbook.Worksheets.First();

        var products = new List<Product>();

        var rows = worksheet.RowsUsed().Skip(1);

        foreach (var row in rows)
        {
            var name = row.Cell(1).GetString().Trim();
            var unit = row.Cell(2).GetString().Trim();
            var unitPriceText = row.Cell(3).GetString().Trim();
            var quantityText = row.Cell(4).GetString().Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            if (!TryParseDecimal(unitPriceText, out var unitPrice))
            {
                throw new InvalidOperationException(
                    $"Не удалось прочитать цену товара '{name}'. Значение: '{unitPriceText}'"
                );
            }

            if (!int.TryParse(quantityText, out var quantity))
            {
                throw new InvalidOperationException(
                    $"Не удалось прочитать количество товара '{name}'. Значение: '{quantityText}'"
                );
            }

            if (unitPrice <= 0 || quantity <= 0)
            {
                continue;
            }

            products.Add(new Product
            {
                Name = name,
                Unit = unit,
                UnitPrice = unitPrice,
                Quantity = quantity,
                IsProcessed = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (products.Count == 0)
        {
            throw new InvalidOperationException("В Excel-файле не найдено товаров");
        }

        await _productRepository.AddRangeAsync(products);
        await _unitOfWork.SaveChangesAsync();

        return products.Count;
    }

    private static bool TryParseDecimal(string value, out decimal result)
    {
        value = value.Trim().Replace(",", ".");

        return decimal.TryParse(
            value,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out result
        );
    }
}