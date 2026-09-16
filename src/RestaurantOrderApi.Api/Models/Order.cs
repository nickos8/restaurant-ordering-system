namespace RestaurantOrderApi.Api.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PaidAt { get; set; }
    public string? PaymentMethod { get; set; }

    public List<OrderItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.UnitPrice * i.Quantity);
}
