namespace Ecom.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public bool IsProcessed { get; set; }
}