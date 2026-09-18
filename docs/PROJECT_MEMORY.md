# Restaurant Order API Project Memory

> Permanent technical handoff and resume point.
>
> Future assistant: read `AGENTS.md` and every document it references before changing the project. Inspect the working tree and tests because GitHub cannot contain uncommitted local work. Never document secrets.

**Last updated:** 2026-09-18
**Repository:** `nickos8/restaurant-order-api`
**Default branch:** `main`
**Latest verified code commit:** `de9a16c` — **Merge: Add CORS policy and Vue menu list page**
**Current phase:** Backend API complete; Vue 3 frontend started, menu list page working end-to-end
**Next exact feature:** Cart state (add/remove item, running total), wired to `POST /api/orders` and `POST /api/orders/{id}/items`
**Working tree at checkpoint:** Local `main` clean and synchronized with `origin/main`

## 1. Purpose

Build a small full-stack restaurant ordering system to demonstrate C# and Vue skills for a specific job application (a Junior Software Developer role preferring C#, with Java and Vue.js as a plus). It recreates, in a stronger form, a college project the owner lost when its laptop broke.

The project should demonstrate:

- C# and ASP.NET Core backend development
- Entity Framework Core and relational database design
- REST API design (CRUD plus a real order lifecycle: cart, checkout, receipt)
- Automated testing (xUnit)
- Vue 3 frontend development (new skill, learned for this project)
- Git and GitHub workflow
- Technical documentation and independent explanation

## 2. Technology and environment

| Layer | Technology | Responsibility |
|---|---|---|
| Backend | ASP.NET Core 8, controller-based | Routes, validation, business logic, JSON |
| ORM | Entity Framework Core 8 | Database access, migrations |
| Database | SQLite | Persistent application data, zero external setup |
| Tests | xUnit + EF Core InMemory provider | Isolated backend verification |
| Frontend | Vue 3 + Vite (planned) | SPA interface, cart state, API requests |
| Version control | Git and GitHub | History, backup, review, handoff |

Repository:

```text
restaurant-order-api/
├── AGENTS.md
├── src/RestaurantOrderApi.Api/
├── tests/RestaurantOrderApi.Tests/
├── frontend/            (planned)
└── docs/
    ├── PROJECT_MEMORY.md
    ├── PROJECT_CONTEXT.md   (quick-scan summary, portable-ai-instructions format)
    ├── LEARNING_LOG.md
    ├── DECISIONS.md
    └── HANDOFF_CHECKLIST.md
```

Windows path:

```text
C:\Users\Niko\restaurant-order-api
```

## 3. Published checkpoints

### Backend scaffold

- `69740e8` — Scaffold Restaurant Order API in C# / ASP.NET Core
- Created `RestaurantOrderApi.Api` (controller-based Web API) and `RestaurantOrderApi.Tests` (xUnit) projects under one solution
- Added EF Core + SQLite, `RestaurantDbContext`, initial migration, seed data (4 menu items)
- Added `MenuItem`, `Order`, `OrderItem`, `OrderStatus` models
- Added `MenuItemsController` (full CRUD, with retire-instead-of-delete when a menu item is referenced by an existing order)
- Added `OrdersController` (create order, add/remove cart item, pay, get receipt), enforcing:
  - a paid order can no longer be modified
  - an order cannot be paid twice
  - an empty order cannot be paid
  - an order line snapshots its unit price at add-time, so a later menu price change never rewrites an already-placed order's total
- Added 9 xUnit tests (unit tests on `Order.Total`, controller tests on cart/pay/receipt rules), all passing
- Manually verified the full flow with curl: select → add to cart → remove from cart → pay → receipt
- Pushed to a private GitHub repository

### Vue frontend, menu list page

- `77f2988` — Add CORS policy and Vue menu list page
- Added `builder.Services.AddCors(...)` in `Program.cs`, policy `FrontendDev` allowing `http://localhost:5173`, applied via `app.UseCors("FrontendDev")` before `MapControllers()`
- Scaffolded `frontend/` with Vue 3 + Vite (no TypeScript, no router, no Pinia, no test runner, no lint/format tooling yet, kept deliberately minimal)
- Built `frontend/src/components/MenuList.vue`: fetches `GET /api/menuitems` in `onMounted()`, stores the result in a `ref`, handles loading/error/success states with `v-if`/`v-else-if`/`v-else`, renders with `v-for`
- Replaced the default `App.vue` boilerplate with `<MenuList />`
- Manually verified in browser: the 4 real seeded menu items render correctly at `localhost:5173` while the API runs at `localhost:5281`
- Owner independently diagnosed and cleaned up stray test data created via Swagger's "Try it out" defaults, understanding it was local dev data, not a code bug
- Committed and pushed; merged cleanly with the documentation commit pushed in parallel

## 4. Domain model

| Entity | Purpose |
|---|---|
| `MenuItem` | `Id`, `Name`, `Category`, `Price`, `IsAvailable` |
| `Order` | `Id`, `CustomerName`, `Status` (Open/Paid/Cancelled), `CreatedAt`, `PaidAt`, `PaymentMethod`, `Items`, computed `Total` |
| `OrderItem` | `Id`, `OrderId`, `MenuItemId`, `Quantity`, `UnitPrice` (snapshotted at add-time) |

## 5. Current backend behavior

### MenuItems (`/api/menuitems`)

Full CRUD. Deleting an item still referenced by an order retires it (`IsAvailable = false`) instead of deleting the row, to preserve order history.

### Orders (`/api/orders`)

```text
POST   /api/orders                       create an order
POST   /api/orders/{id}/items            add an item to the cart (or increase quantity)
DELETE /api/orders/{id}/items/{itemId}   remove or reduce a cart line
POST   /api/orders/{id}/pay              check out; records payment method and timestamp
GET    /api/orders/{id}/receipt          itemized receipt for a paid order
```

Business rules enforced in `OrdersController`:

- cart changes are rejected once an order is `Paid`
- paying twice is rejected
- paying an empty order is rejected
- adding an unavailable menu item is rejected

## 6. Testing

`tests/RestaurantOrderApi.Tests/`:

- `OrderTotalTests.cs` — unit tests on `Order.Total` (empty order, multi-line sum, price snapshot behavior)
- `OrdersControllerTests.cs` — controller tests using EF Core InMemory: add/remove cart quantity math, rejecting an unavailable item, rejecting pay on empty/already-paid orders, rejecting cart edits after payment, rejecting receipt retrieval before payment

Latest result: `dotnet test` — **9 passed, 0 failed**.

## 7. Frontend

Stack: Vue 3 + Vite, calling the existing API directly by full URL (`http://localhost:5281/api/...`) for now; no shared API client module yet (see roadmap).

### Menu list (`frontend/src/components/MenuList.vue`)

Fetches the menu on mount, renders name, category, and price. Handles three states explicitly: loading, error (e.g. API not running), and success. This is the pattern every subsequent page (cart, checkout, receipt) will follow.

See the roadmap below for the remaining build order.

## 8. Verification evidence

### Backend build and tests

```text
dotnet build   → Build succeeded, 0 warnings, 0 errors
dotnet test    → Passed! Failed: 0, Passed: 9, Skipped: 0, Total: 9
```

### Manual API verification (curl, local run)

Verified the full order lifecycle end-to-end: seeded menu returned correctly; order created; two items added to cart (quantities and running total correct); one item quantity reduced (total recalculated correctly); order paid (status, payment method, timestamp recorded); receipt retrieved matching the paid order; a further cart-modification attempt on the paid order correctly returned `400 Bad Request`.

### Local run verified on the owner's Windows machine (VS Code)

- `.NET SDK 8.0.131` confirmed via `dotnet --version` (later environment showed a newer global SDK, `10.0.401`, which also worked — the project's `.csproj` targets `net8.0` explicitly so this is not a concern)
- `dotnet run` applied migrations, seeded the menu, and started listening on `http://localhost:5281`
- Swagger UI reachable at `/swagger`

### Git checks

- exact intended files staged (no `bin/`, `obj/`, `*.db` committed)
- commit `69740e8` pushed successfully to `origin/main`
- local `main` confirmed clean and synchronized with `origin/main`

## 9. Current status

| Area | Status |
|---|---|
| Solution and project scaffold | Complete |
| Domain model | Complete |
| EF Core + SQLite + migration | Complete |
| MenuItems CRUD | Complete |
| Orders: create/cart/pay/receipt | Complete |
| Business rule enforcement | Complete |
| xUnit test suite | Complete (9/9) |
| Manual API verification | Complete |
| README | Complete |
| Pushed to GitHub (private) | Complete |
| Vue 3 frontend scaffold | Complete |
| CORS policy for the frontend origin | Complete |
| Menu list page | Complete |
| Cart UI | Not started |
| Checkout UI | Not started |
| Receipt view | Not started |
| Frontend build/lint verification | Not started |
| Screenshots in README | Not started |
| Deployment | Not planned yet (stretch) |
| Authentication | Not planned yet (stretch) |

## 10. Exact resume procedure

1. Read `AGENTS.md` and all linked documentation.
2. From the project root:

```cmd
git status --short --untracked-files=all
git log --oneline --decorate -10
git fetch origin
git status -sb
```

3. If the tree is clean and documentation changes have been merged remotely, synchronize with:

```cmd
git pull --ff-only origin main
```

4. Re-run verification if the environment or code has changed:

```cmd
dotnet build
dotnet test
```

5. Begin (or continue) the Vue frontend by first inspecting:
   - `src/RestaurantOrderApi.Api/Controllers/MenuItemsController.cs`
   - `src/RestaurantOrderApi.Api/Controllers/OrdersController.cs`
   - `src/RestaurantOrderApi.Api/Dtos/*.cs`
   - whatever exists so far under `frontend/`

6. Teach Vue fundamentals (components, reactive state, calling a REST API) bridging from the owner's existing React knowledge before writing the full pages.

## 11. Roadmap

### Phase 1 — Backend API (complete)

- [x] Solution, API project, test project scaffold
- [x] Domain model (MenuItem, Order, OrderItem, OrderStatus)
- [x] EF Core + SQLite + initial migration + seed data
- [x] MenuItems CRUD
- [x] Orders: create, add/remove cart item, pay, receipt
- [x] Business rules (no edits after paid, no double pay, no empty-order pay, price snapshot)
- [x] xUnit test suite (9 passing)
- [x] README
- [x] Pushed to GitHub (private)
- [x] Verified running locally on the owner's Windows machine via VS Code

### Phase 2 — Vue 3 frontend (in progress)

- [x] Scaffold `frontend/` with Vue 3 + Vite
- [x] Configure the API to allow the Vite dev server origin via CORS
- [x] Menu list page (`GET /api/menuitems`)
- [ ] Cart state: add item, remove/reduce item, running total
- [ ] Create order and add cart items to it (`POST /api/orders`, `POST /api/orders/{id}/items`)
- [ ] Checkout page (`POST /api/orders/{id}/pay`)
- [ ] Receipt view (`GET /api/orders/{id}/receipt`)
- [ ] Loading, empty, and error states
- [ ] Basic, clean styling (no framework required, but a lightweight one is fine)

### Phase 3 — Quality and interview readiness

- [ ] `npm run build` verified (production bundle succeeds)
- [ ] Manual end-to-end browser test of the full flow: browse menu → cart → checkout → receipt
- [ ] README updated with frontend setup instructions and screenshots
- [ ] `docs/PROJECT_MEMORY.md` and `docs/DECISIONS.md` updated with frontend decisions
- [ ] Practice explaining the architecture end-to-end (backend + frontend) out loud

### Phase 4 — Stretch (only if time remains after Phase 3)

- [ ] MenuItems admin CRUD UI
- [ ] Simple authentication
- [ ] Deployment (e.g. a free-tier host for the API and static hosting for the Vue build)
- [ ] Integration tests against the real HTTP pipeline (`WebApplicationFactory`)

## 12. Related documentation

- `AGENTS.md`: permanent AI teaching and safety contract
- `docs/PROJECT_CONTEXT.md`: short, frequently-updated status snapshot (portable-ai-instructions format)
- `docs/LEARNING_LOG.md`: confirmed learning and concepts to reinforce
- `docs/DECISIONS.md`: architectural decisions and tradeoffs
- `docs/HANDOFF_CHECKLIST.md`: safe resume, pause, commit, and assistant-change procedure

Update this memory only from verified evidence. Never include secrets.
