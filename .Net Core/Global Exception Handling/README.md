## Global Exception Handling

Global exception handling means handling application errors in one common place instead of writing `try-catch` blocks in every controller or method.

In a .NET Core Web API, we usually do this by using middleware. When any unhandled exception occurs anywhere in the request pipeline, the global exception handler catches it, logs it, and returns a proper error response to the client.

### When to Use Global Exception Handling

Use global exception handling when:

- You want one common way to handle unexpected errors.
- You want to return clean error messages instead of showing technical exception details.
- You want to log all errors from one place.
- You want to avoid repeating the same `try-catch` code in many controllers.
- You are building a Web API or application that should not expose internal error details to users.

### Benefits of Global Exception Handling

1. **Cleaner Code**

   Controllers and services stay simple because you do not need to write the same error handling code everywhere.

2. **Consistent Error Response**

   Every error can return the same type of response, such as status code, message, and error details.

3. **Better Logging**

   All exceptions can be logged from one place, which makes debugging and production monitoring easier.

4. **Improved Security**

   Users do not see sensitive technical details like stack traces, database errors, or internal class names.

5. **Easy Maintenance**

   If you want to change how errors are handled, you only need to update one place.

### Simple Example

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync("{\"message\":\"Something went wrong. Please try again later.\"}");
    });
});
```

In simple words, global exception handling is like a safety net for your application. If something goes wrong and it is not handled anywhere else, this safety net catches the error and sends a proper response to the user.

## Can we implement global exception handling for specific scenario?

Yes, we can implement global exception handling for specific scenarios.

Global exception handling does not mean every error must return the same response. We can check the exception type and return different responses based on the scenario.

Example:
```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        context.Response.ContentType = "application/json";

        if (exception is UnauthorizedAccessException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("{\"message\":\"You are not authorized.\"}");
        }
        else if (exception is KeyNotFoundException)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("{\"message\":\"Requested record was not found.\"}");
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("{\"message\":\"Something went wrong. Please try again later.\"}");
        }
    });
});
```
Simple idea:
1. **If user is not allowed, return 401 Unauthorized**
2. **If data is not found, return 404 Not Found**
3. **If validation fails, return 400 Bad Request**
4. **For unknown errors, return 500 Internal Server Error**

## What is good global exception handling or separate exception handling?

### Global Exception Handling

**Purpose:** Handles all exceptions in one centralized place.

**When to Use:**

- When you want to log and manage all unhandled exceptions in a consistent way.
- Best for capturing unexpected errors or application-wide policies.

**Advantages:**

- Reduces repetitive code.
- Ensures consistent logging and error responses.

**Example:** Use middleware or UseExceptionHandler in ASP.NET Core to handle all exceptions globally.

### Separate Exception Handling

**Purpose:** Handles exceptions at specific places, like in individual methods or classes.

**When to Use:**

- When exceptions are domain-specific or need special handling for particular operations.
- Useful for cases where you need to recover from certain exceptions or provide custom error messages.

**Advantages:**

- Gives precise control over how each exception is managed.
- Makes handling specific scenarios easier.

### Comparison:

| Aspect              | Global Exception Handling   | Separate Exception Handling          |
| ------------------- | --------------------------- | ------------------------------------ |
| **Scope**           | Application-wide            | Method or class-specific             |
| **Code Repetition** | Minimal                     | More repetitive                      |
| **Custom Handling** | Generalized                 | Fine-tuned for specific exceptions   |
| **Use Case**        | Unexpected/unhandled errors | Known scenarios needing special care |

### Best Practice:

Use global exception handling for general errors and combine it with separate exception handling for domain-specific or critical operations. This provides a balance of consistency and precision.