# Learning Log

> A durable record of concepts the project owner has learned and explained in their own words. Update this file after understanding is demonstrated, not merely after code is copied.

## Learning approach

The project uses the Feynman method:

1. Learn one concept.
2. Explain it simply.
3. Find and correct gaps.
4. Apply it in the project.
5. Verify it with a command, manual check, or automated test.

The owner already knows PHP/Laravel, React, JavaScript, MySQL/PostgreSQL, and Git. C# and Vue are the genuinely new material in this project; concepts should be taught by bridging from the Laravel/React equivalent whenever one exists.

## Confirmed foundations

### EF Core DbContext, the Laravel Eloquent equivalent

`RestaurantDbContext` plays the role Eloquent models play in Laravel: it represents the database and exposes `DbSet<T>` collections (`MenuItems`, `Orders`, `OrderItems`) used to query and save data, similar to how `Project::all()` or `Project::create()` work in Laravel.

### Migration

An EF Core migration is a version-controlled blueprint for creating or changing database structure, the same concept as a Laravel migration, expressed in C# instead of PHP. `dotnet ef migrations add InitialCreate` generated the migration; it is applied automatically on startup in `Program.cs` via `db.Database.Migrate()`.

### Entity vs DTO

An entity (`MenuItem`, `Order`, `OrderItem`) is the shape EF Core persists to the database. A DTO (`MenuItemResponse`, `OrderResponse`, `CreateOrderRequest`, and so on) is the shape the API actually sends or receives over HTTP. Controllers map between them so internal database structure is never accidentally exposed or bound directly from untrusted request data.

### Controller-based ASP.NET Core routing

`[ApiController]` and `[Route("api/[controller]")]` on `MenuItemsController` and `OrdersController` are the ASP.NET Core equivalent of a Laravel route file plus controller: the attribute-based route maps an HTTP verb and path directly onto a C# method, instead of a separate `routes/api.php` file.

### Order total as a computed property

`Order.Total` is a C# computed property (`=> Items.Sum(...)`), not a stored column. This is the same idea as an Eloquent accessor: it is always derived from the current `Items` collection rather than being a value that can drift out of sync.

### Price snapshotting

`OrderItem.UnitPrice` is copied from `MenuItem.Price` at the moment an item is added to the cart, not read live from the menu when the total is calculated later. This mirrors how real order/invoice systems work: an order's total must not change retroactively if the menu price changes after the order was placed.

### Business rules enforced in the controller, not just the database

`OrdersController` explicitly checks `order.Status != OrderStatus.Open` before allowing cart changes, checks for an empty cart before allowing payment, and checks for `OrderStatus.Open` before allowing payment. These are application-level invariants, not something the database schema alone can guarantee.

### xUnit testing with EF Core InMemory

`OrdersControllerTests` build a fresh `RestaurantDbContext` per test using `UseInMemoryDatabase(Guid.NewGuid().ToString())`, giving each test its own isolated database, the same purpose Laravel's `RefreshDatabase` trait serves with SQLite `:memory:`.

### Retire vs delete

Deleting a `MenuItem` still referenced by an `OrderItem` would either violate the foreign key or silently corrupt past order history. `MenuItemsController.Delete` checks for existing references first and sets `IsAvailable = false` instead of removing the row when needed.

## Development tools and verification

### `dotnet build`

Compiles the project and reports compile errors. It is the C# equivalent of a PHP syntax check, but stronger, because C# is statically typed and catches many more classes of error at this stage than PHP's `-l` flag does.

### `dotnet test`

Runs the xUnit test suite. Current result: 9 tests, all passing, covering `Order.Total` calculation and the cart/pay/receipt controller rules.

### `dotnet ef migrations add` / automatic `Database.Migrate()`

Generates and applies schema changes. Migrations are applied automatically on API startup in this project (`Program.cs`), rather than requiring a manual `dotnet ef database update` step, to keep local setup simple for anyone cloning the repo.

### Git checks

Same habits as `developer-portfolio`: confirm no build output, database files, or secrets are staged before committing; confirm the local branch and `origin/main` agree before pushing.

## Concepts to reinforce next (Vue, still new)

- Vue 3 Composition API vs Options API, and which one this project will use
- reactive state (`ref`, `reactive`) compared to React's `useState`
- component structure and props, compared to React components
- calling a REST API from Vue (`fetch` or `axios`, same libraries/concepts as the React frontend already used in `developer-portfolio`)
- CORS: why the browser blocks API calls from a different origin during local development, and how a backend CORS policy fixes it (already understood in principle from `developer-portfolio`'s Laravel CORS work; needs to be re-applied on the ASP.NET Core side)
- Vue Router, if the frontend grows beyond a single page
- component-level state vs a shared store (e.g. Pinia), if cart state needs to be shared across multiple components

## Concepts to reinforce later (stretch phase)

- ASP.NET Core authentication/authorization, if Phase 4 adds it
- `WebApplicationFactory` integration testing through the real HTTP pipeline
- deployment of a .NET API and a static Vue build
