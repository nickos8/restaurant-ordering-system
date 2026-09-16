using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderApi.Api.Controllers;
using RestaurantOrderApi.Api.Data;
using RestaurantOrderApi.Api.Dtos;
using RestaurantOrderApi.Api.Models;

namespace RestaurantOrderApi.Tests;

public class OrdersControllerTests
{
    private static RestaurantDbContext NewDb()
    {
        var options = new DbContextOptionsBuilder<RestaurantDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new RestaurantDbContext(options);
        db.MenuItems.AddRange(
            new MenuItem { Id = 1, Name = "Adobo Rice Bowl", Category = "Main", Price = 150.00m, IsAvailable = true },
            new MenuItem { Id = 2, Name = "Iced Tea", Category = "Beverage", Price = 45.00m, IsAvailable = true },
            new MenuItem { Id = 3, Name = "Sold Out Cake", Category = "Dessert", Price = 120.00m, IsAvailable = false }
        );
        db.SaveChanges();
        return db;
    }

    [Fact]
    public async Task AddItem_ThenRemovePartialQuantity_UpdatesTotalCorrectly()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;

        await controller.AddItem(orderId, new AddOrderItemRequest { MenuItemId = 1, Quantity = 3 });
        var afterRemove = await controller.RemoveItem(orderId, 1, quantity: 1);

        var order = ((OkObjectResult)afterRemove.Result!).Value as OrderResponse;

        Assert.NotNull(order);
        Assert.Equal(300.00m, order!.Total); // 2 remaining x 150
    }

    [Fact]
    public async Task AddItem_ForUnavailableMenuItem_ReturnsBadRequest()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;

        var result = await controller.AddItem(orderId, new AddOrderItemRequest { MenuItemId = 3, Quantity = 1 });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Pay_OnEmptyOrder_ReturnsBadRequest()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;

        var result = await controller.Pay(orderId, new PayOrderRequest { PaymentMethod = "Cash" });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task Pay_TwiceOnSameOrder_SecondCallReturnsBadRequest()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;
        await controller.AddItem(orderId, new AddOrderItemRequest { MenuItemId = 2, Quantity = 1 });

        var firstPay = await controller.Pay(orderId, new PayOrderRequest { PaymentMethod = "Cash" });
        var secondPay = await controller.Pay(orderId, new PayOrderRequest { PaymentMethod = "Card" });

        Assert.IsType<OkObjectResult>(firstPay.Result);
        Assert.IsType<BadRequestObjectResult>(secondPay.Result);
    }

    [Fact]
    public async Task AddItem_AfterOrderIsPaid_ReturnsBadRequest()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;
        await controller.AddItem(orderId, new AddOrderItemRequest { MenuItemId = 2, Quantity = 1 });
        await controller.Pay(orderId, new PayOrderRequest { PaymentMethod = "Cash" });

        var result = await controller.AddItem(orderId, new AddOrderItemRequest { MenuItemId = 1, Quantity = 1 });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetReceipt_BeforePayment_ReturnsBadRequest()
    {
        var db = NewDb();
        var controller = new OrdersController(db);

        var created = await controller.Create(new CreateOrderRequest { CustomerName = "Nickos" });
        var orderId = ((OrderResponse)((CreatedAtActionResult)created.Result!).Value!).Id;

        var result = await controller.GetReceipt(orderId);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
