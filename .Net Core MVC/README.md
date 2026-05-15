# .Net Core MVC Interview Practice

## Explain MVC Page Life Cycle

The MVC (Model-View-Controller) page life cycle describes the flow of how a request is processed in an ASP.NET MVC application from start to finish. Here's a simple breakdown:

1. **Request**: A user makes a request, like typing a URL or clicking a link, which is sent to the server.
2. **Routing**: The request goes through the Routing module, which decides which controller and action method should handle it based on the URL pattern.
3. **Controller**: The Controller receives the request. It processes any user input (like form data), interacts with the Model (business logic or database), and decides what data to send to the view.
4. **Model**: The Model contains the data or business logic. If data is required, the controller communicates with the model to get or modify data, often by querying a database.
5. **View**: The controller passes data from the model to the View, which is responsible for rendering the HTML page that the user will see.
6. **Response**: Finally, the View generates the HTML, and it is sent as a response to the user's browser.


In short, it follows this cycle: **Request → Routing → Controller → Model → View → Response**.

##  How Many Types of Filters Are There in .NET Core and in Which Order Do They Execute?

In .NET Core, there are **five types of filters** used to execute logic before or after certain stages in an action method lifecycle:

1. **Authorization Filters**: Handle user authorization before the action method executes.
2. **Resource Filters**: Deal with resource management before and after an action is executed.
3. **Action Filters**: Run before and after the execution of an action method.
4. **Exception Filters**: Handle exceptions thrown during the action method.
5. **Result Filters**: Run before and after the action result (e.g., view or JSON result).

### Execution Order:

1. **Authorization**
2. **Resource**
3. **Action**
4. **Exception**
5. **Result**

## What is Difference Between @Html.TextBoxFor and @Html.TextBox.

The difference between `@Html.TextBoxFor` and `@Html.TextBox` in ASP.NET MVC is:

- **`@Html.TextBoxFor`**: Strongly typed. It is linked to a specific property of a model, ensuring that the input field is bound to that property. This helps with type safety and reduces errors during compile time.

  **Example:** `@Html.TextBoxFor(model => model.Name)` (automatically binds to the Name property in the model).

- **`@Html.TextBox`**: Weakly typed. You manually specify the name of the field as a string, so it is not tied to the model directly. It's more flexible but lacks the type safety of TextBoxFor.

  **Example:** `@Html.TextBox("Name")` (just creates a textbox with the name "Name").

In short: TextBoxFor is model-bound and type-safe, while TextBox is not bound to a model directly and is less safe but more flexible.

## Explain All Types of Return Type (e.g., ActionResult, ViewResult, JsonResult etc.) of MVC Controller's and API Controller's Action Method.

Here's a quick overview of the different types of return values for action methods in MVC and API controllers:

### MVC Controller Return Types

- **ActionResult**: A base class for various result types. It allows you to return different types of results, making it flexible.

  **Example:** `return new ViewResult();`

  **Usage:** You can return views, JSON, redirects, etc.

- **ViewResult**: Used to render a view (an HTML page).

  **Example:** `return View();`

  **Usage:** Displays an HTML view to the user.

- **JsonResult**: Returns JSON data, useful for AJAX requests.

  **Example:** `return Json(new { Name = "John" });`

  **Usage:** Sends JSON-formatted data to the client.

- **RedirectResult**: Redirects the user to another URL.

  **Example:** `return Redirect("/Home/Index");`

  **Usage:** Navigates to a different action or page.

- **RedirectToActionResult**: Redirects to a specific action in the controller.

  **Example:** `return RedirectToAction("Index");`

  **Usage:** Redirects to another action in the same or different controller.

- **PartialViewResult**: Returns a partial view (a portion of HTML).

  **Example:** `return PartialView("_MyPartialView");`

  **Usage:** Renders only part of a page without reloading the entire view.

- **FileResult**: Returns a file to download.

  **Example:** `return File(filePath, "application/pdf");`

  **Usage:** Used for file downloads.

- **ContentResult**: Returns plain text, HTML, or any other string content.

  **Example:** `return Content("Hello, world!");`

  **Usage:** For custom text responses, like HTML or plain text.

- **EmptyResult**: Represents no response (does nothing).

  **Example:** `return new EmptyResult();`

  **Usage:** When you don't want to return any content.

### API Controller Return Types

- **IHttpActionResult (in Web API 2)**: A standard return type for Web API controllers that provides better control over HTTP responses.

  **Example:** `return Ok(product);`

  **Usage:** Simplifies returning HTTP status codes with content.

- **HttpResponseMessage**: A more detailed HTTP response with headers, status code, and content.

  **Example:** `return Request.CreateResponse(HttpStatusCode.OK, product);`

  **Usage:** Useful when you need more control over the response.

- **OkResult**: Returns a 200 OK response.

  **Example:** `return Ok();`

  **Usage:** To indicate a successful request.

- **NotFoundResult**: Returns a 404 Not Found response.

  **Example:** `return NotFound();`

  **Usage:** When the requested resource is not found.

- **BadRequestResult**: Returns a 400 Bad Request response.

  **Example:** `return BadRequest("Invalid input");`

  **Usage:** When the request data is invalid.

- **CreatedResult**: Returns a 201 Created response, typically used when creating new resources.

  **Example:** `return Created("api/products/1", newProduct);`

  **Usage:** To indicate successful resource creation.

- **NoContentResult**: Returns a 204 No Content response.

  **Example:** `return NoContent();`

  **Usage:** When an action succeeds but there is no content to return.

**Summary:**

- MVC return types focus on rendering views, redirects, or data (like JSON).
- API return types focus on sending HTTP responses (like OK, NotFound, Created) with or without data.

## Explain .NET Core MVC and API Controller Return Types

In ASP.NET Core MVC and ASP.NET Core API, the types of return values from action methods are similar to those in previous versions, but with some refinements. Here's a short overview of the return types for both MVC and API controllers in .NET Core:

### ASP.NET Core MVC Controller Return Types

- **IActionResult**: The most common return type, which allows flexibility in returning different types of results (e.g., views, JSON, redirects). It's similar to ActionResult but preferred in ASP.NET Core for flexibility.

  **Example:** `public IActionResult Index() { return View(); }`

- **ViewResult**: Used to return a view (HTML page).

  **Example:** `return View();`

  **Usage:** Renders a view to the user.

- **JsonResult**: Returns JSON data to the client, typically used in AJAX requests.

  **Example:** `return Json(new { Name = "John" });`

  **Usage:** Sends data in JSON format.

- **RedirectResult**: Redirects the user to another URL.

  **Example:** `return Redirect("/Home/Index");`

  **Usage:** Redirects to a specific URL.

- **RedirectToActionResult**: Redirects to another action within the controller or to another controller.

  **Example:** `return RedirectToAction("Index", "Home");`

  **Usage:** Navigates to an action within the app.

- **PartialViewResult**: Returns a partial view, which renders a portion of HTML without reloading the whole page.

  **Example:** `return PartialView("_PartialViewName");`

  **Usage:** Used for AJAX requests or updating parts of a page.

- **FileResult**: Returns a file for download, such as a PDF, image, or any file type.

  **Example:** `return File("filePath", "application/pdf");`

  **Usage:** Sends a file to the client for download.

- **ContentResult**: Returns a plain string, HTML, or any textual content.

  **Example:** `return Content("Hello World!");`

  **Usage:** For text-based responses like raw HTML or plain text.

- **EmptyResult**: Represents no response, doing nothing.

  **Example:** `return new EmptyResult();`

  **Usage:** When nothing should be returned.

### ASP.NET Core API Controller Return Types

- **ActionResult<T>**: A new return type in ASP.NET Core that allows you to return a value (T) or an IActionResult. It combines the flexibility of returning an HTTP status code with data.

  **Example:** `public ActionResult<Product> GetProduct(int id) { return Ok(product); }`

  **Usage:** Return HTTP status codes (e.g., 200 OK) along with data.

- **OkResult**: Returns an HTTP 200 OK status without any content.

  **Example:** `return Ok();`

  **Usage:** Indicates a successful request without a body.

- **OkObjectResult**: Returns an HTTP 200 OK status with data in the response body.

  **Example:** `return Ok(product);`

  **Usage:** Sends a successful response with data.

- **NotFoundResult**: Returns an HTTP 404 Not Found status when the requested resource does not exist.

  **Example:** `return NotFound();`

  **Usage:** Used when the resource can't be found.

- **BadRequestResult**: Returns an HTTP 400 Bad Request status.

  **Example:** `return BadRequest();`

  **Usage:** When the request contains invalid data.

- **BadRequestObjectResult**: Returns an HTTP 400 Bad Request status along with a message or validation error information.

  **Example:** `return BadRequest("Invalid input");`

  **Usage:** For validation errors or bad input scenarios.

- **CreatedResult**: Returns an HTTP 201 Created status with a location header pointing to the newly created resource.

  **Example:** `return Created("api/products/1", newProduct);`

  **Usage:** When a new resource has been successfully created.

- **NoContentResult**: Returns an HTTP 204 No Content status, indicating the request was successful, but there's no content to return.

  **Example:** `return NoContent();`

  **Usage:** For actions that don't need to return any content after successful execution (e.g., PUT or DELETE).

- **UnauthorizedResult**: Returns an HTTP 401 Unauthorized status, indicating the user is not authenticated.

  **Example:** `return Unauthorized();`

  **Usage:** When the user is not authorized to access the resource.

- **ForbidResult**: Returns an HTTP 403 Forbidden status when the user is authenticated but lacks permission to access the resource.

  **Example:** `return Forbid();`

  **Usage:** When access to a resource is denied due to insufficient permissions.

**Key Differences in .NET Core:**

- **ActionResult<T> in Web API**: Combines IActionResult and the ability to return data directly, making it more flexible for API responses.
- More consistent and streamlined approach between MVC and API return types in .NET Core, using IActionResult as a common return type across both MVC and API controllers.

## Explain Area in .NET Core 6.

In ASP.NET Core 6, an Area is a way to organize large applications by grouping related functionality (like controllers, views, and models) into separate sections. This helps keep your codebase organized, especially when the application has many features.

**Key Points:**

- **Separation**: Areas allow you to break your application into logical sections. For example, you can have areas for "Admin", "User", or "Reports".
- **Structure**: Each area has its own folder structure containing controllers, views, and models.

  **Example:**
  ```
  Areas/
    Admin/
      Controllers/
      Views/
      Models/
    User/
      Controllers/
      Views/
      Models/
  ```

- **Routing**: When using areas, the URL reflects the area. For example, the route for an Admin dashboard might be `/Admin/Dashboard/Index`.
- **Usage**: Useful in large applications where separating concerns by sections (like "Admin" and "Customer") makes the project easier to manage.

**Benefits:**

- Helps in organizing large applications.
- Enables grouping of related functionalities.
- Makes routing easier to understand in complex applications.

**How to Use:**

- Create an Area folder.
- Define controllers and views inside the area.
- Use `[Area("AreaName")]` attribute on the controllers to specify that they belong to an area.

In short, Areas help you organize and modularize large applications by grouping related functionality into distinct sections.

## Explain ViewComponent.  v1

In ASP.NET Core, a View Component is a reusable component that encapsulates rendering logic in a way that allows you to create complex UI elements in a clean and organized manner. Here's a simple breakdown:

**Key Points:**

- **Purpose**: View Components are used to create dynamic and reusable portions of a view, similar to partial views but more powerful. They can include their own logic for retrieving and processing data.
- **Independence**: Unlike partial views, View Components do not rely on a model from the parent view. They can fetch their own data and can be reused across different views.
- **Usage**: View Components are useful for rendering reusable UI elements, like a navigation menu, a sidebar, or a comment section.

**Creating a View Component:**

- **Class**: You create a class that inherits from ViewComponent.
- **Method**: Define a method (often named Invoke) that contains the logic for what the component should do and what data it should return.
- **View**: Create a corresponding view file in the `Views/Shared/Components` folder.

**Invoking a View Component**: You can call a View Component in a view using the `@Component.InvokeAsync("ComponentName")` method.

**Example:** `@await Component.InvokeAsync("Navigation")`

**Benefits:**

- Promotes code reuse and separation of concerns.
- Encapsulates rendering logic and data fetching.
- Makes it easier to maintain and manage complex UIs.

In short, View Components are a powerful way to create reusable, dynamic parts of your UI in ASP.NET Core applications.

## Example of ViewComponent

```csharp
// File: Components/GreetingViewComponent.cs
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class GreetingViewComponent : ViewComponent
    {
        // Method that will contain the logic for the View Component
        public IViewComponentResult Invoke(string name)
        {
            // You can add any logic here, like fetching data from a database
            return View("Default", name);
        }
    }
}
```

### Step 2: Create the View for the View Component

Create a view for your View Component. The view file should be placed in the `Views/Shared/Components/Greeting/` directory. Create a file named `Default.cshtml`.

```html
<!-- File: Views/Shared/Components/Greeting/Default.cshtml -->
<div>
    <h2>Hello, @Model!</h2>
</div>
```

### Step 3: Use the View Component in a View

Invoke the View Component in your main view (e.g., `Index.cshtml`) to display the greeting.

**Summary of Steps:**

1. Create the View Component class (`GreetingViewComponent`) with an `Invoke` method.
2. Create the view for the component (`Default.cshtml`) to define how it should be rendered.
3. Invoke the component in your main view (`Index.cshtml`) to display the greeting.

**Result:**

When you run your application and navigate to the Index view, you will see the greeting "Hello, John!" rendered on the page as part of the View Component. This demonstrates how to create a reusable UI component that encapsulates both logic and rendering.



## View Components in ASP.NET Core V2

A **View Component** in ASP.NET Core is a reusable piece of UI logic that is similar to partial views but more powerful. It allows you to encapsulate rendering logic and is used when you need to provide complex data or functionality to the view. It doesn't depend on controllers, and it's ideal for reusable components like sidebars, footers, or widgets.

### How to Use It:

1. Create a class inheriting from `ViewComponent`.
2. Implement `Invoke` or `InvokeAsync` methods.
3. Call it from a view using `@Component.InvokeAsync("ComponentName")`.

### Usage:

* Sidebar components
* Navigation menus
* Widgets like recent posts, comments, etc.

## Explain Difference Between PartialView and ViewComponent with Example.

### Partial View

**Definition:** A Partial View is a reusable portion of a view that can be rendered within another view. It is similar to a regular view but is designed to be included in other views.

**Usage:** It relies on the model passed from the parent view and is primarily used for rendering UI without additional logic.

**Example:**

Create a Partial View (e.g., `_UserDetails.cshtml`):

```html
<!-- File: Views/Shared/_UserDetails.cshtml -->
<div>
    <h3>User Details</h3>
    <p>Name: @Model.Name</p>
    <p>Email: @Model.Email</p>
</div>
```

Use the Partial View in a Parent View (e.g., `Index.cshtml`):

```html
@model YourNamespace.Models.User

<!-- File: Views/Home/Index.cshtml -->
<h1>Welcome to the Home Page!</h1>

<!-- Render the Partial View and pass the model -->
@await Html.PartialAsync("_UserDetails", Model)
```

### View Component

**Definition:** A View Component is a more powerful reusable component that can encapsulate both logic and rendering. It can retrieve its own data and does not depend on the parent view's model.

**Usage:** It is more flexible than a partial view and can handle more complex scenarios.

**Example:**

Create a View Component (e.g., `UserGreetingViewComponent.cs`):

```csharp
// File: Components/UserGreetingViewComponent.cs
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Components
{
    public class UserGreetingViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string name)
        {
            return View("Default", name);
        }
    }
}
```

Create the View for the View Component (e.g., `Default.cshtml`):

```html
<!-- File: Views/Shared/Components/UserGreeting/Default.cshtml -->
<div>
    <h2>Hello, @Model!</h2>
</div>
```

Use the View Component in a Parent View (e.g., `Index.cshtml`):

```html
@{
    ViewData["Title"] = "Home Page";
}

<h1>Welcome to the Home Page!</h1>

<!-- Call the View Component and pass a name -->
@await Component.InvokeAsync("UserGreeting", new { name = "John" })
```

### Key Differences

- **Data Dependency:**
  - **Partial View:** Relies on the parent view's model.
  - **View Component:** Can fetch its own data independently.

- **Complexity:**
  - **Partial View:** Simpler and primarily used for rendering.
  - **View Component:** More complex and can include business logic.

- **Usage:**
  - **Partial View:** Used when you just want to render a piece of UI.
  - **View Component:** Used when you need a reusable component with its own logic.

In summary, use Partial Views for simple UI pieces that depend on the parent model, and use View Components when you need a more robust, independent component that can handle its own data and logic.



##  What Is the Difference Between HTML Helper Controls and Tag Helpers?

**HTML Helper Controls** and **Tag Helpers** are both ways to generate HTML in ASP.NET Core, but they have key differences:

1. **HTML Helpers**: These are methods in Razor views (`@Html.TextBoxFor`, `@Html.ActionLink`) that generate HTML elements. They are based on C# syntax and can be harder to read since they look more like code than HTML.

2. **Tag Helpers**: These use a syntax similar to standard HTML elements, making them more intuitive and readable. They enable server-side processing using attributes (e.g., `<input asp-for="Name" />`) and are closer to how standard HTML is written.

**Example**:

* HTML Helper: `@Html.TextBoxFor(m => m.Name)`
* Tag Helper: `<input asp-for="Name" />`

## How File Upload and Download Works in .NET Core 6 MVC and API?

In ASP.NET Core 6 MVC and API, file upload and download functionality can be implemented using controllers and Razor views. Here's how you can achieve both:

### File Upload

#### 1. **Creating the Upload Form (MVC)**

In a Razor view, create a form that allows users to upload files:

```html
<form asp-controller="FileUpload" asp-action="Upload" method="post" enctype="multipart/form-data">
    <input type="file" name="file" />
    <button type="submit">Upload</button>
</form>
```

#### 2. **Handling File Upload in the Controller**

Create an action method in your controller to handle the file upload:

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class FileUploadController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return RedirectToAction("UploadSuccess");
        }

        return View();
    }

    public IActionResult UploadSuccess()
    {
        return View();
    }
}
```

### File Download

#### 1. **Creating the Download Method in the Controller**

You can create a method to allow users to download files:

```csharp
public class FileDownloadController : Controller
{
    [HttpGet]
    public IActionResult Download(string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return File(fileBytes, "application/octet-stream", fileName); // Set appropriate MIME type
    }
}
```

#### 2. **Link to Download the File (MVC)**

In your view, create a link to download the file:

```html
<a asp-controller="FileDownload" asp-action="Download" asp-route-fileName="example.txt">Download Example File</a>
```

### API Implementation for File Upload and Download

#### 1. **File Upload API Endpoint**

Create an API controller for handling file uploads:

```csharp
[ApiController]
[Route("api/[controller]")]
public class FileUploadApiController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath = path });
        }

        return BadRequest("No file uploaded.");
    }
}
```

#### 2. **File Download API Endpoint**

Create an API endpoint for downloading files:

```csharp
[HttpGet("download/{fileName}")]
public IActionResult Download(string fileName)
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

    if (!System.IO.File.Exists(filePath))
    {
        return NotFound();
    }

    var fileBytes = System.IO.File.ReadAllBytes(filePath);
    return File(fileBytes, "application/octet-stream", fileName); // Set appropriate MIME type
}
```

### Summary

* **File Upload**: In MVC, use forms with `enctype="multipart/form-data"` and handle uploads in the controller using `IFormFile`. In API, use a similar approach to handle file uploads.
* **File Download**: Use the `File` method in the controller to return files, specifying the appropriate MIME type and file name. In MVC, create links in your views to trigger downloads.

By following these steps, you can successfully implement file upload and download functionality in your .NET Core 6 MVC applications and APIs.

## How to Write If Condition in MVC View File

In .NET Core MVC, view files use Razor syntax. Razor allows us to write C# code inside an HTML page.

To write an `if` condition in a `.cshtml` view file, start with `@if`.

### Basic Syntax

```cshtml
@if (condition)
{
    // HTML or C# code here
}
```

Example:

```cshtml
@if (Model.IsActive)
{
    <p>User is active</p>
}
```

Here:

* `@if` tells Razor that we are writing a C# if condition.
* `Model.IsActive` is the condition.
* If the condition is true, then `<p>User is active</p>` will be shown on the page.
* If the condition is false, nothing will be shown.

### If Else Example

```cshtml
@if (Model.IsActive)
{
    <p>User is active</p>
}
else
{
    <p>User is not active</p>
}
```

Here, if `Model.IsActive` is true, the first message will be shown. Otherwise, the message inside `else` will be shown.

### Example with ViewBag

```cshtml
@if (ViewBag.Message != null)
{
    <p>@ViewBag.Message</p>
}
```

This checks whether `ViewBag.Message` has a value. If it has a value, then the message is displayed.

### Example with List Count

```cshtml
@if (Model.Products != null && Model.Products.Count > 0)
{
    <p>Products are available</p>
}
else
{
    <p>No products found</p>
}
```

This checks whether the product list is not null and contains at least one item.

### Important Points

* Use `@if` in the view file.
* Write the condition inside round brackets `()`.
* Write the HTML output inside curly brackets `{ }`.
* You can use `else` when you want to show another output if the condition is false.
* Razor can understand both C# code and HTML inside the same view file.

## How to Write For Loop in MVC View File

In .NET Core MVC, we can write a `for` loop in a `.cshtml` view file using Razor syntax.

To write a `for` loop, start with `@for`.

### Basic Syntax

```cshtml
@for (int i = 0; i < 5; i++)
{
    <p>Number: @i</p>
}
```

Here:

* `@for` tells Razor that we are writing a C# for loop.
* `int i = 0` means the loop starts from 0.
* `i < 5` means the loop will run while `i` is less than 5.
* `i++` means the value of `i` increases by 1 every time.
* `<p>Number: @i</p>` displays the current value of `i` on the page.

Output:

```text
Number: 0
Number: 1
Number: 2
Number: 3
Number: 4
```

### For Loop with Model List

If your model has a list, you can use a `for` loop to display each item.

Example:

```cshtml
@for (int i = 0; i < Model.Products.Count; i++)
{
    <p>@Model.Products[i].Name</p>
}
```

Here:

* `Model.Products.Count` gives the total number of products.
* `Model.Products[i]` gets one product from the list.
* `.Name` displays the product name.

### For Loop with HTML Table

```cshtml
<table>
    <tr>
        <th>No.</th>
        <th>Product Name</th>
    </tr>

    @for (int i = 0; i < Model.Products.Count; i++)
    {
        <tr>
            <td>@(i + 1)</td>
            <td>@Model.Products[i].Name</td>
        </tr>
    }
</table>
```

Here, the loop creates one table row for each product.

`@(i + 1)` is used because `i` starts from 0, but normally we want to show numbering from 1.

### Important Points

* Use `@for` in the view file.
* Write the loop condition inside round brackets `()`.
* Write HTML inside curly brackets `{ }`.
* Use `@i` to print the value of `i`.
* Use `Model.ListName[i]` to get an item from a list by index.
* In MVC views, `foreach` is usually simpler for displaying lists, but `for` is useful when you need the index number.

## ViewBag, ViewData and TempData in .NET Core MVC

In .NET Core MVC, `ViewBag`, `ViewData` and `TempData` are used to pass data from controller to view.

They are useful when we want to send small data like message, title, success message, error message, etc.

### ViewBag

`ViewBag` is used to pass data from controller to view.

It uses dynamic properties, so we can create any property name directly.

Controller example:

```csharp
public IActionResult Index()
{
    ViewBag.Message = "Welcome to MVC";
    ViewBag.Name = "Rahul";

    return View();
}
```

View example:

```cshtml
<h2>@ViewBag.Message</h2>
<p>@ViewBag.Name</p>
```

Here:

* `ViewBag.Message` stores the message.
* `ViewBag.Name` stores the name.
* These values are displayed in the view using `@ViewBag.Message` and `@ViewBag.Name`.

Important point:

`ViewBag` data is available only for the current request. If the page redirects to another action, then `ViewBag` data will be lost.

### ViewData

`ViewData` is also used to pass data from controller to view.

It stores data in key-value format.

Controller example:

```csharp
public IActionResult Index()
{
    ViewData["Message"] = "Welcome to MVC";
    ViewData["Name"] = "Rahul";

    return View();
}
```

View example:

```cshtml
<h2>@ViewData["Message"]</h2>
<p>@ViewData["Name"]</p>
```

Here:

* `"Message"` is the key.
* `"Welcome to MVC"` is the value.
* We access the value in view using `ViewData["Message"]`.

Important point:

`ViewData` data is also available only for the current request. If redirect happens, then `ViewData` data will be lost.

### TempData

`TempData` is used to pass data from one action method to another action method.

It is mostly used after redirect.

Controller example:

```csharp
public IActionResult Save()
{
    TempData["SuccessMessage"] = "Record saved successfully";

    return RedirectToAction("Index");
}

public IActionResult Index()
{
    return View();
}
```

View example:

```cshtml
@if (TempData["SuccessMessage"] != null)
{
    <p>@TempData["SuccessMessage"]</p>
}
```

Here:

* `TempData["SuccessMessage"]` stores the success message.
* `RedirectToAction("Index")` redirects the user to the Index action.
* The message is still available after redirect.
* This is why `TempData` is useful for success and error messages.

Important point:

`TempData` keeps data for the next request. After reading the value, it is usually removed automatically.

### Difference Between ViewBag, ViewData and TempData

| Feature | ViewBag | ViewData | TempData |
|---|---|---|---|
| Used for | Controller to View | Controller to View | One action to another action |
| Syntax type | Dynamic property | Key-value pair | Key-value pair |
| Available after redirect | No | No | Yes |
| Data lifetime | Current request only | Current request only | Next request |
| Common use | Small view data | Small view data | Success/error message after redirect |

### Simple Memory Trick

* Use `ViewBag` when you want simple and easy syntax.
* Use `ViewData` when you want key-value syntax.
* Use `TempData` when you want data after redirect.

### Important Points

* `ViewBag` and `ViewData` are used to send data from controller to view in the same request.
* `TempData` is used when data is needed after redirect.
* `ViewBag` internally uses `ViewData`.
* For large or strongly typed data, using a model or view model is better than `ViewBag`, `ViewData` or `TempData`.

