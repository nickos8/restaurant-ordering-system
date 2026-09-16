# Project Context

## Project identity

- Project name: Restaurant Order API
- Purpose: C# / ASP.NET Core portfolio project to demonstrate backend skills for job interviews (built to cover the JD's "C# preferred" requirement)
- Intended users: interviewers reviewing the GitHub repo; later, a learning reference
- Repository: not yet pushed to GitHub (pending)
- Current branch: main (local only so far)
- Local project path: /home/user/RestaurantOrderApi

## Technology

- Frontend: none (API only)
- Backend: ASP.NET Core 8 Web API, controller-based
- Database: SQLite via EF Core, code-first migrations
- Hosting or deployment: not deployed yet, local only
- Important versions: .NET SDK 8.0.131, EF Core 8.0.11

## Current state

- What works: full CRUD on MenuItems; Orders support create, add item to cart, remove/reduce cart item, pay, get receipt; a paid order rejects further cart changes; 9 xUnit tests passing (unit tests on Order.Total, controller tests on cart/pay/receipt rules)
- What is incomplete: not pushed to GitHub yet; no auth; no deployment; no integration tests against the real HTTP pipeline (controller tests call the controller directly with an InMemory DbContext)
- Known errors: none currently
- Recent changes: initial scaffold, domain model, controllers, EF Core wiring, migration, manual smoke test via curl, xUnit test suite
- Last verified command: `dotnet test` at repo root — 9/9 passed

## Current goal

- Task: get this project interview-ready before the Sept 22, 2026 IntouchCX screening
- Definition of done: pushed to a public GitHub repo with a clean README, ideally a couple more polish items (see Next step)
- Constraints: ~5 days remaining; keep scope small and finished rather than broad and half-done
- Files likely involved: src/RestaurantOrderApi.Api/**, tests/RestaurantOrderApi.Tests/**, README.md

## Decisions already made

- Decision: Controller-based API, not Minimal API
- Reason: closer to what C# interviews and most job listings expect
- Alternatives rejected: Minimal API (less boilerplate but less standard for interview framing)

- Decision: SQLite over SQL Server/Postgres
- Reason: zero external setup, runs anywhere, fine for a portfolio project
- Alternatives rejected: SQL Server (heavier setup), in-memory only (loses the "real database" story)

- Decision: snapshot unit price onto each OrderItem at add-time
- Reason: an order's total must not silently change if the menu price changes later
- Alternatives rejected: always compute total from the live MenuItem.Price

## Safety and privacy

- No secrets or `.env` values in this project (none needed; connection string is a local SQLite file path).
- Repository will be public once pushed.

## Next step

- Exact next action: create the GitHub repo and push (needs user confirmation before push, per Git safety rules)
- Command or file to inspect: `git status` in /home/user/RestaurantOrderApi once initialized
- Question that remains: repo name and whether to add a couple of stretch items (e.g. GET /api/orders filtering by status, or a simple auth layer) before Sept 22, given time budget
