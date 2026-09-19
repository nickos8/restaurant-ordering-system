# AI and Developer Handoff Checklist

Use this checklist whenever work resumes, pauses, changes assistants, or reaches a Git checkpoint.

## Starting a session

1. Read:
   - `AGENTS.md`
   - `docs/PROJECT_MEMORY.md`
   - `docs/LEARNING_LOG.md`
   - `docs/DECISIONS.md`
2. From the repository root, run:

```cmd
git status --short --untracked-files=all
git log --oneline --decorate -10
git branch --show-current
```

3. If GitHub access is available, compare the local branch with the remote.
4. Do not run `git pull` when local changes exist until the divergence is understood.
5. Inspect the source files involved in the next task.
6. State:
   - verified completed work
   - uncommitted work
   - failing or missing verification
   - exact next safe action

## During development

For each focused change:

1. Explain the concept and intended result.
2. Inspect existing code before editing.
3. Make the smallest coherent change.
4. Build and check the changed project:

```cmd
dotnet build
```

5. Run the narrow relevant test:

```cmd
dotnet test --filter FullyQualifiedName~OrdersControllerTests
```

6. Diagnose failures from the first meaningful error.
7. Run the complete backend suite after focused tests pass:

```cmd
dotnet test
```

8. For frontend changes (once `frontend/` exists):

```cmd
cd frontend
npm run lint
npm run build
```

9. Inspect:

```cmd
git diff --check
git status --short --untracked-files=all
```

## Security check before staging

Confirm that none of these are included:

- any real `.env` or secret file
- real passwords, tokens, or connection secrets
- `bin/`, `obj/` (backend build output)
- `node_modules/`, `dist/` (once the frontend exists)
- `*.db`, `*.db-shm`, `*.db-wal` (local SQLite database files)
- unrelated archives or temporary files
- accidental duplicate folders (e.g. a stray "... copy" or "... (1)" folder from an editor's file explorer, run `git status --short --untracked-files=all` and actually read the file list before staging, not just `git add .` on faith; this has happened once already in this project, see `docs/DECISIONS.md`)

## Staging and committing

1. Stage exact intended files rather than broad unknown directories.
2. Run:

```cmd
git status --short
git diff --cached --check
git diff --cached --stat
```

3. Review the staged set.
4. Commit with a concise outcome-based message.
5. Run:

```cmd
git fetch origin
git status -sb
git log --oneline --decorate -5
```

6. Resolve divergence safely before pushing.
7. Push only a verified commit, and only after explicit confirmation from the owner (per this project's git safety rules).

## Pausing before a commit

If work remains uncommitted, record in `docs/PROJECT_MEMORY.md`:

- every modified and untracked path
- successful checks and their results
- failing command and first meaningful error
- whether the working tree is safe
- the exact next command
- a warning not to pull if appropriate

## Completing a checkpoint

Update:

### `docs/PROJECT_MEMORY.md`

- last verified commit
- current phase
- completed features
- test/build evidence
- current working-tree state
- exact next task
- roadmap checkboxes

### `docs/LEARNING_LOG.md`

- concepts the user successfully explained
- corrected misconceptions
- concepts needing reinforcement (especially C# and Vue, both new to the owner)

### `docs/DECISIONS.md`

- new architectural choices
- changed decisions and reasons
- known tradeoffs

### `docs/PROJECT_CONTEXT.md`

- refresh the short status snapshot to match `PROJECT_MEMORY.md`

Then inspect, commit, and publish the documentation.

## Changing AI assistants

Give the new assistant this instruction:

```text
Continue my restaurant-order-api project.

Read AGENTS.md and every document it lists. Then inspect the current Git branch,
Git status, recent commits, relevant source files, controllers, models, and tests.
Do not modify, pull, commit, push, migrate, or delete anything yet.

Explain the verified project state, anything that exists only as uncommitted local
work, the last verification evidence, and the exact safest next action. Follow the
Feynman teaching method and documentation rules in AGENTS.md. Remember C# and Vue
are both new to the owner, so bridge new concepts from their existing PHP/Laravel
and React experience.
```

The new assistant must not assume that GitHub contains local uncommitted work.

## Current Windows path

```text
Repository: C:\Users\Niko\restaurant-order-api
```

Double-check the actual clone path before running commands — a nested clone folder (for example `restaurant-order-api\restaurant-order-api`) has been seen before in this project and will cause "no project found" errors if `dotnet` commands run from the wrong level.

## Emergency rule

When documentation, GitHub, and the working tree disagree:

1. stop mutations
2. preserve every file
3. inspect Git status, diffs, branches, and commits
4. identify which state contains the newest work
5. explain the evidence
6. choose a recoverable reconciliation method

Never use destructive commands to make the mismatch disappear.
