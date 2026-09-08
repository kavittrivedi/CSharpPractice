## 1. Your API is returning 500 errors. You want to handle all unhandled exceptions centrally and return a standard JSON response. How would you implement it?

### Answer

I would create a **custom exception-handling middleware** and put it early in the pipeline.

```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            statusCode = 500,
            message = "An unexpected error occurred."
        });
    }
}
```

Register it early:

```csharp
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
```

### Architect-level point

I would **not expose the actual exception/stack trace to the client**. I would log the exception with a correlation ID and return a standardized error response, preferably using `ProblemDetails`.

In newer ASP.NET Core applications, I can also use the built-in exception-handling infrastructure such as `UseExceptionHandler()` rather than writing custom middleware when custom behavior isn't required.

---

# 2. You have 10 APIs running behind a load balancer. You want to add request/response logging middleware. What problems do you need to consider?

### Answer

I would consider:

1. **Don't log sensitive information**

   * Passwords
   * Authorization tokens
   * Credit-card information
   * Personal information

2. **Don't blindly log request/response bodies**

   * Large payloads can consume memory.
   * Performance can be affected.

3. **Add correlation/trace ID**

```text
Request
   ↓
Correlation ID
   ↓
Service A
   ↓
Service B
   ↓
Service C
```

The same trace/correlation ID helps us trace one request across multiple services.

4. **Structured logging**

Instead of:

```csharp
_logger.LogInformation("Request received");
```

I would log structured properties such as:

```csharp
_logger.LogInformation(
    "Request {Method} {Path} TraceId {TraceId}",
    context.Request.Method,
    context.Request.Path,
    context.TraceIdentifier);
```

5. **Performance**

For high-volume APIs, logging every request body synchronously can become expensive. I would use appropriate log levels and centralized logging/observability.

### Strong interview point

> "Middleware is a cross-cutting concern, so request logging is a good middleware candidate, but I would design it carefully around security, performance, payload size, and distributed tracing."

---

# 3. Your authentication middleware works, but sometimes users receive 401 and sometimes 403. How would you troubleshoot it?

### Answer

First, I would distinguish the two:

### 401 — Unauthenticated

The request doesn't have a valid authenticated identity.

Typical reasons:

```text
Missing token
Invalid token
Expired token
Invalid issuer
Invalid audience
```

### 403 — Authenticated but not authorized

The user is authenticated, but doesn't have permission to access the resource.

For example:

```text
User → Authenticated ✅
Role → Employee
Required Role → Admin
              ↓
             403
```

I would verify the middleware pipeline:

```csharp
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
```

**Authentication must execute before authorization.**

Then I would inspect:

* JWT validation configuration
* Claims
* Roles/policies
* Token issuer/audience
* Expiration
* Authentication scheme
* `[Authorize]` attributes/policies

For example:

```csharp
[Authorize(Policy = "AdminOnly")]
public IActionResult DeleteUser(int id)
{
    ...
}
```

### Interview-level answer

> "I wouldn't immediately change the middleware. I'd first determine whether the failure is authentication or authorization, then inspect the token, claims, authentication scheme and authorization policy."

---

# 4. You have multiple middleware components. One middleware depends on data created by another middleware. How do you make sure the order is correct?

### Answer

**Middleware execution order matters.**

For example:

```text
Request
  ↓
Exception Handling
  ↓
Logging
  ↓
Authentication
  ↓
Authorization
  ↓
Routing/Endpoint
  ↓
Controller
```

Suppose I have:

```csharp
app.UseMiddleware<CorrelationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

The authentication middleware can populate `HttpContext.User`, and authorization can then use that identity.

If I put authorization before authentication:

```csharp
app.UseAuthorization();
app.UseAuthentication(); // Wrong order
```

authorization may not have the authenticated user information it expects.

### Important architect-level concept

Middleware is a **pipeline**:

```text
Middleware A
     ↓
Middleware B
     ↓
Middleware C
     ↓
Endpoint
```

Each middleware can execute code **before and after**:

```csharp
public async Task InvokeAsync(HttpContext context)
{
    // Before next middleware

    await _next(context);

    // After next middleware
}
```

So the order affects both request processing and response processing.

---

# 5. Your API has a requirement: reject requests when the client sends more than 100 requests per minute. Would you implement this using middleware?

### Answer

**Yes, middleware can be used, but I wouldn't automatically build the rate limiter myself.**

ASP.NET Core provides **rate limiting middleware**.

For example:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 100;
    });
});
```

Then:

```csharp
app.UseRateLimiter();
```

And apply the policy:

```csharp
[EnableRateLimiting("fixed")]
public IActionResult GetData()
{
    ...
}
```

For a production distributed environment, I would also think about:

* Whether the limit is per user, API key, IP, or client
* Multiple API instances
* Distributed state
* Burst traffic
* HTTP `429 Too Many Requests`
* Retry behavior
* Whether rate limiting belongs at the API gateway/load-balancer layer

### Strong 15-year-experience answer

> "Middleware is appropriate for cross-cutting request-level concerns such as rate limiting, but for a distributed production system I would first determine whether this should be handled at the gateway/API-management layer or inside ASP.NET Core. I would avoid implementing a custom in-memory limiter if requests can hit multiple instances."

---