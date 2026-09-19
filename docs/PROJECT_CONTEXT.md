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

- What works: full CRUD on MenuItems; complete Orders lifecycle (create, add/remove cart item, pay, receipt); 9 xUnit tests passing; CORS configured for `localhost:5173`; Vue frontend has a working menu list, client-side cart, and a full checkout flow wired to the real backend, all verified end-to-end in browser on the owner's Windows machine (order #4, 6x Adobo Rice Bowl, paid via Card, ₱900.00, real receipt rendered)
- What is incomplete: no styling pass yet (plain unstyled HTML controls); `npm run build` not yet verified; README not yet updated with frontend info/screenshots; no auth; no deployment; no integration tests against the real HTTP pipeline
- Known errors: none currently
- Recent changes: checkout wired to backend (create order, add items, pay, show receipt), an accidental duplicate `src/RestaurantOrderApi.Api copy/` folder was committed and then cleanly removed, docs updated to mark Phase 2 complete
- Last verified command: `dotnet test` — 9/9 passed; full browser checkout verified end-to-end

## Current goal

- Task: Phase 3 of the roadmap, quality and interview readiness
- Definition of done: `npm run build` succeeds, basic clean styling applied, README updated with frontend setup and screenshots, owner can explain the full stack (backend + frontend) out loud without notes
- Constraints: the Sept 22, 2026 IntouchCX call is a recruiter screening, not technical, so there is no hard deadline forcing this before then, but it should be ready in case a technical round follows; ~2-3 days remain before that call
- Files likely involved: all frontend `.vue` files (styling pass), README.md, possibly a shared `frontend/src/api.js` to reduce repeated `fetch` boilerplate across components

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
