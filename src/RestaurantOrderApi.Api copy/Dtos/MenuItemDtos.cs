using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderApi.Api.Dtos;

public record MenuItemResponse(int Id, string Name, string Category, decimal Price, bool IsAvailable);

public class MenuItemRequest
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;
}
