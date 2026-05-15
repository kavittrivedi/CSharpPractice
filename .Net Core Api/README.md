
# .Net Core Api Interview Practice

## How do You secure API requests? 

Securing API requests is essential to protect sensitive data and ensure that only authorized users can access your API. Here are some common practices to secure API requests:

### 1. **Authentication**:

* **API Keys**: Assign unique keys to each user or application. The API checks for a valid key with each request.
* **OAuth 2.0**: Use OAuth for authorization, allowing users to grant limited access to their resources without sharing credentials. Implement flows like Authorization Code Grant for server-side applications or Client Credentials Grant for machine-to-machine communication.
* **JWT (JSON Web Tokens)**: Use JWTs for stateless authentication. The server generates a token after user login, which is sent with each request.

### 2. **Authorization**:

* Implement role-based access control (RBAC) to define what authenticated users can do.
* Ensure that each API endpoint checks for the necessary permissions based on user roles.

### 3. **HTTPS**:

* Use HTTPS instead of HTTP to encrypt data in transit. This protects against man-in-the-middle attacks by encrypting the data being sent between the client and server.

### 4. **Rate Limiting**:

* Limit the number of requests a client can make in a given time frame to prevent abuse and denial-of-service attacks. You can implement this on the server-side.

### 5. **Input Validation**:

* Validate and sanitize input data to protect against SQL injection, cross-site scripting (XSS), and other injection attacks.

### 6. **CORS (Cross-Origin Resource Sharing)**:

* Configure CORS to control which domains are allowed to access your API. This is especially important for web applications to prevent unauthorized cross-origin requests.

### 7. **Logging and Monitoring**:

* Implement logging to track API requests, including details like IP addresses and request types. Monitor these logs for unusual activity to detect potential threats.

### 8. **Error Handling**:

* Avoid exposing sensitive information in error messages. Provide generic error messages to users and log detailed errors for internal review.

### 9. **Data Encryption**:

* Encrypt sensitive data stored in databases and ensure it is only decrypted when needed. This adds an additional layer of security.

### 10. **Security Headers**:

* Use HTTP security headers like `Content-Security-Policy`, `X-Content-Type-Options`, and `X-Frame-Options` to protect against various web vulnerabilities.

### 11. **API Gateway**:

* Consider using an API gateway to manage, authenticate, and monitor API requests. It can provide a centralized point for implementing security measures.

### Summary:

Securing API requests involves a combination of authentication, authorization, encryption, rate limiting, and monitoring. By implementing these practices, you can protect your API and the data it handles from unauthorized access and attacks.

## How many ways we can do Web API Versioning? 

Web API versioning is essential for managing changes to your API while ensuring that existing clients can continue to function without disruption. There are several common strategies for versioning APIs:

### 1. **URI Versioning**:

* **Path Versioning**: Include the version number in the URL path. This is one of the most straightforward approaches.

  * Example:

    * `https://api.example.com/v1/products`
    * `https://api.example.com/v2/products`

### 2. **Query String Versioning**:

* Append the version number as a query parameter in the URL.

  * Example:

    * `https://api.example.com/products?version=1`
    * `https://api.example.com/products?version=2`

### 3. **Header Versioning**:

* Specify the API version in the request headers. This method keeps the URL clean but requires clients to be aware of the header.

  * Example:

    * Add a custom header like `X-API-Version: 1`
    * Clients would send requests with this header to specify the desired version.

### 4. **Accept Header Versioning**:

* Use the `Accept` header to indicate the version of the API being requested. This approach utilizes content negotiation.

  * Example:

    * `Accept: application/vnd.example.v1+json`
    * `Accept: application/vnd.example.v2+json`

### 5. **Content Negotiation**:

* Similar to Accept header versioning, this approach allows clients to request a specific version based on the `Content-Type` or `Accept` headers. You may define custom media types for different versions.

### 6. **Subdomain Versioning**:

* Version the API by using different subdomains. This method may be less common but can be useful for significant changes.

  * Example:

    * `https://v1.api.example.com/products`
    * `https://v2.api.example.com/products`

### 7. **Using a Custom Routing Convention**:

* Implement a custom routing mechanism in your API framework to handle versioning. This can provide flexibility but may require more setup.

### 8. **Semantic Versioning**:

* While not a method of URL versioning, semantic versioning can be applied to describe the versioning of your API (e.g., MAJOR.MINOR.PATCH). This can help communicate changes more clearly.

### Summary:

In summary, you can version your Web API using various methods, including URI versioning, query string versioning, header versioning, accept header versioning, content negotiation, subdomain versioning, custom routing, and semantic versioning. The choice of versioning strategy depends on your specific requirements, client needs, and how you want to manage backward compatibility as your API evolves.

## Explain Different Types of Authentication for APIs

Explain Different types Authentication for APIs. Authentication for APIs is the process of verifying the identity of the user or system making a request to your API. It ensures that only authorized users or applications can access your API resources.

Here are the common ways to authenticate APIs:

- **API Key**: The client sends a unique key with each request. The server checks if the key is valid before allowing access. It's simple but less secure than other methods.
- **Basic Authentication**: The client sends a username and password in the request header. The server checks if the credentials are valid. This is also simple but not very secure unless used over HTTPS.
- **Bearer Token (JWT)**: The client sends a token (like a JSON Web Token - JWT) in the request header. The server verifies the token to ensure the request is from an authenticated user. This is one of the most secure and common methods used in modern web APIs.
- **OAuth**: This is a more advanced and secure method, often used when your API needs to authenticate users from other services (like Google or Facebook) without sharing their password. OAuth issues access tokens after a user grants permission.

### Example of Bearer Token Authentication in an API

The client sends a request with a token in the Authorization header like this:

```
Authorization: Bearer <your-token-here>
```

The server checks if the token is valid and grants access accordingly.

Authentication ensures that only users with valid credentials can interact with your API and access sensitive data.

## Explain Structure of JWT Token

Explain structure of JWT token. JWT (JSON Web Token) is a compact, URL-safe token used for authentication. It consists of three parts, separated by dots (.):  

### Header
This contains information about how the token is signed (usually with a secret or a key). It typically looks like this:  
```json
{
  "alg": "HS256", 
  "typ": "JWT"
}
```  
- **alg**: The algorithm used to sign the token (e.g., HS256 means HMAC with SHA-256).  
- **typ**: The type of token (usually JWT).

### Payload
This is the main data of the token. It contains claims, which are statements about an entity (usually the user) and additional metadata. Some common claims are:  
- **sub** (subject): The user or entity the token is about.  
- **iat** (issued at): When the token was created.  
- **exp** (expiration): When the token expires.  

Example of a payload:  
```json
{
  "sub": "1234567890",
  "name": "John Doe",
  "iat": 1516239022
}
```

### Signature
This part is used to verify the token's authenticity. It is created by taking the encoded header and payload, and signing them with a secret key or a private key.  

The formula for creating the signature is:  
```
HMACSHA256(
  base64UrlEncode(header) + "." + base64UrlEncode(payload),
  secretKey
)
```  
This ensures that the token has not been tampered with.

### Example JWT
A JWT might look like this:  
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```  
- The first part is the header (base64 encoded).  
- The second part is the payload (base64 encoded).  
- The third part is the signature (base64 encoded).  

JWTs are commonly used for securely transmitting information between a client and server.

## Who Verifies the Signature of a JWT Token?

The server verifies the signature of a JWT token.

Here’s how it works:

- When a client sends a JWT token to the server (usually in the request header), the server needs to ensure that the token is valid and hasn't been tampered with.
- The server takes the header and payload from the token and re-generates the signature using the secret key (or public key if using asymmetric signing) that was used to sign the token.
- The server compares the re-generated signature with the signature part of the token sent by the client.
- If the signatures match, it means the token is valid and hasn’t been altered.
- If they don't match, the token is considered invalid or tampered with, and the server rejects the request.

So, the server is responsible for checking if the JWT's signature is correct, ensuring that the token is authentic and trustworthy.

## Explain difference between GET and POST GET vs POST:

GET:

Purpose: Used to retrieve data from a server.

Data Transmission: The data (parameters) is sent in the URL (query string). For example: /api/users?id=1.

Idempotent: GET requests should not change any data on the server.

Caching: GET requests can be cached by browsers and servers.

Use Case: When you need to fetch data (like viewing a webpage or getting information about a user).

POST:

Purpose: Used to send data to the server to create or update resources.

Data Transmission: The data is sent in the body of the request (not visible in the URL).

Non-Idempotent: POST requests can change the state on the server (e.g., creating a new user or submitting a form).

Caching: POST requests are typically not cached.

Use Case: When you need to send data to be processed (like submitting a form or creating a new record).

HTTP Status Codes:

GET:

Typically uses 200 OK if the request is successful and data is returned.

If the resource is not found, it may return 404 Not Found.

POST:

Typically uses 201 Created if a new resource is successfully created.

It can also return 200 OK if the action is successful but doesn't create new data.

If there is an issue with the request, it might return 400 Bad Request.

In summary:

GET is for fetching data, typically returns 200 OK.

POST is for sending data to be processed, often returns 201 Created or 200 OK.

## How to persist state/how to sync two web API communications?  

To persist state or sync communications between two web APIs, you need to store and manage the data (state) in a way that both APIs can access and update it when needed.

Here are common approaches to achieve this:

1. Using a Database:

Persisting State: Store the necessary information in a database (like SQL, NoSQL, etc.) that both APIs can access.

Syncing: Each API reads from and writes to the same database. Whenever one API updates the data, the other API can read the latest state.

Example: API 1 adds or updates a user in the database, and API 2 fetches the user data from the same database to perform further actions.

2. Using Shared Caching:

Persisting State: Use an in-memory cache (like Redis or Memcached) to temporarily store state that needs to be accessed by both APIs.

Syncing: Both APIs can read from and write to the cache. This allows faster access to frequently used data.

Example: API 1 writes data (e.g., session info) to the cache, and API 2 fetches that data from the cache to continue the session or process.

3. Using Message Queues:

Syncing: Use a message queue (like RabbitMQ, Kafka, or Azure Service Bus) to send messages between APIs. When one API finishes a task, it sends a message (event) to the queue, which the other API listens to and reacts to.

Example: API 1 processes an order and sends a message to a queue. API 2 listens to the queue and processes the payment once it gets the order info.

4. Using Tokens or JWT:

Persisting State: Use tokens (like JWT) to carry information about the current session or user between the two APIs.

Syncing: When API 1 sends a request to API 2, it includes the token. API 2 verifies the token and uses the information in it to continue processing.

Example: API 1 sends a user’s authentication token, and API 2 uses that token to authorize the user and process their request.

5. Using Webhooks:

Syncing: A webhook is a way one API can notify another API about events. When something happens in API 1, it sends an HTTP request to API 2 (the webhook) to notify it.

Example: API 1 processes a new order and sends a webhook to API 2 to start shipping the product.

In Short:

To persist state, store data in a shared location like a database or cache.

To sync communications, you can use mechanisms like shared databases, message queues, tokens, or webhooks to make sure both APIs stay in sync and can access the latest state.

## How will you increase performance for your API? 

To increase the performance of your API, you can focus on improving speed, reducing response time, and handling more requests efficiently. Here are some simple strategies:

1. Caching:

What it is: Store frequently accessed data in memory (e.g., using Redis or Memcached) so that you don’t have to fetch it from the database every time.

Benefit: Reduces load on the database and speeds up response times.

Example: Cache the results of a product search for a few minutes so that repeated requests don’t require re-fetching the data from the database.

2. Database Optimization:

What it is: Optimize database queries to reduce execution time.

Benefit: Faster database access and reduced API response time.

How to do it:

Use indexes on frequently searched fields.

Avoid N+1 query problems (e.g., fetching multiple related items separately).

Use pagination for large data sets to return smaller chunks at a time.

3. Load Balancing:

What it is: Distribute incoming API traffic across multiple servers.

Benefit: Helps handle more traffic and prevents a single server from becoming overwhelmed.

How to do it: Use a load balancer (e.g., AWS Elastic Load Balancing, Nginx) to manage incoming requests.

4. Asynchronous Processing:

What it is: Handle long-running tasks asynchronously using background workers.

Benefit: Improves API response time by offloading time-consuming operations (like sending emails or processing data) to be done later.

Example: If an API needs to generate a report, return an immediate response to the user, while the report is generated in the background.

5. Compression:

What it is: Compress API responses (e.g., using GZIP) to reduce the size of data transferred over the network.

Benefit: Reduces network latency and speeds up data transfer.

Example: API responses with large JSON data can be compressed, reducing the time it takes for clients to receive the data.

6. Use HTTP/2:

What it is: HTTP/2 is a newer version of the HTTP protocol that improves performance, especially for APIs with many requests.

Benefit: Reduces the number of connections required, improves multiplexing, and decreases latency.

How to do it: Ensure your server supports HTTP/2 (many modern web servers do).

7. Rate Limiting:

What it is: Limit the number of requests a user can make within a certain period (e.g., 100 requests per minute).

Benefit: Prevents overloading your API with too many requests and ensures fair usage.

How to do it: Use rate-limiting techniques (e.g., token bucket algorithm) to manage request flow.

8. Optimize API Responses:

What it is: Only return the necessary data in the API response.

Benefit: Reduces the amount of data transferred, improving speed.

How to do it: Use field selection (e.g., GraphQL or selective JSON responses) to only include the data requested by the client.

9. Minimize Dependencies:

What it is: Reduce the number of external services or APIs your API depends on.

Benefit: Fewer dependencies mean fewer points of failure and faster processing.

Example: If your API relies on an external service for weather data, consider caching that data or finding a more reliable source.

10. Connection Pooling:

What it is: Reuse database connections rather than opening and closing a connection for each request.

Benefit: Reduces overhead and improves database interaction speed.

How to do it: Use a connection pool (most ORM frameworks like Entity Framework support this).

By applying these strategies, you can improve the speed, scalability, and reliability of your API, ensuring that it can handle more requests efficiently and provide a better experience for users.

## Explain HTTP verbs & status codes in simple language to give answer during interview

Here is a short, clear, interview-ready explanation 👇  
*(Simple language + easy to remember)*

### HTTP Verbs (What action you want to do)

**GET**

- **Use:** Read data
- **Example:** Get user details
- **Safe?** Yes (no data change)
- **Status Code:** 200 OK

**POST**

- **Use:** Create new data
- **Example:** Create a new user
- **Safe?** No
- **Status Code:** 201 Created

**PUT**

- **Use:** Update entire resource
- **Example:** Update full user profile
- **Status Code:** 200 OK / 204 No Content

**PATCH**

- **Use:** Update partial data
- **Example:** Update user email only
- **Status Code:** 200 OK

**DELETE**

- **Use:** Delete data
- **Example:** Delete user
- **Status Code:** 200 OK / 204 No Content

### HTTP Status Codes (What happened with the request)

**2xx – Success**

| Code  | Meaning                   |
| ----- | ------------------------- |
| `200` | Request successful        |
| `201` | Resource created          |
| `204` | Success, no response body |

**4xx – Client Error**

| Code  | Meaning                      |
| ----- | ---------------------------- |
| `400` | Bad request (invalid input)  |
| `401` | Unauthorized (not logged in) |
| `403` | Forbidden (no permission)    |
| `404` | Resource not found           |

**5xx – Server Error**

| Code  | Meaning               |
| ----- | --------------------- |
| `500` | Internal server error |
| `502` | Bad gateway           |
| `503` | Service unavailable   |

### One-Line Interview Answer

HTTP verbs define what action we perform on a resource, and HTTP status codes tell us the result of that action.

### Quick Memory Trick 🧠

- GET → Read
- POST → Create
- PUT → Replace
- PATCH → Update
- DELETE → Remove


## Custom Middleware - Explained for Logging in .net core api

Creating a custom middleware for logging in a .NET Core API allows you to capture and log important information about incoming requests and outgoing responses. Middleware in ASP.NET Core is a way to process HTTP requests and responses, and custom middleware can be used to add additional functionality to the request pipeline, such as logging.

### Steps to Create Custom Logging Middleware

Here's a step-by-step guide on how to implement custom middleware for logging:

#### 1. **Create the Middleware Class**

First, create a new class for your middleware. This class will include the logic for logging.

```csharp
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Log request details
        var request = context.Request;
        Console.WriteLine($"Incoming Request: {request.Method} {request.Path}");

        // Call the next middleware in the pipeline
        await _next(context);

        // Log the outgoing response
        stopwatch.Stop();
        var response = context.Response;
        Console.WriteLine($"Outgoing Response: {response.StatusCode} in {stopwatch.ElapsedMilliseconds} ms");
    }
}
```

#### 2. **Register the Middleware**

Next, you need to register the middleware in the `Startup.cs` file. This is done in the `Configure` method.

```csharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Register the custom logging middleware
        app.UseMiddleware<LoggingMiddleware>();

        // Other middleware registrations (e.g., routing, authorization)
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

#### 3. **Using the Middleware**

Once the middleware is registered, it will automatically log incoming requests and outgoing responses for every request processed by the API. You can run your application and check the console output or logs to see the details.

### Example of the Logging Output

When a request is made to the API, the console might output something like:

```
Incoming Request: GET /api/products
Outgoing Response: 200 in 123 ms
```

### Benefits of Custom Middleware for Logging

* **Centralized Logging**: All request and response logging is handled in one place, making it easier to maintain and update.
* **Performance Monitoring**: By measuring the time taken for each request, you can identify slow endpoints and optimize them.
* **Flexible and Reusable**: You can easily modify the logging behavior or extend it (e.g., adding log levels or writing logs to a file or external logging service).

### Summary

Creating a custom middleware for logging in a .NET Core API involves defining a middleware class, implementing the logging logic, and registering the middleware in the application pipeline. This approach provides a centralized way to capture request and response details, which can be beneficial for monitoring and debugging your API.

## How Many Ways Can We Bind Data in .NET Core 6 API?

In ASP.NET Core 6 API, data binding can be done in several ways to handle incoming data from requests. Here are the main methods of data binding:

1. **From Route Parameters**:

   * You can bind data directly from the URL route parameters.

   ```csharp
   [HttpGet("{id}")]
   public IActionResult GetItem(int id)
   {
       // Use the id parameter directly
   }
   ```

2. **From Query String**:

   * Data can be bound from the query string of the URL.

   ```csharp
   [HttpGet]
   public IActionResult GetItems([FromQuery] string category)
   {
       // Use the category parameter
   }
   ```

3. **From Body**:

   * You can bind complex types from the request body, typically in JSON format.

   ```csharp
   [HttpPost]
   public IActionResult CreateItem([FromBody] ItemModel item)
   {
       // Use the item object
   }
   ```

4. **From Form Data**:

   * Bind data from form submissions, often used with `multipart/form-data` content type.

   ```csharp
   [HttpPost]
   public async Task<IActionResult UploadFile([FromForm] IFormFile file)
   {
       // Use the file object
   }
   ```

5. **From Headers**:

   * Bind data directly from the HTTP request headers.

   ```csharp
   [HttpGet]
   public IActionResult GetItems([FromHeader] string authorization)
   {
       // Use the authorization header
   }
   ```

6. **From Services**:

   * Inject services using dependency injection to access data or perform operations.

   ```csharp
   private readonly IItemService _itemService;

   public MyController(IItemService itemService)
   {
       _itemService = itemService;
   }
   ```

### Summary

These binding methods provide flexibility in how you can receive and work with data in your ASP.NET Core 6 APIs, allowing you to handle different types of input based on your application's requirements.

## What action attributes comes in these "Authorization, Resource Action, Exception, Result" action filter types?

In ASP.NET Core, action filters are a way to run code before and after an action method executes. The action filter types you mentioned can include various action attributes that correspond to their functionality. Here’s a breakdown of each type and the common action attributes associated with them:

### 1. **Authorization Filters**

Authorization filters are executed first and are used to determine whether a user is authorized to access a resource.

**Common Action Attributes:**

* **[Authorize]**: This attribute restricts access to the action method or controller to only authenticated users. It can be used with roles or policies.
* **[AllowAnonymous]**: This attribute allows access to an action method or controller to anonymous users, even if the controller or action is decorated with `[Authorize]`.

### 2. **Resource Filters**

Resource filters are executed after authorization filters and are used to perform tasks related to the resource, such as caching or modifying the request or response.

**Common Action Attributes:**

* **[ServiceFilter]**: This attribute allows you to specify a resource filter that is registered in the DI container.
* **[TypeFilter]**: Similar to `ServiceFilter`, but it creates an instance of the filter type directly, allowing for constructor parameters.

### 3. **Action Filters**

Action filters are executed before and after the action method is executed. They are used to perform tasks such as logging, modifying the action result, or implementing caching.

**Common Action Attributes:**

* **[ActionFilter]**: This attribute is used to apply a custom action filter that you create, inheriting from `ActionFilterAttribute`.
* **[HttpGet]**, **[HttpPost]**, **[HttpPut]**, **[HttpDelete]**: These are routing attributes that specify the HTTP method the action supports, but they also imply a form of action filtering based on HTTP requests.

### 4. **Exception Filters**

Exception filters are executed when an unhandled exception occurs during the execution of an action method. They are used to handle errors globally or at a more granular level.

**Common Action Attributes:**

* **[ExceptionFilter]**: This attribute allows you to apply a custom exception filter that you create, inheriting from `IAsyncExceptionFilter` or `IExceptionFilter`.

### 5. **Result Filters**

Result filters are executed just before and after the result is executed, allowing you to modify the result (e.g., the view result or JSON result) before it is sent to the client.

**Common Action Attributes:**

* **[ResultFilter]**: This attribute allows you to apply a custom result filter that you create, inheriting from `IAsyncResultFilter` or `IResultFilter`.

### Summary

Here's a quick overview of the action attributes associated with each filter type:

* **Authorization Filters**: `[Authorize]`, `[AllowAnonymous]`
* **Resource Filters**: `[ServiceFilter]`, `[TypeFilter]`
* **Action Filters**: `[ActionFilter]`, HTTP method attributes (`[HttpGet]`, `[HttpPost]`, etc.)
* **Exception Filters**: `[ExceptionFilter]`
* **Result Filters**: `[ResultFilter]`

Using these action attributes, you can create a clean and maintainable way to handle cross-cutting concerns in your ASP.NET Core applications.

## What is Rate Limiting in .NET Core API?

Rate limiting means limiting how many requests a client can send to an API in a fixed time.

Example:

If we allow only 5 requests per minute, then one user or one client can call the API only 5 times in 1 minute.

If the client sends more than 5 requests, the API will block extra requests and return an error like `429 Too Many Requests`.

### Why Do We Use Rate Limiting?

Rate limiting is used to protect our API.

It helps us to:

* Stop users from sending too many requests.
* Protect the server from heavy traffic.
* Prevent API abuse.
* Improve application performance.
* Make the API available for all users fairly.

### Real Life Example

Suppose we have a login API.

If one user tries to call the login API 100 times in 1 minute, it may be a brute force attack.

Using rate limiting, we can allow only a few login attempts in a fixed time.

### How to Implement Rate Limiting in .NET Core API

In .NET 7 and .NET 8, rate limiting is available by default in ASP.NET Core.

First, add this namespace in `Program.cs`:

```csharp
using System.Threading.RateLimiting;
```

Then add rate limiting service:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});
```

Here:

* `FixedPolicy` is the policy name.
* `PermitLimit = 5` means only 5 requests are allowed.
* `Window = TimeSpan.FromMinutes(1)` means the time limit is 1 minute.
* `QueueLimit = 0` means extra requests will not wait in queue.

After adding the service, add middleware in `Program.cs`:

```csharp
app.UseRateLimiter();
```

Important:

Add `app.UseRateLimiter()` before mapping controllers.

Example:

```csharp
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();
```

### Apply Rate Limiting on Controller or Action Method

To apply rate limiting, use `[EnableRateLimiting]`.

Controller example:

```csharp
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedPolicy")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        return Ok("Products list");
    }
}
```

Here, rate limiting is applied to the full `ProductsController`.

Action method example:

```csharp
using Microsoft.AspNetCore.RateLimiting;

[HttpGet]
[EnableRateLimiting("FixedPolicy")]
public IActionResult GetProducts()
{
    return Ok("Products list");
}
```

Here, rate limiting is applied only to this action method.

### Complete Program.cs Example

```csharp
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedPolicy", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
```

### Output

If the user sends requests within the limit, the API will return a normal response.

If the user sends more requests than the allowed limit, the API will return:

```text
429 Too Many Requests
```

### Important Points

* Rate limiting controls how many requests a client can send.
* It protects the API from overuse and abuse.
* In .NET 7 and .NET 8, we can use built-in rate limiting.
* Use `AddRateLimiter()` to register rate limiting.
* Use `UseRateLimiter()` to enable rate limiting middleware.
* Use `[EnableRateLimiting("PolicyName")]` on controller or action method.
* `429 Too Many Requests` means the request limit has been crossed.

## What is Content Negotiation in .NET Core API?

Content negotiation means the client can tell the API which response format it wants.

The API can return data in different formats like:

* JSON
* XML
* Plain text

In .NET Core API, content negotiation is mostly done using the `Accept` header.

### Simple Example

Suppose the client sends this request header:

```text
Accept: application/json
```

Then the API returns data in JSON format.

Example JSON response:

```json
{
  "id": 1,
  "name": "Rahul"
}
```

If the client sends this request header:

```text
Accept: application/xml
```

Then the API can return data in XML format, if XML support is configured.

Example XML response:

```xml
<User>
  <Id>1</Id>
  <Name>Rahul</Name>
</User>
```

### Real Life Meaning

Content negotiation is like asking the API:

```text
Please send me data in this format.
```

The API checks the requested format and sends the response in that format if it supports it.

### Default Format in .NET Core API

By default, .NET Core API returns data in JSON format.

Example controller:

```csharp
[HttpGet]
public IActionResult GetUser()
{
    var user = new
    {
        Id = 1,
        Name = "Rahul"
    };

    return Ok(user);
}
```

Default response:

```json
{
  "id": 1,
  "name": "Rahul"
}
```

### How to Add XML Support

If we want the API to return XML also, we can add XML formatter in `Program.cs`.

```csharp
builder.Services.AddControllers()
    .AddXmlSerializerFormatters();
```

After this, the API can return XML when the client sends:

```text
Accept: application/xml
```

### Complete Program.cs Example

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddXmlSerializerFormatters();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

### Interview Answer

Content negotiation in .NET Core API is a process where the client tells the server which response format it wants using the `Accept` header.

For example, if the client sends `Accept: application/json`, the API returns JSON. If the client sends `Accept: application/xml` and XML formatter is configured, the API returns XML.

By default, .NET Core API returns JSON.

### Important Points

* Content negotiation decides the response format.
* The client sends the expected format using the `Accept` header.
* JSON is the default response format in .NET Core API.
* XML support can be added using `AddXmlSerializerFormatters()`.
* If the requested format is not supported, the API may return the default format or an error depending on configuration.

