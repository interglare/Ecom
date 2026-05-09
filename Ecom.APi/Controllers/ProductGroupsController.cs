using Microsoft.AspNetCore.Mvc;
using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;

namespace Ecom.Controllers;

[ApiController]
[Route("api/product-groups")]
public class ProductGroupsController : ControllerBase
{
    private readonly IProductGroupingService _productGroupingService;
    private readonly IProductGroupRepository _productGroupRepository;

    public ProductGroupsController(
        IProductGroupingService productGroupingService,
        IProductGroupRepository productGroupRepository)
    {
        _productGroupingService = productGroupingService;
        _productGroupRepository = productGroupRepository;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessProducts()
    {
        try
        {
            var createdGroupsCount =
                await _productGroupingService.ProcessProductsAsync();

            return Ok(new
            {
                message = "Группировка товаров выполнена",
                createdGroupsCount
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
    public async Task<ActionResult<List<ProductGroupDto>>> GetGroups()
    {
        var groups = await _productGroupRepository.GetAllAsync();

        var result = groups
            .Select(x => new ProductGroupDto
            {
                Id = x.Id,
                Name = x.Name,
                TotalPrice = x.TotalPrice,
                CreatedAt = x.CreatedAt
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("{groupId:int}/products")]
    public async Task<ActionResult<ProductGroupDetailsDto>> GetProductsByGroupId(
        int groupId)
    {
        var group = await _productGroupRepository.GetByIdWithItemsAsync(groupId);

        if (group == null)
        {
            return NotFound(new
            {
                error = "Группа не найдена"
            });
        }

        var result = new ProductGroupDetailsDto
        {
            Id = group.Id,
            Name = group.Name,
            TotalPrice = group.TotalPrice,
            Products = group.Items
                .Select(x => new ProductGroupItemDto
                {
                    ProductName = x.ProductName,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    TotalPrice = x.UnitPrice * x.Quantity
                })
                .ToList()
        };

        return Ok(result);
    }
}