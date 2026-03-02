# CSharpPractice

## What is .NET Core?

.NET Core is an open-source, cross-platform framework developed by Microsoft for building modern applications. It allows developers to create web apps, APIs, microservices, and console applications using languages like C#, F#, and VB.NET. Unlike the older .NET Framework, .NET Core runs on Windows, Linux, and macOS, making it more flexible for different environments.

## What is Middleware?

Have you written any custom middleware? Explain in simple language.

Middleware in the context of web development is a piece of software that sits between the request from a user (client) and the response that your application sends back to the client. It processes the incoming request, modifies it, or performs actions based on it before it reaches the next part of your application or before the response is sent back.

In simple terms, middleware acts like a gatekeeper for your application, doing tasks like:

- **Logging**: Keeping track of requests to monitor how your app is being used.
- **Authentication**: Checking if the user is allowed to access the requested resource.
- **Error handling**: Catching errors in the application before the response is sent to the user.
- **Request modification**: Changing or adding things to the request, like headers or query parameters.

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

## Types of Files in a .NET Core Project

Explain me all types of files of .net core project. Here’s a simplified explanation of the common files you’ll find in a .NET Core project:

1. **Program.cs**  
   Purpose: Entry point of the application. It’s where the application starts.  
   What it does: Configures services and the request pipeline. In newer .NET Core versions (6 and above), it's used to set up dependency injection and middleware.

2. **Startup.cs** (For versions prior to .NET 6)  
   Purpose: Configures services and the application's HTTP request pipeline.  
   What it does: Sets up services for Dependency Injection, middleware, routing, etc. This file was replaced by Program.cs in .NET 6.

3. **appsettings.json**  
   Purpose: Holds configuration settings for the application.  
   What it does: Used to store key-value pairs for things like database connection strings, API keys, or application settings.

4. **appsettings.Development.json** (or other environment-specific files)  
   Purpose: Stores environment-specific settings for different configurations (like Development, Production).  
   What it does: Used alongside appsettings.json to provide specific settings for the environment you're working in.

5. **launchSettings.json**  
   Purpose: Defines settings for launching and debugging the application.  
   What it does: Specifies things like environment variables, profiles (e.g., IIS Express or Kestrel), and the port on which the application runs during development.

6. **.csproj** (C# Project file)  
   Purpose: Contains project metadata and references.  
   What it does: Defines dependencies, target framework (like .NET 6, .NET Core), and build settings for the project. It also specifies files to be included in the build process.

7. **Properties/launchSettings.json** (same as above, sometimes exists under the Properties folder)  
   Purpose: Defines settings for how the app is launched and debugged, like specific environments or URLs.

8. **Controllers/** Folder  
   Purpose: Contains controller classes for handling HTTP requests.  
   What it does: In an MVC or Web API project, controllers define actions that handle incoming HTTP requests and return responses.

9. **Models/** Folder  
   Purpose: Contains data models or entities.  
   What it does: Models represent data that the application works with (e.g., database entities or data transfer objects).

10. **Views/** Folder (For MVC applications)  
    Purpose: Contains HTML templates or Razor views for rendering UI.  
    What it does: Stores .cshtml files that are rendered to the user as part of the MVC pattern.

11. **wwwroot/** Folder  
    Purpose: Stores static files such as images, JavaScript, CSS, etc.  
    What it does: Anything in this folder is publicly accessible. The browser can request static files like style.css, app.js, and images.

12. **Migrations/** Folder (If using Entity Framework)  
    Purpose: Holds migration files that help with updating the database schema.  
    What it does: Contains classes that represent changes to the database schema, used by Entity Framework to apply changes to the database.

13. **bin/** and **obj/** Folders  
    Purpose: Temporary files generated during the build process.  
    What they do: These folders store compiled binaries and intermediate files. You don’t need to worry about them as they are created during build and deployment.

14. **Dockerfile**  
    Purpose: Defines how to build a Docker image for your application.  
    What it does: Contains instructions to package your application inside a Docker container.

15. **global.json**  
    Purpose: Defines the version of the SDK to use in the project.  
    What it does: Ensures consistency by locking the SDK version, preventing issues with different .NET SDK versions on different developer machines.

16. **Dependencies** Folder (or packages/)  
    Purpose: Contains external libraries and NuGet packages the project depends on.  
    What it does: You won’t normally manually interact with this folder, as it's managed by the NuGet package manager.

## Explain logging mechanisms. 

Logging mechanisms are tools and techniques used to track and record events that occur in an application. These logs help developers monitor the system, troubleshoot issues, and understand how the application is behaving in production. Here’s a simple breakdown of common logging mechanisms:

1. Console Logging:

What it is: Outputs logs directly to the console (standard output).

Benefit: Simple and useful during development or debugging.

Example: You might log messages using console.log() in JavaScript or Console.WriteLine() in C#.

2. File-based Logging:

What it is: Writes logs to a file on the server.

Benefit: Makes logs available for later review, especially for production environments.

Example: Log files might include details like errors, warnings, and information on system performance.

Tools: Libraries like Serilog or log4net in .NET, and Winston or Pino in Node.js can log to files.

3. Log Levels:

Logs are categorized by severity or importance, known as log levels. This helps you filter logs based on the type of information you want to see:

DEBUG: Detailed information, typically used during development.

INFO: General information about the application's normal operation (e.g., startup, shutdown).

WARN: Indications that something unexpected happened, but the application is still working.

ERROR: Serious issues that impact the system's functionality, but might not crash the app.

FATAL: Critical issues that cause the application to stop or crash.

Example: A DEBUG log could say "User login attempt," and an ERROR log might say "User login failed due to incorrect password."

4. Structured Logging:

What it is: Instead of just writing plain text, logs are written in a structured format (like JSON) with key-value pairs.

Benefit: Makes it easier to search, filter, and analyze logs automatically (e.g., using tools like ELK stack or Splunk).

Example: {
  "level": "ERROR",
  "timestamp": "2025-01-07T12:34:56",
  "message": "Database connection failed",
  "details": "Timeout while connecting to DB"
} 5. Centralized Logging:

What it is: Collects logs from multiple servers or services into one central place for easier management.

Benefit: Helps track application health across different services or environments and makes troubleshooting faster.

Tools: Use services like ELK stack (Elasticsearch, Logstash, Kibana), Splunk, or Azure Monitor to aggregate logs from multiple sources.

Example: If your app has multiple microservices, logs from all services are sent to a central server, where you can query them.

6. Cloud-based Logging:

What it is: Logs are sent to cloud platforms, which manage storage, monitoring, and analysis.

Benefit: Easier to scale, and cloud platforms provide additional features like alerts, dashboards, and log querying.

Tools: Services like AWS CloudWatch, Azure Monitor, or Google Cloud Logging.

Example: Logs from your app in the cloud can be monitored in real-time through the cloud provider’s dashboard.

7. Logging Libraries and Frameworks:

What it is: These are pre-built tools that make it easier to implement logging with features like log rotation, filtering, and output formatting.

Benefit: Saves time by providing a robust logging system with minimal setup.

Examples:

log4net or Serilog for .NET.

Winston or Morgan for Node.js.

Python’s logging module for Python.

8. Error Tracking Tools:

What it is: Specialized tools for tracking application errors, especially in production.

Benefit: Provides detailed error reports, including stack traces, user actions, and environment details, to help identify and fix bugs faster.

Tools: Sentry, Rollbar, or Raygun.

Example: When a user encounters an error, the tool captures the error with context (e.g., user info, environment details) and notifies the development team.

Summary:

Logging mechanisms track what’s happening in an application, which helps you monitor performance, troubleshoot issues, and improve your system.

Logs can be written to files, sent to centralized services like cloud logging, or managed using log libraries that support different log levels (e.g., INFO, ERROR).

For production systems, it's important to have structured logs and use centralized or cloud-based logging to manage and analyze the logs effectively.

## Explain Configure vs ConfigureService in .Net Core. 

In .NET Core, Configure and ConfigureServices are two important methods used in the Startup class to set up your application’s services and request processing pipeline. They are part of the configuration process for your app.

Here’s a simple explanation:

1. ConfigureServices:

Purpose: This method is used to register services that your application will use, such as database connections, dependency injection, authentication, logging, etc.

Where it's used: It's called first, during the application startup, to configure and add services to the DI (Dependency Injection) container.

What it does: It prepares the services that will be available throughout the application (e.g., controllers, middleware, etc.). Example: public void ConfigureServices(IServiceCollection services)
{
    // Registering a service (e.g., adding a database context, MVC, etc.)
    services.AddDbContext<MyDbContext>(options => options.UseSqlServer("YourConnectionString"));
    services.AddControllersWithViews();
} What happens here: We register services like DbContext for database access and ControllersWithViews for MVC support.

2. Configure:

Purpose: This method is used to define how the HTTP request pipeline should be configured.

Where it's used: It’s called after ConfigureServices and defines how the application handles HTTP requests.

What it does: This is where you configure things like middleware (e.g., routing, authentication, authorization, static files, etc.) and how requests should be processed. 

Example: 
```C#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();  // Show detailed error page in development
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");  // Show generic error page in production
        app.UseHsts();  // HTTP Strict Transport Security
    }

    app.UseHttpsRedirection();  // Redirect HTTP to HTTPS
    app.UseStaticFiles();  // Serve static files (e.g., CSS, JavaScript)
    app.UseRouting();  // Set up routing for your controllers

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    });
} 
```
What happens here: We configure middleware for exception handling, static files, HTTPS redirection, and routing. This determines how incoming requests will be processed.

In Summary:

ConfigureServices: Register services (like database access, authentication, MVC, etc.) that the app will need.

Configure: Set up the request pipeline (middleware) to handle HTTP requests, manage routing, and apply other configurations like error handling and static files.

How They Work Together:

ConfigureServices is used to add services that your app will need throughout its lifecycle.

Configure defines how requests will be processed using those services.

In short, ConfigureServices is for service registration, and Configure is for defining how HTTP requests are handled in the pipeline.

## What is Dependency Injection?  

Dependency Injection (DI) is a design pattern used in programming to make applications more flexible, modular, and easier to test. It allows you to inject (provide) the dependencies (like services or objects) that a class needs, instead of the class creating them itself.

Simple Explanation:

What it is: DI is a way of passing dependencies (e.g., objects, services) into a class from the outside, rather than creating them inside the class. This helps to decouple the class from its dependencies and makes it easier to change or replace those dependencies.

Why it's useful: It promotes loose coupling and code reusability. It also makes testing easier because you can easily mock or replace the dependencies during unit testing.

How It Works:

Imagine you have a class that needs to send an email. Instead of the class creating an email service itself, you inject the email service from the outside. This way, if you need to change the email service (e.g., from SMTP to a third-party service), you only need to change it in one place.

Example in Simple Terms:
Without Dependency Injection: 
```C#
public class UserService
{
    private EmailService _emailService;

    public UserService()
    {
        _emailService = new EmailService();  // The class creates its own dependency
    }

    public void SendWelcomeEmail(string userEmail)
    {
        _emailService.SendEmail(userEmail, "Welcome to our service!");
    }
} 
```
In this case, UserService directly creates an instance of EmailService. If you want to change the way emails are sent (e.g., using a different service), you need to modify UserService.

With Dependency Injection: 
```C#
public class UserService
{
    private IEmailService _emailService;

    // Dependency is injected through the constructor
    public UserService(IEmailService emailService)
    {
        _emailService = emailService;  // The class doesn't create its own dependency
    }

    public void SendWelcomeEmail(string userEmail)
    {
        _emailService.SendEmail(userEmail, "Welcome to our service!");
    }
}
```

In this case, UserService doesn’t create IEmailService. Instead, the IEmailService (which could be any implementation like SmtpEmailService or ThirdPartyEmailService) is injected into the UserService via the constructor.

Benefits of Dependency Injection:

Loose Coupling: Classes don’t depend directly on specific implementations of services. This makes the system more flexible.

Easier Testing: You can inject mock services (e.g., fake implementations) into classes during unit testing instead of having to rely on real services.

Code Reusability: Dependencies can be shared across multiple classes and configurations.

Maintainability: Easier to replace or change services without affecting the classes using them.

Types of Dependency Injection:

Constructor Injection: The dependency is provided through the class constructor (like the second example above).

Property Injection: The dependency is set through a property.

Method Injection: The dependency is provided through a method.

Example in .NET Core:

In .NET Core, you typically configure Dependency Injection in the Startup.cs file. Here’s an example: 
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IEmailService, SmtpEmailService>(); // Registering dependency
    services.AddTransient<UserService>(); // Registering the class that needs the dependency
} 
```
In this example, we register IEmailService and its implementation SmtpEmailService with the DI container. When the UserService is needed, the DI container will automatically inject the IEmailService dependency into it.

In Summary:

Dependency Injection helps decouple classes by providing their dependencies from the outside.

It makes the code more flexible, testable, and maintainable.

## Explain AddTransient, AddScope and AddSingleton methods with technical information.

In .NET Core, when you register services in the Dependency Injection (DI) container, you specify their lifetime using methods like AddTransient, AddScoped, and AddSingleton. These methods determine how the instances of the services are created and managed during the lifetime of the application.

Here’s a detailed explanation of each method and what happens when requests come in:

1. AddTransient:

Lifetime: Transient services are created each time they are requested.

Behavior: Every time a request is made for a transient service, a new instance of the service is created.

Use case: Suitable for lightweight services that do not maintain any state and do not need to be shared across different components or requests.

Example: A service that processes an individual task and doesn’t need to retain state between calls.

What happens when a request comes in:

A new instance of the service is created each time it is requested, even within the same HTTP request (e.g., within different methods of a controller).

Example in code: 
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IEmailService, SmtpEmailService>();
}
``` 

Behavior:

If you have two different controllers requesting the IEmailService, each will receive a new instance.

2. AddScoped:

Lifetime: Scoped services are created once per request (or per scope in other contexts).

Behavior: A new instance of the service is created at the beginning of an HTTP request and is shared throughout the duration of that request.

Use case: Suitable for services that need to maintain state during the processing of a single request, like database context or unit of work.

Example: A service that interacts with a database and needs to share the same instance within the same HTTP request.

What happens when a request comes in:

The DI container creates a new instance of the service once per HTTP request and shares that instance across all components (controllers, middlewares, etc.) that need it during the same request.

Example in code: 

```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IUserService, UserService>();
} 
```
Behavior:

If two controllers request IUserService during the same HTTP request, they will both receive the same instance of UserService.

3. AddSingleton:

Lifetime: Singleton services are created once and shared throughout the application’s lifetime.

Behavior: The same instance of the service is created the first time it is requested and shared across all subsequent requests.

Use case: Suitable for services that do not maintain state or services that are expensive to create and should be shared across the entire application, like logging, caching, or configuration management.

Example: A service that needs to maintain global state or is used frequently and should be instantiated once.

What happens when a request comes in:

The DI container creates a single instance of the service when it is first requested, and that same instance is reused for all subsequent requests throughout the application's lifetime.

Example in code: 
```C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<ICacheService, CacheService>();
} 
```
Behavior:

If the ICacheService is requested from different controllers or services throughout the application's lifetime, all will share the same instance of CacheService. 

Summary of Behavior: 
| Method           | Lifetime                           | Instance Creation                                                                           | When to Use                                                                                              |
| ---------------- | ---------------------------------- | ------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------------------- |
| **AddTransient** | Per request                        | A new instance is created every time it is requested.                                       | For lightweight, stateless services.                                                                     |
| **AddScoped**    | Per request (per HTTP request)     | A new instance is created once per HTTP request and reused within that request.             | For services that maintain state during a request, like a database context.                              |
| **AddSingleton** | Single instance throughout the app | A single instance is created once and shared across all requests during the app's lifetime. | For services that should be reused throughout the application's lifetime, like logging or configuration. |
Example Scenario to Clarify:

Assume you have a service MyService registered in all three ways, and you make multiple requests. 
```C#
services.AddTransient<IMyService, MyService>();  // Transient
services.AddScoped<IMyService, MyService>();     // Scoped
services.AddSingleton<IMyService, MyService>();  // Singleton 
```
For Transient: If you call IMyService from multiple places in the same HTTP request, each place gets a new instance.

For Scoped: All calls to IMyService within the same HTTP request will get the same instance.

For Singleton: Even across multiple HTTP requests, all calls to IMyService will use the same instance.

When to Use Each:

Transient: For lightweight services, like utilities or services that perform a single task without maintaining state.

Scoped: For services that should maintain state during a single HTTP request, such as data repositories or database contexts.

Singleton: For services that are expensive to create, need to maintain global state, or should be shared throughout the entire application.

