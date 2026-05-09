namespace Ecom.Application.Interfaces;

public interface IExcelImportService
{
    Task<int> ImportProductsAsync(Stream fileStream, string fileName);
}