# OWASP Top 10:2025 Explained

This document explains the OWASP Top 10:2025 security risks in simple language, with examples in C# .NET Core where applicable. Useful for interview preparation.

## A01:2025 - Broken Access Control
This happens when an application doesn't properly restrict what users can do. For example, a user might access another user's data or perform actions they're not allowed to.

**Example in C# .NET Core:** In an ASP.NET Core controller, if you don't check the user's role before allowing access to an admin endpoint:

```csharp
[HttpGet("admin-data")]
public IActionResult GetAdminData()
{
    // No authorization check - anyone can access!
    return Ok("Sensitive admin data");
}
```

**Fix:** Add `[Authorize(Roles = "Admin")]` attribute.

## A02:2025 - Security Misconfiguration
This happens when security settings are not properly configured, like default passwords, unnecessary services exposed, or misconfigured permissions.

**Example in C# .NET Core:** Leaving debug mode enabled in production:

```csharp
// In Program.cs
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Good for dev, bad for prod
}
```

**Fix:** Use proper error handling for production.

## A03:2025 - Software Supply Chain Failures
This involves vulnerabilities introduced through third-party components, libraries, or dependencies, including compromised supply chains.

**Example:** Using a malicious or vulnerable NuGet package.

**Fix:** Regularly update dependencies, use tools like OWASP Dependency Check, and verify package integrity.

## A04:2025 - Cryptographic Failures
This is when sensitive data isn't properly encrypted or protected. It can lead to data breaches if information like passwords or credit cards are exposed.

**Example in C# .NET Core:** Storing passwords in plain text:

```csharp
// Bad: Plain text password
var user = new User { Password = "mypassword123" };
```

**Fix:** Use hashing with libraries like BCrypt:

```csharp
var hashedPassword = BCrypt.Net.BCrypt.HashPassword("mypassword123");
```

## A05:2025 - Injection
This occurs when untrusted data is sent to an interpreter as part of a command. The most common is SQL injection.

**Example in C# .NET Core:** Using raw SQL with user input:

```csharp
// Vulnerable to SQL injection
var query = $"SELECT * FROM Users WHERE Username = '{username}'";
```

**Fix:** Use parameterized queries or Entity Framework:

```csharp
var user = context.Users.FirstOrDefault(u => u.Username == username);
```

## A06:2025 - Insecure Design
This is about flaws in the application's architecture or design that make it vulnerable, even if implemented correctly.

**Example:** Designing an API without rate limiting, allowing brute force attacks.

**In C# .NET Core:** Implement rate limiting using middleware.

## A07:2025 - Authentication Failures
Problems with how users are identified and authenticated, like weak passwords or poor session management.

**Example in C# .NET Core:** Allowing weak passwords:

```csharp
// No password policy
if (password.Length < 4) // Too weak!
{
    // Allow registration
}
```

**Fix:** Implement strong password requirements.

## A08:2025 - Software or Data Integrity Failures
This involves issues with ensuring that software and data are not tampered with, like insecure deserialization or CI/CD pipeline vulnerabilities.

**Example:** Deserializing untrusted data without validation.

**In C# .NET Core:** Use safe deserialization options.

## A09:2025 - Security Logging and Alerting Failures
Not properly logging security events or setting up alerts for suspicious activity.

**Example in C# .NET Core:** Not logging failed login attempts.

**Fix:** Add logging for security events using Serilog or built-in logging, and configure alerts.

## A10:2025 - Mishandling of Exceptional Conditions
This occurs when applications don't handle errors or exceptions properly, potentially leading to information disclosure, crashes, or further attacks.

**Example in C# .NET Core:** Exposing stack traces in production:

```csharp
// In production, this might leak sensitive info
try
{
    // Some code
}
catch (Exception ex)
{
    return BadRequest(ex.Message); // Exposes internal details
}
```

**Fix:** Use generic error messages and log details internally.