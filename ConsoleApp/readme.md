# `First` vs `FirstOrDefault` (LINQ)

This repo contains a small .NET 8 console app that demonstrates the difference between LINQ's `First` and `FirstOrDefault` when searching a sequence.

## What the sample does

Given a list:

```csharp
var numbers = new List<int> { 1, 2, 3, 4, 5 };
```

It tries to find the first number that matches a predicate (`x > 10`). Since **no elements match**, the two methods behave differently.

## Behavior difference

### `First(predicate)`

- Returns the first element that matches the predicate.
- If **no element matches**, it throws an `InvalidOperationException` ("Sequence contains no matching element").

Use `First` when “no match” should be treated as an error.

### `FirstOrDefault(predicate)`

- Returns the first element that matches the predicate.
- If **no element matches**, it returns the *default value* of the element type.
  - For `int`, default is `0`
  - For reference types, default is `null`

Use `FirstOrDefault` when “no match” is expected and you want to handle it without exceptions.

## Example output

With the current predicate (`x > 10`), typical output looks like:

```
First Exception: Sequence contains no matching element
FirstOrDefault: 0
```

## Important note about defaults

For value types (like `int`), `FirstOrDefault` returning `0` can be ambiguous because `0` might also be a valid value.

Common alternatives:

- Use `Any(...)` first to check whether a match exists.
- Use `Where(...).Select(...).Cast<int?>().FirstOrDefault()` (or similar) to return a nullable type.

## Build and run

From the folder that contains the project file (`ConsoleApp.csproj`):

```bash
dotnet build
dotnet run
```

## Files

- `ConsoleApp/Program.cs`: the demo comparing `First` and `FirstOrDefault`.
