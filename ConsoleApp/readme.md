# `++i` vs `i++` in C# (pre-increment vs post-increment)

Both `++i` and `i++` increase `i` by 1. The difference is **when the increment happens** relative to using the value in an expression.

## `++i` (pre-increment)

- Increments `i` first
- Then returns the new value

## `i++` (post-increment)

- Returns the current value first
- Then increments `i`

## Simple example

```csharp
int i = 5;

int a = ++i; // i becomes 6, then a gets 6
Console.WriteLine($"i={i}, a={a}");

i = 5;
int b = i++; // b gets 5, then i becomes 6
Console.WriteLine($"i={i}, b={b}");
```

Expected output:

```
i=6, a=6
i=6, b=5
```

## Quick rule

- Use `++i` when you need the **incremented value** right away.
- Use `i++` when you need the **current value** first, and the increment can happen after.
