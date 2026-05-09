using Ecom.Application.Interfaces;

namespace Ecom.Application.Services;

public class ProductGroupingService : IProductGroupingService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductGroupRepository _productGroupRepository;
    private readonly IProductPackingService _productPackingService;
    private readonly IUnitOfWork _unitOfWork;

    public ProductGroupingService(
        IProductRepository productRepository,
        IProductGroupRepository productGroupRepository,
        IProductPackingService productPackingService,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _productGroupRepository = productGroupRepository;
        _productPackingService = productPackingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> ProcessProductsAsync()
    {
        var products = await _productRepository.GetUnprocessedAsync();

        if (products.Count == 0)
        {
            return 0;
        }

        var existingGroupsCount = await _productGroupRepository.CountAsync();

        var groups = _productPackingService.CreateGroups(
            products,
            existingGroupsCount
        );

        foreach (var group in groups)
        {
            await _productGroupRepository.AddAsync(group);
        }

        _productRepository.MarkAsProcessed(products);

        await _unitOfWork.SaveChangesAsync();

        return groups.Count;
    }
}