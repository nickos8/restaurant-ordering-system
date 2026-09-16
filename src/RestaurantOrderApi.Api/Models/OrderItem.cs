namespace RestaurantOrderApi.Api.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public int Quantity { get; set; }

    // Snapshot of the price at the time it was added, so later menu price
    // changes never rewrite an already-placed order's total.
    public decimal UnitPrice { get; set; }
}
