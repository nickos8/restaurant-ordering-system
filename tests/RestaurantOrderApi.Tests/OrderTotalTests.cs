using RestaurantOrderApi.Api.Models;

namespace RestaurantOrderApi.Tests;

public class OrderTotalTests
{
    [Fact]
    public void Total_WithNoItems_IsZero()
    {
        var order = new Order();

        Assert.Equal(0m, order.Total);
    }

    [Fact]
    public void Total_SumsQuantityTimesUnitPrice_AcrossMultipleLines()
    {
        var order = new Order
        {
            Items =
            {
                new OrderItem { MenuItemId = 1, Quantity = 2, UnitPrice = 150.00m },
                new OrderItem { MenuItemId = 3, Quantity = 1, UnitPrice = 45.00m }
            }
        };

        Assert.Equal(345.00m, order.Total);
    }

    [Fact]
    public void Total_UsesSnapshottedUnitPrice_NotCurrentMenuPrice()
    {
        // The order line stores the price at the time it was added, so a later
        // menu price change must not silently change an already-placed order's total.
        var order = new Order
        {
            Items = { new OrderItem { MenuItemId = 1, Quantity = 1, UnitPrice = 150.00m } }
        };

        var menuItem = new MenuItem { Id = 1, Price = 200.00m };

        Assert.Equal(150.00m, order.Total);
        Assert.NotEqual(menuItem.Price, order.Total);
    }
}
