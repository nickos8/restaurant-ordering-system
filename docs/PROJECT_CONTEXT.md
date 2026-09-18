# Project Context

## Project identity

- Project name: Restaurant Order API
- Purpose: C# / ASP.NET Core + Vue portfolio project to demonstrate backend and frontend skills for a specific job application (JD prefers C#, lists Vue.js as a plus)
- Intended users: interviewers reviewing the GitHub repo; later, a learning reference
- Repository: `nickos8/restaurant-order-api` (private, pushed)
- Current branch: main, pushed and up to date with origin
- Local project path (Windows): C:\Users\Niko\restaurant-order-api

## Technology

- Frontend: none yet, Vue 3 + Vite planned (Phase 2)
- Backend: ASP.NET Core 8 Web API, controller-based
- Database: SQLite via EF Core, code-first migrations
- Hosting or deployment: not deployed yet, local only
- Important versions: .NET SDK 8.0.131 (project targets net8.0), EF Core 8.0.11

## Current state

- What works: full CRUD on MenuItems; Orders support create, add item to cart, remove/reduce cart item, pay, get receipt; a paid order rejects further cart changes; 9 xUnit tests passing; CORS policy configured for `localhost:5173`; Vue 3 frontend scaffolded and running; menu list page fetches and renders the real seeded menu end-to-end, verified in browser on the owner's Windows machine
- What is incomplete: cart state, checkout, and receipt pages not built yet; no auth; no deployment; no integration tests against the real HTTP pipeline
- Known errors: none currently
- Recent changes: menu list page built and working; CORS added; docs merged and pushed cleanly alongside the owner's frontend commit
- Last verified command: `dotnet test` — 9/9 passed; browser confirmed the menu list renders the 4 real seeded items at `localhost:5173`

## Current goal

- Task: build cart state next (Phase 2 of the roadmap in docs/PROJECT_MEMORY.md)
- Definition of done: add/remove items to a cart, running total, wired to `POST /api/orders` and `POST /api/orders/{id}/items`
- Constraints: owner is new to Vue; the Sept 22, 2026 IntouchCX call is a recruiter screening, not technical, so there is no hard deadline forcing this before then, but it should be ready in case a technical round follows
- Files likely involved: new frontend/src/components/Cart.vue; possibly a shared api.js helper; App.vue to wire menu selection into the cart

## Decisions already made

See docs/DECISIONS.md for the full record. Summary:

- Controller-based API, not Minimal API (matches interview expectations)
- SQLite over SQL Server/Postgres (zero setup, portable)
- Snapshot unit price onto each OrderItem at add-time (correct order-total behavior)
- Retire instead of delete a MenuItem referenced by an existing order (preserves history)
- Vue 3 + Vite for the frontend, not React (directly covers the JD's stated preference, and avoids duplicating existing React evidence from developer-portfolio)

## Safety and privacy

- No secrets or `.env` values in this project (none needed; connection string is a local SQLite file path).
- Repository is currently private; revisit going public once Phase 2/3 progress further.

## Next step

- Exact next action: scaffold frontend/ with Vue 3 + Vite, teaching Vue fundamentals bridged from the owner's React experience, starting with a menu list page
- Command or file to inspect: docs/PROJECT_MEMORY.md section 11 (Roadmap, Phase 2) for the exact build order
- Question that remains: none blocking; proceed with Phase 2 when the owner is ready
