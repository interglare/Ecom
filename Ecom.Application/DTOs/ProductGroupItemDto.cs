namespace Ecom.Application.DTOs;

public class ProductGroupItemDto
{
    public string ProductName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}