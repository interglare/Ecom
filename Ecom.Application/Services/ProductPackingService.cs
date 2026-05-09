using Ecom.Application.Interfaces;
using Ecom.Domain.Constants;
using Ecom.Domain.Entities;

namespace Ecom.Application.Services;

public class ProductPackingService : IProductPackingService
{
    public List<ProductGroup> CreateGroups(
        List<Product> products,
        int existingGroupsCount)
    {
        if (products.Count == 0)
        {
            return new List<ProductGroup>();
        }

        var invalidProduct = products.FirstOrDefault(x =>
            x.UnitPrice > ProductGroupConstants.MaxGroupTotal
        );

        if (invalidProduct != null)
        {
            throw new InvalidOperationException(
                $"Товар '{invalidProduct.Name}' стоит дороже {ProductGroupConstants.MaxGroupTotal} евро и не может быть добавлен в группу"
            );
        }

        var remainingProducts = products
            .Select(x => new ProductRemaining
            {
                ProductId = x.Id,
                Name = x.Name,
                Unit = x.Unit,
                UnitPrice = x.UnitPrice,
                RemainingQuantity = x.Quantity
            })
            .ToList();

        var groups = new List<ProductGroup>();

        while (remainingProducts.Any(x => x.RemainingQuantity > 0))
        {
            var selectedItems = FindBestGroup(remainingProducts);

            if (selectedItems.Count == 0)
            {
                break;
            }

            var groupNumber = existingGroupsCount + groups.Count + 1;

            var group = new ProductGroup
            {
                Name = $"Группа {groupNumber}",
                TotalPrice = selectedItems.Sum(x => x.UnitPrice * x.Quantity),
                Items = selectedItems
                    .Select(x => new ProductGroupItem
                    {
                        ProductName = x.Name,
                        Unit = x.Unit,
                        UnitPrice = x.UnitPrice,
                        Quantity = x.Quantity
                    })
                    .ToList()
            };

            groups.Add(group);

            foreach (var selectedItem in selectedItems)
            {
                var remaining = remainingProducts.First(x =>
                    x.ProductId == selectedItem.ProductId
                );

                remaining.RemainingQuantity -= selectedItem.Quantity;
            }
        }

        return groups;
    }

    private static List<SelectedProductItem> FindBestGroup(
        List<ProductRemaining> products)
    {
        var maxTotalInCents = ToCents(ProductGroupConstants.MaxGroupTotal);

        var states = new Dictionary<int, Dictionary<int, int>>
        {
            [0] = new()
        };

        foreach (var product in products.Where(x => x.RemainingQuantity > 0))
        {
            var priceInCents = ToCents(product.UnitPrice);

            var newStates = new Dictionary<int, Dictionary<int, int>>(states);

            foreach (var state in states)
            {
                var currentTotal = state.Key;
                var currentSelection = state.Value;

                var maxQuantityByLimit =
                    (maxTotalInCents - currentTotal) / priceInCents;

                var maxQuantity = Math.Min(
                    product.RemainingQuantity,
                    maxQuantityByLimit
                );

                for (var quantity = 1; quantity <= maxQuantity; quantity++)
                {
                    var newTotal = currentTotal + priceInCents * quantity;

                    if (newTotal > maxTotalInCents)
                    {
                        break;
                    }

                    if (newStates.ContainsKey(newTotal))
                    {
                        continue;
                    }

                    var newSelection = new Dictionary<int, int>(currentSelection)
                    {
                        [product.ProductId] = quantity
                    };

                    newStates[newTotal] = newSelection;
                }
            }

            states = newStates;
        }

        var bestTotal = states.Keys
            .Where(x => x > 0)
            .OrderByDescending(x => x)
            .FirstOrDefault();

        if (bestTotal == 0)
        {
            return new List<SelectedProductItem>();
        }

        var bestSelection = states[bestTotal];

        return bestSelection
            .Select(selection =>
            {
                var product = products.First(x => x.ProductId == selection.Key);

                return new SelectedProductItem
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Unit = product.Unit,
                    UnitPrice = product.UnitPrice,
                    Quantity = selection.Value
                };
            })
            .ToList();
    }

    private static int ToCents(decimal value)
    {
        return (int)Math.Round(value * 100, MidpointRounding.AwayFromZero);
    }
}

internal sealed class ProductRemaining
{
    public int ProductId { get; init; }

    public string Name { get; init; } = null!;

    public string Unit { get; init; } = null!;

    public decimal UnitPrice { get; init; }

    public int RemainingQuantity { get; set; }
}

internal sealed class SelectedProductItem
{
    public int ProductId { get; init; }

    public string Name { get; init; } = null!;

    public string Unit { get; init; } = null!;

    public decimal UnitPrice { get; init; }

    public int Quantity { get; init; }
}