using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();

    Task<List<Product>> GetUnprocessedAsync();

    Task AddRangeAsync(List<Product> products);

    void MarkAsProcessed(List<Product> products);
}