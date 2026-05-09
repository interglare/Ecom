using Ecom.Domain.Entities;

namespace Ecom.Application.Interfaces;

public interface IProductPackingService
{
    List<ProductGroup> CreateGroups(
        List<Product> products,
        int existingGroupsCount);
}