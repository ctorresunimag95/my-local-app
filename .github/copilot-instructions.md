# Copilot instructions for this repository

## Build, test, and lint commands

### .NET solution

- Build the full solution: `dotnet build Cinema.slnx -nologo`
- Run the functional test suite that boots the distributed app through Aspire: `dotnet test Cinema.Functional.Tests\Cinema.Functional.Tests.csproj -nologo`
- Run a single .NET test: `dotnet test Cinema.Functional.Tests\Cinema.Functional.Tests.csproj --filter "FullyQualifiedName~ManagamentTests.GetWebResourceRootReturnsOkStatusCode" -nologo`

### Angular web app

- Install dependencies: `npm --prefix Cinema.Web install`
- Build the web app: `npm --prefix Cinema.Web run build`
- Run unit tests once: `npm --prefix Cinema.Web test -- --watch=false --browsers=ChromeHeadless`
- Run a single Angular spec file: `npm --prefix Cinema.Web test -- --watch=false --browsers=ChromeHeadless --include="src/app/reservation/services/reservation.service.spec.ts"`

### Playwright e2e tests

- Install dependencies: `npm --prefix e2e install`
- Run all e2e tests: `npm --prefix e2e test`
- Run a single Playwright spec file: `npm --prefix e2e exec playwright test tests/movies.spec.ts`
- Run a single Playwright test by title: `npm --prefix e2e exec playwright test -g "Toy Story"`

### Linting / static analysis

- There is no separate lint script in the repo.
- .NET analyzer and code-style checks run as part of `dotnet build` via `Directory.Build.props`.

## High-level architecture

- `Cinema.AppHost` is the source of truth for local orchestration. It wires the distributed app together with .NET Aspire, starts the backend services, gateway, Angular app, SQL Server, Redis, MailPit, and development-time Azure Service Bus / Cosmos emulators, and defines AppHost-only commands like SQL movie cleanup and reservation cache invalidation.
- `Cinema.Gateway` is a thin YARP reverse proxy. It exposes `/api/management/*` and `/api/reservation/*`, then forwards to the internal `management` and `reservation` services through Aspire service discovery.
- `Cinema.Web` never talks to backend services directly. Frontend API calls go through the gateway-relative base path `gateway/api`, and `proxy.conf.js` rewrites `/gateway` to the AppHost-provided gateway endpoint during local development.
- `Cinema.Management` is the write side for movies. It stores movie records in Cosmos DB, enriches search requests through the OMDb API, and publishes `movie.management.topic` Service Bus events after a movie is published.
- `Cinema.Reservation` is a read/projection service. It persists movies in SQL Server through EF Core, consumes the movie-published topic subscription, and serves movie reads from FusionCache backed by Redis.
- `Cinema.Reservation.MigrationWorker` applies reservation database migrations before the reservation API is allowed to start; AppHost explicitly waits for it.
- `Cinema.Notification` is another Service Bus consumer of the same movie-published topic. It sends notification emails through MailPit rather than participating in the request/response path.
- `Cinema.ServiceDefaults` centralizes cross-service defaults: OpenTelemetry, service discovery, resilient `HttpClient` behavior, and development health endpoints. Management and Reservation also share the same ProblemDetails exception handler from this project.

## Key conventions

- Prefer running and testing the system through `Cinema.AppHost` when the change crosses service boundaries. The functional tests already follow this pattern with `Aspire.Hosting.Testing` and a gateway client instead of booting individual services manually.
- Keep HTTP surface area in endpoint extension classes (`MapMoviesEndpoints`) and keep bootstrapping in each service `Program.cs`. New API routes should follow the existing minimal-API grouping pattern under `movies`.
- For frontend API calls, use `environment.apiUrls.cinema` and append `/management` or `/reservation` in the Angular service classes. Do not hardcode backend hostnames or ports in components or services.
- Movie publication is event-driven: publishing in `Cinema.Management` must stay consistent with the downstream Reservation and Notification consumers that listen on `movie.management.topic`.
- Reservation caching is tag-based. Reads cache under the `"movies"` tag, and any code path that changes reservation-side movie state should invalidate that tag rather than deleting individual Redis keys manually.
- `Program` is intentionally declared as a public partial type in the ASP.NET entry points used by tests. Preserve that pattern when changing startup code so `WebApplicationFactory` and Aspire-based tests can still bootstrap the apps.
- OMDb configuration is expected through `omdb:apiUrl` and `omdb:apiKey`; for local Aspire runs the key is loaded from AppHost user secrets rather than committed config.
