namespace Ecom.Domain.Entities;

public class ProductGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<ProductGroupItem> Items { get; set; } = new();
}