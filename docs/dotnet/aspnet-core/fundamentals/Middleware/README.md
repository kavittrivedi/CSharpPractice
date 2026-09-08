## What is Middleware? Have you written any custom middleware? 

Middleware in the context of web development is a piece of software that sits between the request from a user (client) and the response that your application sends back to the client. It processes the incoming request, modifies it, or performs actions based on it before it reaches the next part of your application or before the response is sent back.

In simple terms, middleware acts like a gatekeeper for your application, doing tasks like:

- **Logging**: Keeping track of requests to monitor how your app is being used.
- **Authentication**: Checking if the user is allowed to access the requested resource.
- **Error handling**: Catching errors in the application before the response is sent to the user.
- **Request modification**: Changing or adding things to the request, like headers or query parameters.

One‑liner: Middleware are pipeline components that handle and transform HTTP requests and responses.  

## Custom Middleware

Yes, you can write your own custom middleware. Here's an example of a custom middleware in .NET Core:

```csharp
public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add custom logic before the request reaches the controller
        Console.WriteLine("Request received at: " + DateTime.Now);

        // Call the next middleware in the pipeline
        await _next(context);

        // Add custom logic after the response is generated
        Console.WriteLine("Response sent at: " + DateTime.Now);
    }
}
```

## Using Custom Middleware

To use the custom middleware in your application, you need to register it in the Startup.cs file:

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseMiddleware<CustomMiddleware>();

    // other middlewares like routing, authorization, etc.
}
```

In this example, the CustomMiddleware logs the time when the request is received and when the response is sent. You can extend this middleware to include other tasks as per your requirements.

## Purposes for Custom Middleware

For what purpose we can implement custom middleware? 

You can implement custom middleware for various purposes in your application. Here are some common use cases:

- **Logging**: Track when requests come in, what the user is requesting, and when responses are sent back. This helps in debugging and monitoring.
- **Authentication and Authorization**: Check if the user is logged in and has the right permissions before allowing them to access certain resources.
- **Error Handling**: Catch errors and send a proper response (like a custom error page) to the user instead of crashing the application.
- **Request Modification**: Modify or add information to the incoming request, like adding headers or processing data before passing it to the next part of the application.
- **Performance Monitoring**: Track how long a request takes to process and log performance metrics for optimization.
- **Caching**: Serve cached responses for frequently requested resources to improve performance and reduce load.
- **Custom Headers**: Add or modify headers in the response for security or other purposes, like CORS (Cross-Origin Resource Sharing).

Custom middleware allows you to insert these tasks in the request-response pipeline, helping you handle cross-cutting concerns in a clean and modular way.

## How Middleware Works in .NET Core?

In .NET Core, **middleware** is a component that processes requests and responses in the **request-response pipeline**. It works in a pipeline fashion where each middleware handles part of the request, can modify it, and then either passes control to the next middleware or short-circuits the pipeline (e.g., returning a response). Middleware components are executed in the order they are registered in the `Startup.cs` class, using methods like `app.Use()`, `app.Map()`, and `app.Run()`.

### Working Flow:

1. **Request enters** the pipeline.
2. **Middleware processes** the request.
3. **Passes to the next** middleware (or returns a response).
4. **Response travels back** through the pipeline.

Each middleware can inspect or modify the request/response as needed.

## What is app.Use & app.Run in .NET Core?

In ASP.NET Core, `app.Use()` and `app.Run()` are methods used to configure the middleware pipeline in the application. The middleware is a sequence of components that handle requests and responses as they pass through the pipeline.

### 1. app.Use()

- **Purpose:** `app.Use()` is used to add middleware components that can perform tasks both before and after passing the request further down the pipeline.
- **Flow Control:** Middleware added with `app.Use()` can decide whether to pass the request to the next middleware or terminate the request at that point.
- **Next():** Middleware defined with `app.Use()` usually calls `next()`, which forwards the request to the next component in the pipeline.

**Example:**
```csharp
app.Use(async (context, next) =>
{
    // Do something before the next middleware
    await context.Response.WriteAsync("Before Middleware 1\n");

    // Call the next middleware in the pipeline
    await next.Invoke();

    // Do something after the next middleware
    await context.Response.WriteAsync("After Middleware 1\n");
});
```

### 2. app.Run()

- **Purpose:** `app.Run()` is used to add terminal middleware, meaning it handles the request and stops the pipeline. It doesn't call the next() middleware in the pipeline.
- **Flow Control:** Since `app.Run()` does not pass control to the next middleware, it typically handles the request completely and sends the response.

**Example:**
```csharp
app.Run(async (context) =>
{
    // This will terminate the pipeline, no other middleware will be executed
    await context.Response.WriteAsync("This is the final middleware.\n");
});
```

### Key Differences

- **Flow:**
  - `app.Use()` allows passing the request to the next middleware using `next()`.
  - `app.Run()` is terminal and does not call the next middleware.

- **Use Case:**
  - `app.Use()` is used for middleware that needs to perform work before and after the request is passed through the pipeline.
  - `app.Run()` is used for middleware that ends the request pipeline (e.g., handling the final response).

**Example Combining Both:**
```csharp
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Before Middleware 1\n");
    await next();  // Pass to the next middleware
    await context.Response.WriteAsync("After Middleware 1\n");
});

app.Run(async (context) =>
{
    await context.Response.WriteAsync("This is the final middleware\n");
});
```

**Summary:**

- `app.Use()`: Adds middleware that can pass the request to the next middleware using `next()`.
- `app.Run()`: Adds terminal middleware that handles the request and stops further processing.

These methods help build a flexible and customizable request processing pipeline in ASP.NET Core.

## What is app.Map() in .NET Core?

In ASP.NET Core, `app.Map()` is used to branch the middleware pipeline based on a specific request path. It allows you to define a separate pipeline for handling requests that match a particular URL path.

### How app.Map() Works

- **Purpose:** `app.Map()` sets up middleware for a specific URL path and ensures that only requests starting with that path are processed by the middleware in that branch.
- **Path-based Branching:** You can create different middleware pipelines for different request paths.
- **Usage:** You pass a URL path as the first argument to `app.Map()`, and then define a middleware pipeline for that path. Only requests matching the given path will be handled by the mapped middleware.

**Example:**
```csharp
app.Map("/greet", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Hello from the /greet path!");
    });
});

app.Map("/goodbye", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Goodbye from the /goodbye path!");
    });
});
```

### How It Works

- `app.Map("/greet")`: This maps requests that start with `/greet`. When someone navigates to `/greet`, it will respond with "Hello from the /greet path!".
- `app.Map("/goodbye")`: This maps requests that start with `/goodbye`. When someone navigates to `/goodbye`, it will respond with "Goodbye from the /goodbye path!".

If you visit any other path, these middlewares won't execute since they are specific to `/greet` and `/goodbye`.

### Key Features

- **Path Matching:** `app.Map()` only applies to requests that match the specified path.
- **Branching:** You can define different processing pipelines for different paths.

**Summary:**

- `app.Map()` is used to branch the middleware based on the request path.
- It helps you create separate logic for specific URL patterns.
- It simplifies organizing request handling for different sections of your application.