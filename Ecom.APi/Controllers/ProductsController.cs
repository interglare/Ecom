using Microsoft.AspNetCore.Mvc;
using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;

namespace Ecom.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IExcelImportService _excelImportService;
    private readonly IProductRepository _productRepository;

    public ProductsController(
        IExcelImportService excelImportService,
        IProductRepository productRepository)
    {
        _excelImportService = excelImportService;
        _productRepository = productRepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcel(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    error = "Файл не был загружен"
                });
            }

            await using var stream = file.OpenReadStream();

            var importedCount = await _excelImportService.ImportProductsAsync(
                stream,
                file.FileName
            );

            return Ok(new
            {
                message = "Файл успешно загружен",
                importedCount
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts()
    {
        var products = await _productRepository.GetAllAsync();

        var result = products
            .Select(x => new ProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Unit = x.Unit,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                IsProcessed = x.IsProcessed
            })
            .ToList();

        return Ok(result);
    }
}