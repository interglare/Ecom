namespace Ecom.Application.Interfaces;

public interface IProductGroupingService
{
    Task<int> ProcessProductsAsync();
}