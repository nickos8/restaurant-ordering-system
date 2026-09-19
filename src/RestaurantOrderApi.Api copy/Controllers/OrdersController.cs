using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderApi.Api.Data;
using RestaurantOrderApi.Api.Dtos;
using RestaurantOrderApi.Api.Models;

namespace RestaurantOrderApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly RestaurantDbContext _db;

    public OrdersController(RestaurantDbContext db)
    {
        _db = db;
    }

    private static OrderResponse ToResponse(Order order) => new(
        order.Id,
        order.CustomerName,
        order.Status.ToString(),
        order.CreatedAt,
        order.PaidAt,
        order.PaymentMethod,
        order.Items.Select(i => new OrderItemResponse(
            i.Id,
            i.MenuItemId,
            i.MenuItem?.Name ?? string.Empty,
            i.Quantity,
            i.UnitPrice,
            i.UnitPrice * i.Quantity)).ToList(),
        order.Total);

    private Task<Order?> LoadOrderAsync(int id) =>
        _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
    {
        var orders = await _db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .AsNoTracking()
            .ToListAsync();

        return Ok(orders.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id)
    {
        var order = await LoadOrderAsync(id);
        if (order is null) return NotFound();
        return Ok(ToResponse(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
    {
        var order = new Order { CustomerName = request.CustomerName };
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, ToResponse(order));
    }

    // Add an item to the cart. Selecting from the menu happens client-side;
    // this is the "add to cart" step.
    [HttpPost("{id:int}/items")]
    public async Task<ActionResult<OrderResponse>> AddItem(int id, AddOrderItemRequest request)
    {
        var order = await LoadOrderAsync(id);
        if (order is null) return NotFound($"Order {id} not found.");
        if (order.Status != OrderStatus.Open)
            return BadRequest($"Order {id} is {order.Status} and can no longer be modified.");

        var menuItem = await _db.MenuItems.FindAsync(request.MenuItemId);
        if (menuItem is null) return BadRequest($"Menu item {request.MenuItemId} not found.");
        if (!menuItem.IsAvailable) return BadRequest($"'{menuItem.Name}' is not currently available.");

        var existingLine = order.Items.FirstOrDefault(i => i.MenuItemId == request.MenuItemId);
        if (existingLine is not null)
        {
            existingLine.Quantity += request.Quantity;
        }
        else
        {
            order.Items.Add(new OrderItem
            {
                OrderId = order.Id,
                MenuItemId = menuItem.Id,
                Quantity = request.Quantity,
                UnitPrice = menuItem.Price
            });
        }

        await _db.SaveChangesAsync();
        return Ok(ToResponse(await LoadOrderAsync(id) ?? order));
    }

    // Remove (or reduce) an item from the cart.
    [HttpDelete("{id:int}/items/{menuItemId:int}")]
    public async Task<ActionResult<OrderResponse>> RemoveItem(int id, int menuItemId, [FromQuery] int quantity = 0)
    {
        var order = await LoadOrderAsync(id);
        if (order is null) return NotFound($"Order {id} not found.");
        if (order.Status != OrderStatus.Open)
            return BadRequest($"Order {id} is {order.Status} and can no longer be modified.");

        var line = order.Items.FirstOrDefault(i => i.MenuItemId == menuItemId);
        if (line is null) return NotFound($"Menu item {menuItemId} is not in order {id}.");

        if (quantity <= 0 || quantity >= line.Quantity)
        {
            _db.OrderItems.Remove(line);
        }
        else
        {
            line.Quantity -= quantity;
        }

        await _db.SaveChangesAsync();
        return Ok(ToResponse(await LoadOrderAsync(id) ?? order));
    }

    [HttpPost("{id:int}/pay")]
    public async Task<ActionResult<ReceiptResponse>> Pay(int id, PayOrderRequest request)
    {
        var order = await LoadOrderAsync(id);
        if (order is null) return NotFound($"Order {id} not found.");
        if (order.Status != OrderStatus.Open)
            return BadRequest($"Order {id} is already {order.Status}.");
        if (order.Items.Count == 0)
            return BadRequest("Cannot pay for an empty order.");

        order.Status = OrderStatus.Paid;
        order.PaymentMethod = request.PaymentMethod;
        order.PaidAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(BuildReceipt(order));
    }

    [HttpGet("{id:int}/receipt")]
    public async Task<ActionResult<ReceiptResponse>> GetReceipt(int id)
    {
        var order = await LoadOrderAsync(id);
        if (order is null) return NotFound($"Order {id} not found.");
        if (order.Status != OrderStatus.Paid || order.PaidAt is null || order.PaymentMethod is null)
            return BadRequest($"Order {id} has not been paid yet.");

        return Ok(BuildReceipt(order));
    }

    private static ReceiptResponse BuildReceipt(Order order) => new(
        order.Id,
        order.CustomerName,
        order.PaidAt!.Value,
        order.PaymentMethod!,
        order.Items.Select(i => new OrderItemResponse(
            i.Id,
            i.MenuItemId,
            i.MenuItem?.Name ?? string.Empty,
            i.Quantity,
            i.UnitPrice,
            i.UnitPrice * i.Quantity)).ToList(),
        order.Total);
}
