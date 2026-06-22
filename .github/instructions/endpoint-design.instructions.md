---
applyTo: "**/*Endpoint.cs,**/*Endpoints.cs"
---

## Structure

- One `static` class per feature area, named `{Feature}Endpoint` or `{Feature}Endpoints`, placed in the feature folder (e.g. `Cinema.Management/Movies/MoviesEndpoint.cs`).
- Expose a single public extension method `Map{Feature}Endpoints(this IEndpointRouteBuilder app)` that groups all routes under a shared slug via `app.MapGroup("resource-name")`.
- Register the group's routes inside that method, then implement each route as a **private static async method** in the same class — never public.

```csharp
public static class MoviesEndpoint
{
    public static void MapMoviesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("movies");
        group.MapPost("/publish", PublishMovieAsync);
        group.MapGet("/search", SearchMovieAsync);
    }

    private static async Task<Results<Ok<Movie>, ProblemHttpResult>> PublishMovieAsync(...) { ... }
    private static async Task<Results<Ok<MovieResponseDto>, NotFound>> SearchMovieAsync(...) { ... }
}
```

## Response types

- Always use `TypedResults` (not `Results`) to construct responses: `TypedResults.Ok(...)`, `TypedResults.NotFound()`, `TypedResults.NoContent()`.
- Declare the return type as `Results<T1, T2>` to enumerate every possible HTTP result the method can produce. Use as many type parameters as needed.

## Parameter binding

- Use `[FromBody]` for request body DTOs, `[FromQuery]` for query-string parameters, and `[FromServices]` only when the compiler cannot infer the binding (ambiguous types). Omit `[FromServices]` for well-known service interfaces — ASP.NET resolves them automatically from DI.
- Always include `CancellationToken cancellationToken` as the **last** parameter.

## Business logic placement

- For write / command operations, delegate to a dedicated `{Action}{Feature}Handler` class (e.g. `PublishMovieHandler`) rather than embedding logic in the endpoint method.
- For reads that require caching, inject `IFusionCache` directly into the handler parameter list. Cache under a descriptive key, set both `Duration` (in-memory) and `DistributedCacheDuration` (Redis), and always apply the feature tag (`tags: ["movies"]`) so invalidation can be done tag-based.

## Route conventions

- Group slugs are lowercase, noun-plural (`movies`, `reservations`).
- Sub-routes use kebab-case when multi-word. Path parameters embed the type constraint: `{id:guid}`.
- Register each endpoint method call (`group.MapGet(...)`) one per line for readability.

## Registration

- Call `app.Map{Feature}Endpoints()` once in each service's `Program.cs` after `app.Build()`.
