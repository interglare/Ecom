using Ecom.Domain.Entities;
    
namespace Ecom.Application.Interfaces;

public interface IProductGroupRepository
{
    Task<int> CountAsync();
    Task AddAsync(ProductGroup group);
    Task<List<ProductGroup>> GetAllAsync();
    Task<ProductGroup?> GetByIdWithItemsAsync(int id);
}