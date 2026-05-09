namespace Ecom.Domain.Entities;

public class ProductGroupItem
{
    public int Id { get; set; }

    public int ProductGroupId { get; set; }

    public ProductGroup ProductGroup { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice => UnitPrice * Quantity;
}