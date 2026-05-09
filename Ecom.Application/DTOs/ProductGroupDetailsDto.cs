namespace Ecom.Application.DTOs;

public class ProductGroupDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public List<ProductGroupItemDto> Products { get; set; } = new();
}