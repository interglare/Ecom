using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Product>> GetAllAsync()
    {
        return _dbContext.Products
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public Task<List<Product>> GetUnprocessedAsync()
    {
        return _dbContext.Products
            .Where(x => !x.IsProcessed)
            .OrderByDescending(x => x.UnitPrice)
            .ToListAsync();
    }

    public async Task AddRangeAsync(List<Product> products)
    {
        await _dbContext.Products.AddRangeAsync(products);
    }

    public void MarkAsProcessed(List<Product> products)
    {
        foreach (var product in products)
        {
            product.IsProcessed = true;
        }
    }
}