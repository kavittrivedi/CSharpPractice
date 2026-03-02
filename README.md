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

