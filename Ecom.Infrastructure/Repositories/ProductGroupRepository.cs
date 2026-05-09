using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Repositories;

public class ProductGroupRepository : IProductGroupRepository
{
    private readonly AppDbContext _dbContext;

    public ProductGroupRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> CountAsync()
    {
        return _dbContext.ProductGroups.CountAsync();
    }

    public async Task AddAsync(ProductGroup group)
    {
        await _dbContext.ProductGroups.AddAsync(group);
    }

    public Task<List<ProductGroup>> GetAllAsync()
    {
        return _dbContext.ProductGroups
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    public Task<ProductGroup?> GetByIdWithItemsAsync(int id)
    {
        return _dbContext.ProductGroups
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}