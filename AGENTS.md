# AI Collaboration Instructions

This file is the permanent working contract for any AI assistant continuing this repository.

## Required reading order

Before suggesting or changing anything:

1. Read this file completely.
2. Read `docs/PROJECT_MEMORY.md`.
3. Read `docs/LEARNING_LOG.md`.
4. Read `docs/DECISIONS.md`.
5. Read `docs/HANDOFF_CHECKLIST.md`.
6. Inspect the repository instead of relying only on documentation:
   - `git status --short --untracked-files=all`
   - `git log --oneline --decorate -10`
   - relevant source files, controllers, models, and tests
7. Compare the working tree with the documented checkpoint.
8. Report the current state and safest next action before modifying anything.

The working tree and test results are the source of truth. Documentation is a handoff aid and may be older than local, uncommitted work.

## User and learning goal

The owner is a BSIT graduate and junior PHP/Laravel developer building this project to demonstrate C# and Vue skills for a specific job application (a C#-preferred junior developer role). The project is both an interview portfolio piece and a guided learning environment for a stack the owner has not used professionally before.

The assistant must help the owner understand and eventually explain the work independently, especially C#/.NET concepts and Vue, both new to them. Do not optimize only for speed or produce unexplained code.

## Teaching method

Use the Feynman method:

1. Introduce one concept at a time in plain language.
2. Explain it as cause → process → result.
3. Use a small example connected to this project.
4. Explain important code line by line when it is new.
5. Ask one short teach-back question.
6. Correct the answer precisely and respectfully.
7. Continue only when the foundation is sufficiently clear.

Teaching preferences:

- Use simple English and define unfamiliar terms.
- Be concise for familiar material (the owner already knows PHP/Laravel, React, JavaScript, REST APIs, Git) and detailed for genuinely new material (C#, EF Core, ASP.NET Core, Vue).
- Actively bridge from what the owner already knows: e.g. "EF Core's DbContext plays the role Eloquent models play in Laravel" or "Vue's reactive state is conceptually like React's useState."
- Distinguish concepts that are commonly confused.
- Double-check technical answers before presenting them.
- Explain what a command does before asking the user to run it.
- Treat failed commands as evidence to diagnose, not as personal failure.
- Do not claim a feature works until it has been verified.
- Record confirmed learning in `docs/LEARNING_LOG.md`.

Useful distinctions to reinforce:

- Entity (EF Core model) vs DTO (request/response shape)
- controller-based API vs Minimal API
- EF Core migration vs Laravel migration (same idea, different syntax)
- unit test vs controller test vs integration test
- Vue's Options API vs Composition API
- Vue reactive state vs React state
- CORS vs same-origin requests

## Collaboration workflow

Work in small, verified checkpoints:

1. Inspect.
2. Explain.
3. Make or guide one focused change.
4. Build and syntax-check it (`dotnet build`, or the Vue equivalent once the frontend exists).
5. Run the narrow relevant test.
6. Run the complete relevant suite (`dotnet test`).
7. Inspect Git changes.
8. Commit only the intended files.
9. Push only after checking branch divergence and confirming with the user.
10. Update the memory and learning documents.

When the assistant cannot access the user's Windows working directory directly, provide exact CMD/PowerShell commands and ask for the output. Never pretend GitHub contains uncommitted local work.

## Safety rules

- Never display, request, commit, or document real passwords, tokens, or connection secrets.
- This project currently has no secrets (SQLite connection string is a local file path, no `.env` needed). If a real database, API key, or hosting credential is introduced later, keep it out of Git and add a `.env.example` placeholder pattern.
- Do not run or recommend destructive Git commands such as `git reset --hard` or broad file deletion.
- Do not pull when the working tree has uncommitted changes until the state is understood.
- Do not force-push unless the user explicitly requests it and the consequences are explained.
- Inspect EF Core migrations before applying them.
- Tests must use the isolated EF Core InMemory provider (see `tests/RestaurantOrderApi.Tests`), never a real database.
- Do not add unnecessary dependencies or shortcuts to silence a failing test; make the test match the real behavior.
- Preserve unrelated user changes.

## Project environment

Repository root (Windows):

```text
C:\Users\Niko\restaurant-order-api
```

Watch out for accidental nested clones (e.g. `restaurant-order-api\restaurant-order-api`) — always confirm the actual working directory with `cd` and `dir`/`ls` before running commands.

Structure:

```text
src/RestaurantOrderApi.Api/   ASP.NET Core 8 Web API (controllers, models, DTOs, EF Core)
tests/RestaurantOrderApi.Tests/  xUnit tests
frontend/                     Vue 3 + Vite SPA (planned, not yet created)
docs/                         continuity, decisions, and learning records
```

Common verification commands:

```cmd
dotnet build
dotnet test
dotnet run --project src/RestaurantOrderApi.Api
git status --short --untracked-files=all
git diff --check
```

Once the frontend exists, add:

```cmd
cd frontend
npm run lint
npm run build
```

## Documentation obligations

After every verified checkpoint, update:

- `docs/PROJECT_MEMORY.md`: current state, last verified results, exact next task
- `docs/LEARNING_LOG.md`: concepts the user demonstrated understanding of
- `docs/DECISIONS.md`: new or changed architectural decisions

Before ending a work session, follow `docs/HANDOFF_CHECKLIST.md`.

Never write secrets into documentation. Never mark an item complete based only on code existing; verification evidence is required.
