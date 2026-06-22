---
applyTo: "**/*.cs"
---

## Formatting

- Apply code-formatting style defined in .editorconfig.
- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after if, for, while, foreach, using, try, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use nameof instead of string literals when referring to member names.
- Place private class declarations at the bottom of the file.
- Allman braces (.NET default), 4-space indent, **CRLF for `.cs`** (the repo default is
  LF; `.cs` overrides to CRLF for Visual Studio / .NET tooling), UTF-8, final newline, no
  trailing whitespace. All machine-enforced; `dotnet format` fixes violations.

## Code comments

- Err on the side of over-commenting code when the reasoning is not obvious. Comments should explain WHY code is written a particular way; the WHY is the most important part.
- Do comment non-obvious implementation details: concurrency hazards, lifecycle constraints, compatibility requirements, platform quirks, upstream workarounds, and intentional deviations from the obvious helper or API.
- When parsing strings, logs, command output, protocol payloads, or other loosely structured data, include a comment with an example of the raw format being parsed. Show edge cases, escaping rules, delimiters, optional fields, or malformed-but-observed inputs when they affect the parser.
- When code follows an external standard, protocol, or ecosystem convention, include valid links to the relevant source material so future readers can verify the rule and understand why the code follows it.
- Do not add comments that simply narrate clear code, such as "set the timeout" immediately before assigning a timeout.
- Keep workaround comments close to the workaround. Include an issue link when the workaround is tied to an upstream bug, and describe the condition for removing it when that is known. Good comments explain the constraint or tradeoff.

## Nullable Reference Types

- Declare variables non-nullable, and check for null at entry points.
- Always use is null or is not null instead of == null or != null.
- Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.

## Namespaces

- **File-scoped**, one per file (`namespace Foo;`).
- Namespace **must match folder structure**.

## `var`

- Use `var` for **built-in types** and **when the type is apparent** from the
  right-hand side (`new T()`, casts, literals).
- **Explicit type everywhere else.**
- Target-typed `new` is the apparent-type counterpart and is preferred where the
  type is already stated.

## Braces

- Required around `if`/`for`/`while`/etc. bodies, **except** when the entire
  statement is a true single line (`if (x) return;`).

## Expression-bodied members

- **Allowed anywhere** the body is a single expression. This is _permission_, at
  `suggestion` level — block bodies are equally fine; neither fails the build.

## Members

- `int`/`string`/etc., never `Int32`/`String`.
- No `this.` qualification.
- Prefer `readonly` fields, auto-properties, modern null/pattern idioms
  (`??`, `?.`, pattern matching over cast/`as`, switch expressions).
- Accessibility modifiers required on all non-interface members.
- Unused assignments must be explicit discards (`_ = ...`).

---

## Naming (enforced via `.editorconfig`, all at `warning`)

| Symbol                           | Convention       | Example                    |
| -------------------------------- | ---------------- | -------------------------- |
| Interface                        | `I` + PascalCase | `IModelRouter`             |
| Type parameter                   | `T` + PascalCase | `TResult`                  |
| Class / struct / enum / delegate | PascalCase       | `ResponsePipeline`         |
| Method / property / event        | PascalCase       | `RunAsync`                 |
| Constant                         | PascalCase       | `MaxRetries`               |
| Private / internal field         | `_camelCase`     | `_modelRouter`             |
| Public / protected field         | PascalCase       | (rare — prefer properties) |
| Parameter / local                | camelCase        | `projectId`                |
| Local function                   | PascalCase       | `ParseLine`                |

## Async suffix

Methods returning `Task`/`ValueTask` **end in `Async`**.

**Enforcement gap (review covers it):** the editorconfig naming rule can only match
on the `async` _modifier_, not the return type. A method that returns a `Task`
_without_ the `async` keyword (returning the task directly) is **not** caught by the
analyzer — name it correctly by hand.

---

## Async discipline (convention — review-enforced)

- Never `.Result` or `.Wait()` — no sync-over-async blocking.
- No `async void` except event handlers.
- Thread a `CancellationToken` through every async call chain — especially your
  agentic pipeline stages and all model/SSE calls, which must be cancellable.

---

## Constructor

- Prefer regular constructors over static factory methods, unless the factory is doing something non-trivial (e.g., caching, pooling, or returning a subtype).
- When creating a service/provider/manager class that implements an interface, prefer normal constructor with private readonly fields over primary constructor.
- Use primary constructor for simple data classes (DTOs, records, etc.) with no logic in the constructor.