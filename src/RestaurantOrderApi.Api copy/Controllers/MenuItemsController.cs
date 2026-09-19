using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantOrderApi.Api.Data;
using RestaurantOrderApi.Api.Dtos;
using RestaurantOrderApi.Api.Models;

namespace RestaurantOrderApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly RestaurantDbContext _db;

    public MenuItemsController(RestaurantDbContext db)
    {
        _db = db;
    }

    private static MenuItemResponse ToResponse(MenuItem item) =>
        new(item.Id, item.Name, item.Category, item.Price, item.IsAvailable);

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetAll()
    {
        var items = await _db.MenuItems.AsNoTracking().ToListAsync();
        return Ok(items.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MenuItemResponse>> GetById(int id)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item is null) return NotFound();
        return Ok(ToResponse(item));
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemResponse>> Create(MenuItemRequest request)
    {
        var item = new MenuItem
        {
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            IsAvailable = request.IsAvailable
        };

        _db.MenuItems.Add(item);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, ToResponse(item));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MenuItemRequest request)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item is null) return NotFound();

        item.Name = request.Name;
        item.Category = request.Category;
        item.Price = request.Price;
        item.IsAvailable = request.IsAvailable;

        await _db.SaveChangesAsync();
        return Ok(ToResponse(item));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.MenuItems.FindAsync(id);
        if (item is null) return NotFound();

        var isUsedInAnOrder = await _db.OrderItems.AnyAsync(oi => oi.MenuItemId == id);
        if (isUsedInAnOrder)
        {
            // Preserve order history: retire the item instead of deleting a row other orders reference.
            item.IsAvailable = false;
            await _db.SaveChangesAsync();
            return Ok(ToResponse(item));
        }

        _db.MenuItems.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
