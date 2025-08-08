# OpenRaceView – AI Coding Agent Instructions

Concise, project-specific guidance so an AI agent can be productive quickly. Focus on THIS repo's patterns (avoid generic advice).

## 1. Architecture Snapshot
- Monorepo with `backend/` (ASP.NET Core, planned clean architecture layers) + `frontend/` (SvelteKit + Tailwind + MapLibre + Chart.js).
- Backend projects (currently stubs except WeatherForecast):
  - `OpenRaceView.API` – Web API host (Program.cs, controllers, DI setup, OpenAPI). 
  - `OpenRaceView.Domain` – Domain entities + core logic (empty now – create Lap, LapSample soon).
  - `OpenRaceView.Application` – CQRS/MediatR handlers, DTOs, validation (empty now).
  - `OpenRaceView.Infrastructure` – EF Core DbContext, persistence, external services (empty now).
- Frontend uses SvelteKit filesystem routing; example pages: `/laps`, `/laps/[id]`, `/upload` using fetch calls to `/api/*` (dev proxy or same origin expected).
- Mapping library: MapLibre in `src/lib/components/LapMap.svelte` with hard-coded Nürburgring coordinates (to be replaced with fetched lap samples).

## 2. Current State vs Roadmap
- Only demo WeatherForecast endpoint exists. Core lap persistence feature spec lives at `docs/issues/backend-lap-persistence.md` – FOLLOW it when scaffolding backend.
- README references PostgreSQL + PostGIS, Identity, MediatR – not yet implemented. Default early implementation may use SQLite unless explicit Postgres config added.

## 3. Backend Conventions (Intended)
- Layered: API -> Application (MediatR commands/queries) -> Domain (entities, invariants) -> Infrastructure (EF Core).
- Add new endpoints via a controller in `OpenRaceView.API/Controllers/*Controller.cs` mapping to MediatR handlers (once MediatR wired).
- DTO naming: *Request (input), *Dto (output), Commands/Queries: Verb + Entity (e.g., `CreateLapCommand`).
- Validation: Prefer FluentValidation or manual guard clauses in handler; return 400 for invalid payloads.
- **Explicit typing**: Do not use `var` keyword - always use explicit type declarations for clarity.

## 4. Lap Persistence Implementation Pointers
When implementing the lap feature:
1. Add EF Core + provider to Infrastructure csproj; register DbContext in Program.cs.
2. Domain entities: `Lap` (aggregate root), `LapSample` (child). Keep collections ordered by sample index.
3. Use relative millisecond offsets for samples plus lap StartTimeUtc.
4. Config-driven limit: `Telemetry:MaxSamplesPerLap` (add to `appsettings.Development.json`). Reject payloads exceeding it.
5. Controller routes (under `/api/laps`): POST create, GET list, GET detail with optional `includeSamples=true`.
6. Return 201 with `Location` header on create.
7. Deletion (future): Hard delete only (if added).

## 5. Frontend Patterns
- Data fetch: Direct `fetch('/api/laps')` in `+page.svelte` (client-side). For SEO or SSR, shift to page `load` function returning data.
- For per-lap pages, `+page.ts` currently returns only `id`; extend to fetch lap detail JSON including samples for `LapMap`.
- Map rendering expects Geo coordinates array; transform backend samples to `[lon, lat]` pairs for MapLibre sources.

## 6. Adding Telemetry to Map
- Create a GeoJSON LineString source from samples; add layer `type: line` for track outline, and a circle/symbol layer for animated position (future).
- Store processed GeoJSON in a Svelte store if reused across components.

## 7. Build & Run (Assumptions)
Backend (dev):
```pwsh
cd backend/OpenRaceView.API
# (After adding EF + migrations)
dotnet ef database update
dotnet run
```
Frontend:
```pwsh
cd frontend
npm install
npm run dev
```
Front-end dev server at http://localhost:5173; assume API at https://localhost:5001 or http://localhost:5000 unless proxied.

## 8. Introducing EF Core & Migrations
- Add package refs (e.g., `Microsoft.EntityFrameworkCore`, provider like `Microsoft.EntityFrameworkCore.Sqlite` or `Npgsql.EntityFrameworkCore.PostgreSQL`).
- Create `ApplicationDbContext` (or `TelemetryDbContext`); add DbSets for Lap, LapSample.
- Use `dotnet ef migrations add InitialLap` then `update`.

## 9. Testing Strategy (Initial Minimal)
- Add test project later; for now, create simple handler/unit tests if you scaffold Application layer (focus on Lap factory invariants: ordered offsets, max count, non-empty samples).

## 10. Performance & Future Hooks
- For now EF AddRange ok for samples; consider bulk insert later (see spec follow-up issues).
- Plan for optional raw file storage (deferred) – interface stub in Infrastructure.

## 11. PR / Change Guidelines for Agents
- Align with spec in `docs/issues/backend-lap-persistence.md`; reference tasks you implement.
- Keep public API stable unless coordinating with frontend changes (simultaneous PR modifications acceptable in monorepo).
- Prefer small, focused commits: (1) scaffolding packages, (2) domain/entities, (3) persistence + migration, (4) API endpoints, (5) frontend wiring.

## 12. Avoid / Do Not
- Do not introduce authentication/Identity until a dedicated issue exists.
- Do not implement raw file upload endpoint yet.
- Avoid premature PostGIS/spatial geometry; stick to lat/lon doubles now.

## 13. Useful File References
- Spec: `docs/issues/backend-lap-persistence.md`
- API entrypoint: `backend/OpenRaceView.API/Program.cs`
- Frontend lap list: `frontend/src/routes/laps/+page.svelte`
- Frontend lap detail placeholder: `frontend/src/routes/laps/[id]/+page.svelte`
- Map component: `frontend/src/lib/components/LapMap.svelte`

---
