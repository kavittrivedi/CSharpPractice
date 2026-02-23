# CancellationToken demo (ConsoleApp)

This repo contains a small .NET 8 console app that demonstrates how to **cancel an async operation** using `CancellationToken` and `CancellationTokenSource`.

## Program overview (simple explanation)

The app runs two things at the same time:

1. A long-running async method (`LongRunningOperationAsync`) that simulates work.
2. A background task that waits ~2 seconds and then requests cancellation.

Inside `LongRunningOperationAsync`, the code:

- Loops up to 5 times
- Checks if cancellation was requested (`cancellationToken.ThrowIfCancellationRequested()`)
- Waits 1 second (`Task.Delay`) to simulate work
- Prints `Working...`

When cancellation is requested, `ThrowIfCancellationRequested()` throws an `OperationCanceledException`. `Main` catches it and prints `Operation was canceled.`

## Expected output (example)

Exact output can vary slightly due to timing, but it typically looks like:

```
Working...
Working...
Operation was canceled.
```

## Key types used

- `CancellationTokenSource`: creates and controls cancellation (`Cancel()`).
- `CancellationToken`: passed into async work so it can stop cooperatively.
- `OperationCanceledException`: the standard exception used to end work when cancellation happens.

## Build and run

From the folder that contains the project file (`ConsoleApp.csproj`):

```bash
dotnet build
dotnet run
```

## Files

- `ConsoleApp/Program.cs`: the full demo.