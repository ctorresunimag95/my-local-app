---
applyTo: "Cinema.Web/src/**"
---

## Stack

- **Angular 19** with **standalone components** — no NgModules anywhere.
- **RxJS ~7.8** — all async is `Observable<T>`; do not introduce Promises or signals.
- **ng-zorro-antd 19** — use its components for all UI; do not add other component libraries.
- **Auth0** via `@auth0/auth0-angular` for authentication.
- **OpenTelemetry** instrumentation is wired globally via `provideInstrumentation()` in `app.config.ts`; do not add manual spans unless there is a specific tracing requirement.

## Bootstrapping & providers

- Entry point is `bootstrapApplication(AppComponent, appConfig)` in `main.ts`.
- All application-wide providers live in `app.config.ts`. Add new global providers there, not in individual components.
- Zone-based change detection (`provideZoneChangeDetection({ eventCoalescing: true })`). Do not switch to `OnPush` or zoneless unless explicitly asked.

## Components

- Every component must be **standalone** (`standalone: true` in the decorator).
- Use **class-based components** with `@Component`. Do not use signal-based or functional component patterns.
- Keep components focused on presentation. Delegate data fetching and business logic to services.
- Template type checking is strict (`strictTemplates: true`) — all template bindings must be type-safe.

## Services

- Decorate every service with `@Injectable({ providedIn: 'root' })`.
- HTTP methods return `Observable<T>`, never `Promise<T>`.
- Resolve the API base URL from `environment.apiUrls.cinema`, then append `/management` or `/reservation` depending on the target service. Never hardcode a hostname or port.

```typescript
@Injectable({ providedIn: 'root' })
export class ReservationService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrls.cinema}/reservation`;

  getMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.baseUrl}/movies`);
  }
}
```

## Routing

- Define routes in a `*.routes.ts` file co-located with the feature.
- Use `loadChildren` for feature route sets (lazy loading). Use `loadComponent` for single standalone components when a full route file is unnecessary.
- Register feature routes via `loadChildren` in `app.routes.ts`.

## Forms

- Use **Reactive Forms** exclusively — no template-driven forms.
- Always inject `NonNullableFormBuilder` (not `FormBuilder`) so controls are non-nullable by default.
- Call `markAsDirty()` and `updateValueAndValidity()` when triggering manual validation.

```typescript
private readonly fb = inject(NonNullableFormBuilder);

form = this.fb.group({
  title: ['', Validators.required],
});
```

## State management

- No NgRx. State lives in services.
- Use `Subject` or `BehaviorSubject` for shared reactive state (see `GlobalAlertService`).
- Component-local state uses class fields (`isLoading`, `items`, etc.) updated inside `subscribe` callbacks.

## Folder structure

Place new features under `src/app/<feature>/` following the existing layout:

```
<feature>/
  <feature>.component.ts
  <feature>.component.html
  <feature>.routes.ts          # only if the feature has sub-routes
  services/
    <feature>.service.ts
  types/
    <feature>.types.ts
```

Shared UI that is used across features goes in `src/app/shared/components/`.

## TypeScript

- Strict mode is on (`strict: true`). All types must be explicit; no `any`.
- `noImplicitOverride: true` — add the `override` keyword on any method that overrides a base class member.
- Prefer `readonly` for injected dependencies and fields that are never reassigned.
- Use `inject()` for dependency injection inside constructors or field initializers; avoid constructor parameter injection.

## Testing

- Framework: **Jasmine + Karma**.
- Configure the component or service under test with `TestBed.configureTestingModule({ imports: [ComponentName] })`.
- Every new component and service must have a spec file with at minimum a "should create" test.
