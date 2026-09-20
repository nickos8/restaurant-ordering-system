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

- What works: full CRUD on MenuItems; complete Orders lifecycle (create, add/remove cart item, pay, receipt); 10 xUnit tests passing (one written from scratch by the owner); CORS configured for `localhost:5173`; Vue frontend has a working, styled menu list, client-side cart, and a full checkout flow wired to the real backend, all verified end-to-end in browser (order #4, 6x Adobo Rice Bowl, paid via Card, ₱900.00, real receipt rendered); `npm run build` verified (686ms, clean production bundle)
- What is incomplete: README not yet updated with frontend info/screenshots; no auth; no deployment; no integration tests against the real HTTP pipeline; interview-talk practice not yet done
- Known errors: none currently
- Recent changes: styling pass applied (CSS custom properties, no framework), production build verified, owner learned xUnit fundamentals and wrote + verified their own test from scratch, a full read-only code audit was produced (`REVIEW_INPUT.md`, untracked, awaiting the owner's decision on whether to commit it publicly or keep it private)
- Last verified command: `dotnet test` — 10/10 passed; `npm run build` succeeded; full browser checkout verified end-to-end

## Current goal

- Task: finish Phase 3, then shift fully into interview prep
- Definition of done: README updated with frontend setup and a screenshot, owner can explain the full stack (backend + frontend) out loud without notes, decision made on `REVIEW_INPUT.md` (commit or keep private)
- Constraints: the Sept 22, 2026 IntouchCX call is a recruiter screening, not technical, so there is no hard deadline forcing this before then, but it should be ready in case a technical round follows; ~2 days remain before that call
- Files likely involved: README.md (add frontend section + screenshot); no more component code changes expected unless something breaks

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
