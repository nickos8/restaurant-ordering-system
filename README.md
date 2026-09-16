# Restaurant Order API

A restaurant ordering system built with C# and ASP.NET Core, covering menu management and the full order lifecycle: select items, manage a cart, pay, and retrieve a receipt.

## Why this project

Built to demonstrate C# / ASP.NET Core backend skills: REST API design, EF Core with a relational database, request validation, and automated tests.

## Tech stack

- C# / .NET 8
- ASP.NET Core Web API (controller-based)
- Entity Framework Core + SQLite
- xUnit + EF Core InMemory for tests

## Features

- **Menu management** — full CRUD on menu items (name, category, price, availability). Deleting an item already used in an order retires it instead of removing order history.
- **Cart** — add items to an order, adjust or remove quantities, while the order is still open.
- **Checkout** — pay an order once, recording payment method and timestamp. A paid order can no longer be modified.
- **Receipt** — retrieve the itemized receipt for a paid order.
- Prices are snapshotted onto each order line at the time they're added, so a later menu price change never rewrites an already-placed order's total.

## Project structure

```
src/RestaurantOrderApi.Api/
  Controllers/     MenuItemsController, OrdersController
  Models/          MenuItem, Order, OrderItem, OrderStatus
  Dtos/            Request/response shapes
  Data/            RestaurantDbContext, seed data
  Migrations/      EF Core migrations
tests/RestaurantOrderApi.Tests/
  OrderTotalTests.cs        unit tests for order total calculation
  OrdersControllerTests.cs  controller tests for cart/pay/receipt rules
```

## Running locally

```bash
cd src/RestaurantOrderApi.Api
dotnet run
```

The API applies EF Core migrations automatically on startup and seeds a starter menu. Swagger UI is available at `/swagger` in development.

## Running tests

```bash
dotnet test
```

## API overview

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/menuitems` | List menu items |
| POST | `/api/menuitems` | Create a menu item |
| PUT | `/api/menuitems/{id}` | Update a menu item |
| DELETE | `/api/menuitems/{id}` | Delete or retire a menu item |
| POST | `/api/orders` | Create an order |
| POST | `/api/orders/{id}/items` | Add an item to the cart |
| DELETE | `/api/orders/{id}/items/{menuItemId}` | Remove (or reduce) an item from the cart |
| POST | `/api/orders/{id}/pay` | Pay the order |
| GET | `/api/orders/{id}/receipt` | Get the receipt for a paid order |
