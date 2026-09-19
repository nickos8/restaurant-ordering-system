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

## Vue foundations (confirmed)

### Single-file components

A `.vue` file holds `<script setup>`, `<template>`, and `<style scoped>` together, the same role a React `.jsx` file plays in `developer-portfolio`, just with template and script kept in clearly separated blocks instead of mixed as JSX.

### `ref()` as Vue's `useState`

`ref(initialValue)` creates a reactive value. Reading/writing it in `<script setup>` uses `.value`; the template auto-unwraps it, no `.value` needed there. Confirmed understanding of the actual mechanism, not just the end value: Vue's reactivity system tracks which DOM nodes depend on which `ref`, and patches only those nodes when the value changes, rather than re-rendering the whole page.

### `onMounted()` as the equivalent of React's `useEffect(fn, [])`

Code inside `onMounted()` runs once, after the component is added to the page, the correct place to kick off a data fetch.

### Fetching data: the three-state pattern

`MenuList.vue` uses `isLoading`, `errorMessage`, and the data itself (`menuItems`) as three separate `ref`s, and the template branches on all three with `v-if`/`v-else-if`/`v-else`. This is the reusable pattern for every future page that loads data from the API.

### CORS, applied on the ASP.NET Core side

Already understood in principle from `developer-portfolio`'s Laravel CORS work. Re-applied here as `builder.Services.AddCors(...)` with a named policy, then `app.UseCors("policyName")` in the middleware pipeline before `app.MapControllers()`. Confirmed working: the browser fetch from `localhost:5173` to `localhost:5281` succeeded once the policy was added.

### Swagger's default placeholder values are real data, not documentation

Clicking "Try it out" → "Execute" in Swagger without changing the example values (`"string"`, a placeholder number) actually creates that row in the database, since Swagger is a live client, not a mockup. The owner independently correctly diagnosed 3 stray "string" rows in the rendered menu as leftover test data rather than a bug in the fetch code, and cleaned it up by deleting the local `.db` file and letting migrations reseed it.

## Vue component communication (confirmed)

### Props (parent to child)

Data flows one direction, down. `App.vue` passes `:items="cart"` to `<Cart>`; inside `Cart.vue`, `defineProps({ items: { type: Array, required: true } })` receives it. The child can read the prop but must never reassign it directly (mutating an object inside the array is fine, replacing the prop itself is not).

### Emits (child to parent)

The reverse direction: a child reports "this happened" upward with `emit('event-name', payload)`, and the parent listens with `@event-name="handler"` on the child's tag. Confirmed through real tracing: `Cart.vue`'s `+` button emits `increase-item` with a menu item id, `App.vue` catches it via `@increase-item="handleIncreaseItem"`, and `handleIncreaseItem` is the only place that actually mutates `cart`.

### Why state lives in the parent, not each child

`MenuList.vue` and `Cart.vue` are siblings, neither can see the other's data directly. Since both need to affect the same cart, the cart has to live in their common parent, `App.vue`, as a `ref`. This is the same "lift state up" pattern as React; Vue's answer is props down, emits up, exactly one owner of the real data.

### `computed()` re-verified after an initial gap

First teach-back conflated the consequence (needing manual updates) with the actual failure mode (a stale, silently wrong total shown with no error). Corrected: a plain `const total = ...` would calculate once and freeze, `computed()` re-runs its formula whenever `props.items` changes, because Vue tracks what a computed function reads. Confirmed understanding held up under a second, harder question about why the Cart's total updates automatically when quantity changes deep inside a prop array.

### Diagnosed misconception, corrected: no backend call happens yet

First teach-back on the +/- button flow assumed clicking "+" calls the API to update an existing order. Corrected: the cart is currently 100% client-side JavaScript state in `App.vue`, no HTTP request happens on any cart change. This is deliberate groundwork for the next lesson (wiring the cart to real `POST /api/orders` calls), not a bug.

## Concepts to reinforce next (Vue, still new)

- Vue 3 Composition API vs Options API (this project uses Composition API via `<script setup>`, but explaining the distinction is still worth practicing)
- Vue Router, if the frontend grows beyond a single page
- component-level state vs a shared store (e.g. Pinia), if cart state ever needs to be shared beyond `App.vue`'s direct children
- form input binding with `v-model`, needed for the checkout/payment-method step
- chaining multiple `async`/`await` API calls in sequence (create order, then add each item, then pay), and what to do if one call in the middle fails

## Concepts to reinforce later (stretch phase)

- ASP.NET Core authentication/authorization, if Phase 4 adds it
- `WebApplicationFactory` integration testing through the real HTTP pipeline
- deployment of a .NET API and a static Vue build
