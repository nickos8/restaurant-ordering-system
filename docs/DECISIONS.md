# Technical Decisions

> Records important architectural choices and why they were made. Update an entry when a decision changes instead of silently replacing its history.

## Project scope

### Restaurant ordering domain, not a generic CRUD demo

**Decision:** Model a real order lifecycle (menu selection, cart, checkout, receipt) instead of a single flat CRUD resource.

**Reason:** Mirrors a real college project the owner lost, and demonstrates business-rule logic (payment state, price snapshotting), not just basic CRUD, which is a stronger interview talking point.

## Backend framework

### Controller-based ASP.NET Core, not Minimal API

**Decision:** Use `[ApiController]` classes with attribute routing rather than Minimal API endpoint mapping.

**Reason:** Closer to what most C# job interviews and tutorials expect. The owner is learning C# specifically for interviews, so matching common patterns matters more than minimizing boilerplate.

**Alternatives rejected:** Minimal API — less boilerplate, but less standard framing for interview conversations.

### .NET 8 (LTS)

**Decision:** Target `net8.0` explicitly in the `.csproj` files.

**Reason:** .NET 8 is a Long Term Support release and the most likely version referenced in job listings and interviews at the time this project was built.

## Database

### SQLite via EF Core, not SQL Server or PostgreSQL

**Decision:** Use SQLite as the relational database, accessed through EF Core code-first migrations.

**Reason:** Zero external setup (no server to install or configure), runs identically on any machine that clones the repo, and is fully sufficient to demonstrate relational modeling, migrations, and EF Core competence for a portfolio project.

**Alternatives rejected:** SQL Server (heavier local setup, less portable for anyone reviewing the repo), PostgreSQL (adds an external dependency with no benefit at this project's scale), in-memory only (loses the "real persisted database" story and migrations).

**Revisit when:** the project is deployed publicly and a hosted database becomes worthwhile.

## Domain modeling

### Snapshot unit price onto each OrderItem at add-time

**Decision:** `OrderItem.UnitPrice` is copied from `MenuItem.Price` when the item is added to the cart, and `Order.Total` is computed only from stored `OrderItem` rows.

**Reason:** An order's total must never silently change if the menu price changes later. This is standard e-commerce/POS practice and a good detail to explain in an interview.

**Alternatives rejected:** Always computing the total from the live `MenuItem.Price` — simpler, but incorrect once prices change after an order is placed.

### Retire instead of delete for a menu item already used in an order

**Decision:** `DELETE /api/menuitems/{id}` sets `IsAvailable = false` instead of removing the row when the item is referenced by any `OrderItem`.

**Reason:** Deleting the row would break the foreign key and destroy order history. The `OrderItem.MenuItemId` relationship uses `DeleteBehavior.Restrict` for the same reason.

**Alternatives rejected:** Cascading delete (would silently corrupt historical orders), hard block with an error (less useful than automatically retiring the item).

## Testing

### xUnit with EF Core InMemory provider for controller tests

**Decision:** `OrdersControllerTests` instantiate the real `OrdersController` against a fresh `RestaurantDbContext` backed by `UseInMemoryDatabase(Guid.NewGuid().ToString())` per test.

**Reason:** Tests the actual business logic in the controller (cart math, payment rules) without needing a real SQLite file or a running HTTP server, keeping tests fast and isolated.

**Alternatives rejected:** `WebApplicationFactory` integration tests through the real HTTP pipeline — more realistic but heavier to set up; may be added later (see roadmap Phase 4) once the core logic is stable.

**Known limitation:** InMemory is not a full relational database (no real foreign-key enforcement, no SQL translation checks). This is acceptable for testing business logic but should be named honestly if asked in an interview.

## Frontend (planned)

### Vue 3 + Vite, not React

**Decision:** Build the frontend in Vue 3 with Vite, even though the owner's existing portfolio work uses React.

**Reason:** The target job description lists Vue.js as a plus alongside C#. Building in Vue directly demonstrates the actual preferred stack rather than reusing React, which the owner already has evidence of elsewhere (`developer-portfolio`).

**Alternatives rejected:** React (would duplicate existing portfolio evidence instead of covering a gap), no frontend at all (leaves the JD's Vue preference unaddressed).

**Revisit when:** not applicable — this is the committed direction for this project.

### CORS will be configured explicitly for the Vite dev server origin

**Decision status:** Planned, not yet implemented.

**Current direction:** Add an explicit CORS policy in `Program.cs` allowing the local Vite dev server origin (typically `http://localhost:5173`) with credentials disabled (no cookies/auth planned for the core project).

**Reason:** The API and frontend run on different local ports during development, so the browser will block requests without an explicit CORS policy.

## Version control and handoff

### Public interview link, private repo for now

**Decision:** Keep `restaurant-order-api` private while under active development; make public once Phase 2/3 are further along.

**Reason:** Avoids showing an unfinished or thin repo to anyone who might look it up early. Revisit before sharing the link with an interviewer.

### Documentation structure mirrors `developer-portfolio`

**Decision:** Use the same `AGENTS.md` + `docs/PROJECT_MEMORY.md` + `docs/DECISIONS.md` + `docs/HANDOFF_CHECKLIST.md` + `docs/LEARNING_LOG.md` structure as the owner's other active project.

**Reason:** Consistency across the owner's projects makes both easier to resume, and reuses a documentation habit the owner has already found effective, rather than inventing a new one per project.
