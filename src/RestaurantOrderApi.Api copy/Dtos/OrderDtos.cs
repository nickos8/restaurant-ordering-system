using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderApi.Api.Dtos;

public class CreateOrderRequest
{
    [Required, StringLength(100)]
    public string CustomerName { get; set; } = string.Empty;
}

public class AddOrderItemRequest
{
    [Required]
    public int MenuItemId { get; set; }

    [Range(1, 100)]
    public int Quantity { get; set; } = 1;
}

public class PayOrderRequest
{
    [Required, StringLength(30)]
    public string PaymentMethod { get; set; } = string.Empty;
}

public record OrderItemResponse(int Id, int MenuItemId, string MenuItemName, int Quantity, decimal UnitPrice, decimal LineTotal);

public record OrderResponse(
    int Id,
    string CustomerName,
    string Status,
    DateTime CreatedAt,
    DateTime? PaidAt,
    string? PaymentMethod,
    List<OrderItemResponse> Items,
    decimal Total);

public record ReceiptResponse(
    int OrderId,
    string CustomerName,
    DateTime PaidAt,
    string PaymentMethod,
    List<OrderItemResponse> Items,
    decimal Total);
