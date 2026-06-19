You want **Microsoft Entra ID authentication**, not classic “Windows Authentication”. In ASP.NET Core MVC, the usual approach is **OpenID Connect with Microsoft.Identity.Web**.

Steps:

1. **Register app in Microsoft Entra ID**

Go to:

`Microsoft Entra admin center` → `Microsoft Entra ID` → `App registrations` → `New registration`

Use something like:

- Name: `MyMvcApp`
- Supported account types: usually `Single tenant`
- Redirect URI:
  - Platform: `Web`
  - URI: `https://localhost:5001/signin-oidc`

After registration, copy:

- `Application (client) ID`
- `Directory (tenant) ID`

Microsoft docs: https://learn.microsoft.com/en-us/entra/identity-platform/tutorial-web-app-dotnet-prepare-app

2. **Install NuGet package**

In your MVC project:

```powershell
dotnet add package Microsoft.Identity.Web
dotnet add package Microsoft.Identity.Web.UI
```

3. **Add config in `appsettings.json`**

```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "yourdomain.onmicrosoft.com",
    "TenantId": "YOUR_TENANT_ID",
    "ClientId": "YOUR_CLIENT_ID",
    "CallbackPath": "/signin-oidc"
  }
}
```

4. **Update `Program.cs`**

For .NET 6/7/8/9/10 style MVC apps:

```csharp
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
```

5. **Protect controllers/actions**

To require login for a controller:

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

Or protect only one action:

```csharp
[Authorize]
public IActionResult SecurePage()
{
    return View();
}
```

6. **Add sign-in / sign-out links**

In `_Layout.cshtml`, add:

```html
<partial name="_LoginPartial" />
```

If `_LoginPartial.cshtml` does not exist, create it under:

`Views/Shared/_LoginPartial.cshtml`

```cshtml
@if (User.Identity?.IsAuthenticated == true)
{
    <span>Hello @User.Identity.Name</span>
    <a asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignOut">Sign out</a>
}
else
{
    <a asp-area="MicrosoftIdentity" asp-controller="Account" asp-action="SignIn">Sign in</a>
}
```

7. **Run and test**

```powershell
dotnet run
```

Open:

```text
https://localhost:5001
```

You should be redirected to Microsoft Entra ID login, then back to your MVC app at `/signin-oidc`.

Important: make sure the redirect URI in Entra exactly matches your running app URL, for example:

```text
https://localhost:5001/signin-oidc
```

Redirect URI docs: https://learn.microsoft.com/en-us/entra/identity-platform/reply-url