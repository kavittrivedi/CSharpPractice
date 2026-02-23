# `First` vs `FirstOrDefault` in simple language (LINQ)

This repo contains a small .NET 8 console app that shows the difference between LINQ's `First` and `FirstOrDefault` when you try to find an item in a list.

## The idea

In `ConsoleApp/Program.cs`, we have a list of `Person` objects and we search for a person named "David".

- If "David" exists, **both** methods return that person.
- If "David" does **not** exist, they behave differently.

## `First(...)`

`First` means: **"Give me the first match. If there is no match, throw an error."**

- Match found => returns the item.
- No match => throws `InvalidOperationException`.

## `FirstOrDefault(...)`

`FirstOrDefault` means: **"Give me the first match. If there is no match, return a default value."**

- Match found => returns the item.
- No match => returns the default value.

For reference types (like `Person`), the default value is `null`. That’s why the sample checks for `null` and prints "No match found".

## Quick rule

- Use `First` when not finding a match should be treated as a problem.
- Use `FirstOrDefault` when not finding a match is normal and you want to handle it yourself.

## Build and run

From the folder that contains the project file (`ConsoleApp.csproj`):

```bash
dotnet build
dotnet run
```

## Files

- `ConsoleApp/Program.cs`: the demo comparing `First` and `FirstOrDefault`.
