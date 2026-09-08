# Unit Testing Interview Questions and Answers for .NET

This document contains common interview questions and simple answers about unit testing in .NET using **xUnit**, **NUnit**, and **MSTest**.

## Basic Unit Testing Questions

### 1. What is unit testing?

Unit testing is the process of testing a small part of code, usually one method or one class, to check whether it works as expected.

Example: Testing a `CalculateTotal()` method separately without running the full application.

### 2. Why do we write unit tests?

We write unit tests to:

- Find bugs early
- Make code safer to change
- Improve code quality
- Check business logic automatically
- Reduce manual testing effort
- Give confidence during refactoring

### 3. What is a unit in unit testing?

A unit is the smallest testable part of an application. In .NET, it is usually a method, class, or small service.

### 4. What is the difference between unit testing and integration testing?

Unit testing tests a small piece of code in isolation.

Integration testing checks whether multiple parts of the system work together, such as API plus database, service plus repository, or application plus external API.

### 5. What is the difference between unit testing and functional testing?

Unit testing checks internal code behavior.

Functional testing checks whether the application feature works from the user's point of view.

### 6. What is the difference between unit testing and end-to-end testing?

Unit testing checks a small method or class.

End-to-end testing checks the full application flow from start to finish, like login, add item to cart, and checkout.

### 7. What are the main qualities of a good unit test?

A good unit test should be:

- Fast
- Simple
- Independent
- Repeatable
- Easy to read
- Focused on one behavior
- Not dependent on external systems

### 8. What does AAA mean in unit testing?

AAA means:

- **Arrange**: Prepare test data and objects
- **Act**: Call the method being tested
- **Assert**: Check the result

Example:

```csharp
[Fact]
public void Add_ShouldReturnSum()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    var result = calculator.Add(2, 3);

    // Assert
    Assert.Equal(5, result);
}
```

### 9. What is an assertion?

An assertion checks whether the actual result matches the expected result.

Example:

```csharp
Assert.Equal(10, result);
```

### 10. What happens when an assertion fails?

The test fails, and the test runner shows an error message explaining what was expected and what was actually returned.

### 11. What is test coverage?

Test coverage shows how much application code is executed by tests. For example, 80% coverage means tests executed 80% of the code.

### 12. Is 100% code coverage always good?

Not always. 100% coverage only means the code was executed. It does not guarantee that all important scenarios were tested correctly.

### 13. What is a test case?

A test case is a specific scenario that verifies one expected behavior.

Example: "When user enters valid email and password, login should succeed."

### 14. What is a test suite?

A test suite is a collection of test cases.

### 15. What is a test runner?

A test runner is a tool that discovers and runs tests. Examples include Visual Studio Test Explorer, `dotnet test`, ReSharper, Rider, and Azure DevOps pipelines.

### 16. What command is used to run .NET tests?

```bash
dotnet test
```

### 17. Which unit testing frameworks are commonly used in .NET?

Common frameworks are:

- xUnit
- NUnit
- MSTest

### 18. Which unit testing framework is best for .NET?

There is no single best framework for every project.

- xUnit is popular in modern .NET projects.
- NUnit is mature and feature-rich.
- MSTest is Microsoft's official test framework and integrates well with Visual Studio.

### 19. What is the purpose of a test project?

A test project contains test classes, test methods, test data, and test dependencies for testing the main application project.

### 20. What is the naming convention for test methods?

A common naming style is:

```text
MethodName_StateUnderTest_ExpectedBehavior
```

Example:

```csharp
CalculateDiscount_WhenCustomerIsPremium_ReturnsTenPercentDiscount()
```

## xUnit Interview Questions

### 21. What is xUnit?

xUnit is a popular open-source unit testing framework for .NET. It is commonly used in modern .NET Core and .NET applications.

### 22. Which NuGet packages are commonly used for xUnit?

Common packages are:

```text
xunit
xunit.runner.visualstudio
Microsoft.NET.Test.Sdk
```

### 23. Which attribute is used for a simple test in xUnit?

xUnit uses the `[Fact]` attribute for a simple test method.

```csharp
[Fact]
public void Test1()
{
}
```

### 24. Which attribute is used for parameterized tests in xUnit?

xUnit uses `[Theory]` with data attributes like `[InlineData]`, `[MemberData]`, and `[ClassData]`.

```csharp
[Theory]
[InlineData(2, 3, 5)]
[InlineData(5, 5, 10)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    var calculator = new Calculator();

    var result = calculator.Add(a, b);

    Assert.Equal(expected, result);
}
```

### 25. What is the difference between Fact and Theory in xUnit?

`[Fact]` is used when the test has fixed data.

`[Theory]` is used when the same test should run with different input values.

### 26. Does xUnit use SetUp and TearDown attributes?

No. xUnit does not use `[SetUp]` or `[TearDown]`.

It uses:

- Constructor for setup before each test
- `IDisposable.Dispose()` for cleanup after each test
- `IClassFixture<T>` for shared setup across tests in one class
- `ICollectionFixture<T>` for shared setup across multiple test classes

### 27. How do you run setup code before each test in xUnit?

Use the test class constructor.

```csharp
public class CalculatorTests
{
    private readonly Calculator _calculator;

    public CalculatorTests()
    {
        _calculator = new Calculator();
    }
}
```

### 28. How do you run cleanup code after each test in xUnit?

Implement `IDisposable`.

```csharp
public class FileTests : IDisposable
{
    public void Dispose()
    {
        // Cleanup code
    }
}
```

### 29. What is IClassFixture in xUnit?

`IClassFixture<T>` is used when multiple tests in the same test class need shared setup data or expensive objects.

```csharp
public class DatabaseFixture
{
    public DatabaseFixture()
    {
        // Start database connection
    }
}

public class UserServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public UserServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
}
```

### 30. What is ICollectionFixture in xUnit?

`ICollectionFixture<T>` shares fixture data across multiple test classes.

### 31. Are xUnit tests run in parallel by default?

Yes. xUnit can run tests in parallel by default. This improves speed but can cause problems if tests share mutable state.

### 32. How can you disable parallel execution in xUnit?

You can use assembly-level configuration:

```csharp
[assembly: CollectionBehavior(DisableTestParallelization = true)]
```

### 33. How do you skip a test in xUnit?

Use the `Skip` property.

```csharp
[Fact(Skip = "Temporary disabled")]
public void Test1()
{
}
```

### 34. How do you test exceptions in xUnit?

Use `Assert.Throws<T>()`.

```csharp
[Fact]
public void Divide_ByZero_ThrowsException()
{
    var calculator = new Calculator();

    Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
}
```

### 35. How do you test async exceptions in xUnit?

Use `Assert.ThrowsAsync<T>()`.

```csharp
[Fact]
public async Task GetUserAsync_WhenUserNotFound_ThrowsException()
{
    await Assert.ThrowsAsync<InvalidOperationException>(
        () => service.GetUserAsync(1));
}
```

### 36. How do you write async tests in xUnit?

Use `async Task`.

```csharp
[Fact]
public async Task GetDataAsync_ReturnsData()
{
    var result = await service.GetDataAsync();

    Assert.NotNull(result);
}
```

### 37. Should xUnit test methods return void or Task?

Synchronous tests can return `void`.

Asynchronous tests should return `Task`, not `async void`.

### 38. What assertion library does xUnit provide?

xUnit provides the `Assert` class, which includes methods like:

- `Assert.Equal`
- `Assert.NotEqual`
- `Assert.True`
- `Assert.False`
- `Assert.Null`
- `Assert.NotNull`
- `Assert.Contains`
- `Assert.Throws`

## NUnit Interview Questions

### 39. What is NUnit?

NUnit is a popular open-source unit testing framework for .NET. It is mature, flexible, and widely used.

### 40. Which NuGet packages are commonly used for NUnit?

Common packages are:

```text
NUnit
NUnit3TestAdapter
Microsoft.NET.Test.Sdk
```

### 41. Which attribute marks a test class in NUnit?

NUnit uses `[TestFixture]`.

```csharp
[TestFixture]
public class CalculatorTests
{
}
```

In modern NUnit, `[TestFixture]` is often optional if the class contains `[Test]` methods.

### 42. Which attribute marks a test method in NUnit?

NUnit uses `[Test]`.

```csharp
[Test]
public void Add_ShouldReturnSum()
{
}
```

### 43. Which attribute is used for parameterized tests in NUnit?

NUnit uses `[TestCase]`.

```csharp
[TestCase(2, 3, 5)]
[TestCase(5, 5, 10)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    var calculator = new Calculator();

    var result = calculator.Add(a, b);

    Assert.That(result, Is.EqualTo(expected));
}
```

### 44. What is TestCaseSource in NUnit?

`[TestCaseSource]` provides test data from a property, method, or field.

It is useful when test data is too large or complex for `[TestCase]`.

### 45. How do you write setup code in NUnit?

Use `[SetUp]`. It runs before each test.

```csharp
[SetUp]
public void Setup()
{
    _calculator = new Calculator();
}
```

### 46. How do you write cleanup code in NUnit?

Use `[TearDown]`. It runs after each test.

```csharp
[TearDown]
public void Cleanup()
{
}
```

### 47. How do you run setup code once before all tests in a class in NUnit?

Use `[OneTimeSetUp]`.

```csharp
[OneTimeSetUp]
public void OneTimeSetup()
{
}
```

### 48. How do you run cleanup code once after all tests in a class in NUnit?

Use `[OneTimeTearDown]`.

```csharp
[OneTimeTearDown]
public void OneTimeCleanup()
{
}
```

### 49. How do you skip a test in NUnit?

Use `[Ignore]`.

```csharp
[Test]
[Ignore("Temporary disabled")]
public void Test1()
{
}
```

### 50. How do you test exceptions in NUnit?

Use `Assert.Throws<T>()`.

```csharp
[Test]
public void Divide_ByZero_ThrowsException()
{
    Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
}
```

### 51. How do you test async code in NUnit?

Use `async Task`.

```csharp
[Test]
public async Task GetDataAsync_ReturnsData()
{
    var result = await service.GetDataAsync();

    Assert.That(result, Is.Not.Null);
}
```

### 52. What is Assert.That in NUnit?

`Assert.That` is NUnit's constraint-based assertion style. It makes assertions more readable.

```csharp
Assert.That(result, Is.EqualTo(10));
Assert.That(name, Does.Contain("John"));
Assert.That(list, Has.Count.EqualTo(3));
```

### 53. What is Category in NUnit?

`[Category]` is used to group tests.

```csharp
[Test]
[Category("Smoke")]
public void Test1()
{
}
```

You can run only selected categories from a test runner or CI pipeline.

### 54. Does NUnit support parallel test execution?

Yes. NUnit supports parallel execution using attributes like `[Parallelizable]`.

```csharp
[Parallelizable]
public class CalculatorTests
{
}
```

## MSTest Interview Questions

### 55. What is MSTest?

MSTest is Microsoft's unit testing framework for .NET. It is built by Microsoft and works well with Visual Studio and Azure DevOps.

### 56. Which NuGet packages are commonly used for MSTest?

Common packages are:

```text
MSTest.TestFramework
MSTest.TestAdapter
Microsoft.NET.Test.Sdk
```

### 57. Which attribute marks a test class in MSTest?

MSTest uses `[TestClass]`.

```csharp
[TestClass]
public class CalculatorTests
{
}
```

### 58. Which attribute marks a test method in MSTest?

MSTest uses `[TestMethod]`.

```csharp
[TestMethod]
public void Add_ShouldReturnSum()
{
}
```

### 59. Which attribute is used for parameterized tests in MSTest?

MSTest uses `[DataTestMethod]` with `[DataRow]`.

```csharp
[DataTestMethod]
[DataRow(2, 3, 5)]
[DataRow(5, 5, 10)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    var calculator = new Calculator();

    var result = calculator.Add(a, b);

    Assert.AreEqual(expected, result);
}
```

### 60. How do you write setup code in MSTest?

Use `[TestInitialize]`. It runs before each test.

```csharp
[TestInitialize]
public void Setup()
{
    _calculator = new Calculator();
}
```

### 61. How do you write cleanup code in MSTest?

Use `[TestCleanup]`. It runs after each test.

```csharp
[TestCleanup]
public void Cleanup()
{
}
```

### 62. How do you run setup code once before all tests in a class in MSTest?

Use `[ClassInitialize]`.

```csharp
[ClassInitialize]
public static void ClassSetup(TestContext context)
{
}
```

### 63. How do you run cleanup code once after all tests in a class in MSTest?

Use `[ClassCleanup]`.

```csharp
[ClassCleanup]
public static void ClassCleanup()
{
}
```

### 64. How do you run setup code once before all tests in an assembly in MSTest?

Use `[AssemblyInitialize]`.

```csharp
[AssemblyInitialize]
public static void AssemblySetup(TestContext context)
{
}
```

### 65. How do you run cleanup code once after all tests in an assembly in MSTest?

Use `[AssemblyCleanup]`.

```csharp
[AssemblyCleanup]
public static void AssemblyCleanup()
{
}
```

### 66. How do you skip a test in MSTest?

Use `[Ignore]`.

```csharp
[TestMethod]
[Ignore]
public void Test1()
{
}
```

### 67. How do you test exceptions in MSTest?

You can use `Assert.ThrowsException<T>()`.

```csharp
[TestMethod]
public void Divide_ByZero_ThrowsException()
{
    Assert.ThrowsException<DivideByZeroException>(
        () => calculator.Divide(10, 0));
}
```

### 68. How do you test async exceptions in MSTest?

Use `Assert.ThrowsExceptionAsync<T>()`.

```csharp
[TestMethod]
public async Task GetUserAsync_WhenUserNotFound_ThrowsException()
{
    await Assert.ThrowsExceptionAsync<InvalidOperationException>(
        () => service.GetUserAsync(1));
}
```

### 69. What is TestContext in MSTest?

`TestContext` provides information about the current test, such as test name, test result, deployment directory, and custom properties.

### 70. How do you write async tests in MSTest?

Use `async Task`.

```csharp
[TestMethod]
public async Task GetDataAsync_ReturnsData()
{
    var result = await service.GetDataAsync();

    Assert.IsNotNull(result);
}
```

## xUnit vs NUnit vs MSTest

### 71. What are the main test attributes in xUnit, NUnit, and MSTest?

| Purpose | xUnit | NUnit | MSTest |
|---|---|---|---|
| Test class | Not required | `[TestFixture]` | `[TestClass]` |
| Test method | `[Fact]` | `[Test]` | `[TestMethod]` |
| Parameterized test | `[Theory]` | `[TestCase]` | `[DataTestMethod]` |
| Inline data | `[InlineData]` | `[TestCase]` | `[DataRow]` |
| Setup before each test | Constructor | `[SetUp]` | `[TestInitialize]` |
| Cleanup after each test | `IDisposable` | `[TearDown]` | `[TestCleanup]` |
| Setup once per class | `IClassFixture<T>` | `[OneTimeSetUp]` | `[ClassInitialize]` |
| Cleanup once per class | Fixture cleanup | `[OneTimeTearDown]` | `[ClassCleanup]` |
| Skip test | `Skip` property | `[Ignore]` | `[Ignore]` |

### 72. Which framework requires a test class attribute?

MSTest requires `[TestClass]`.

NUnit usually uses `[TestFixture]`, but it can be optional in many cases.

xUnit does not require a test class attribute.

### 73. Which framework uses constructor for setup?

xUnit uses the constructor for setup before each test.

### 74. Which framework uses SetUp and TearDown?

NUnit uses `[SetUp]` and `[TearDown]`.

### 75. Which framework uses TestInitialize and TestCleanup?

MSTest uses `[TestInitialize]` and `[TestCleanup]`.

### 76. Which framework is most commonly used in modern .NET projects?

xUnit is very common in modern .NET projects, but NUnit and MSTest are also widely used.

### 77. Which framework is easiest for beginners?

MSTest is often easy for beginners because the attributes are descriptive.

Example:

```csharp
[TestClass]
[TestMethod]
```

xUnit is also simple after learning `[Fact]` and `[Theory]`.

### 78. Which framework has the richest attribute-based features?

NUnit has many rich attributes like `[TestCase]`, `[TestCaseSource]`, `[Category]`, `[Parallelizable]`, and `[Explicit]`.

### 79. Can all three frameworks be used with `dotnet test`?

Yes. xUnit, NUnit, and MSTest can all be run using:

```bash
dotnet test
```

### 80. Can we use more than one test framework in the same solution?

Yes, but it is usually better to use one framework per project or solution to keep the test style consistent.

## Mocking and Test Doubles

### 81. What is mocking?

Mocking means creating a fake version of a dependency so that the unit test can focus only on the class being tested.

Example: Mocking an email service instead of sending a real email.

### 82. Why do we use mocks?

We use mocks to:

- Avoid real database calls
- Avoid real API calls
- Avoid sending emails or messages
- Make tests faster
- Make tests independent
- Control dependency behavior

### 83. What is a dependency?

A dependency is another class or service that a class needs to do its work.

Example: `OrderService` may depend on `IOrderRepository` and `IEmailService`.

### 84. What is a fake?

A fake is a simple working implementation used only for testing.

Example: An in-memory repository instead of a real database repository.

### 85. What is a stub?

A stub provides fixed data to the class being tested.

Example: A stub user service always returns a test user.

### 86. What is a mock?

A mock is used to verify interactions, such as checking whether a method was called.

Example: Verify that `SendEmail()` was called once.

### 87. What is a spy?

A spy records information about calls made during the test. It can tell how many times a method was called and with what values.

### 88. What is the difference between mock and stub?

A stub provides data.

A mock verifies behavior or interaction.

### 89. Which mocking libraries are common in .NET?

Common mocking libraries are:

- Moq
- NSubstitute
- FakeItEasy

### 90. How do you mock a dependency using Moq?

```csharp
var repositoryMock = new Mock<IUserRepository>();

repositoryMock
    .Setup(x => x.GetById(1))
    .Returns(new User { Id = 1, Name = "John" });

var service = new UserService(repositoryMock.Object);
```

### 91. How do you verify a method was called using Moq?

```csharp
emailServiceMock.Verify(
    x => x.SendEmail("test@example.com"),
    Times.Once);
```

### 92. What should usually be mocked in unit tests?

Mock external dependencies like:

- Database repositories
- Email services
- File system
- External APIs
- Message queues
- Date/time providers

### 93. Should we mock the class being tested?

No. Usually, we mock the dependencies of the class being tested, not the class itself.

### 94. Can we mock static methods?

Most mocking libraries cannot easily mock static methods. A common solution is to wrap static calls behind an interface.

Example: Instead of calling `DateTime.Now` directly, use an `IDateTimeProvider`.

### 95. Can we mock private methods?

Usually, private methods should not be mocked directly. Test the public behavior that uses the private method.

### 96. Can we mock extension methods?

Most mocking libraries cannot directly mock extension methods. You can wrap the behavior in an interface if needed.

### 97. Why is dependency injection useful for unit testing?

Dependency injection makes it easy to pass mocks or fake objects into a class during testing.

### 98. What is over-mocking?

Over-mocking means using too many mocks or verifying too many internal details. It makes tests fragile and hard to maintain.

### 99. What is a fragile test?

A fragile test fails often because of small internal code changes, even when the application behavior is still correct.

### 100. How can we avoid fragile tests?

We can avoid fragile tests by:

- Testing behavior, not implementation details
- Avoiding unnecessary mocks
- Keeping tests simple
- Avoiding shared mutable state
- Using clear test data

## Assertions

### 101. What are common assertion methods?

Common assertions include:

- Equal
- NotEqual
- True
- False
- Null
- NotNull
- Contains
- DoesNotContain
- Throws
- IsType
- IsAssignableFrom

### 102. How do you check equality in xUnit?

```csharp
Assert.Equal(expected, actual);
```

### 103. How do you check equality in NUnit?

```csharp
Assert.That(actual, Is.EqualTo(expected));
```

### 104. How do you check equality in MSTest?

```csharp
Assert.AreEqual(expected, actual);
```

### 105. How do you check null in xUnit?

```csharp
Assert.Null(value);
Assert.NotNull(value);
```

### 106. How do you check null in NUnit?

```csharp
Assert.That(value, Is.Null);
Assert.That(value, Is.Not.Null);
```

### 107. How do you check null in MSTest?

```csharp
Assert.IsNull(value);
Assert.IsNotNull(value);
```

### 108. How do you check true or false in xUnit?

```csharp
Assert.True(result);
Assert.False(result);
```

### 109. How do you check true or false in NUnit?

```csharp
Assert.That(result, Is.True);
Assert.That(result, Is.False);
```

### 110. How do you check true or false in MSTest?

```csharp
Assert.IsTrue(result);
Assert.IsFalse(result);
```

### 111. How do you check collection count in NUnit?

```csharp
Assert.That(items, Has.Count.EqualTo(3));
```

### 112. How do you check collection count in xUnit?

```csharp
Assert.Equal(3, items.Count);
```

### 113. How do you check collection count in MSTest?

```csharp
Assert.AreEqual(3, items.Count);
```

### 114. What is FluentAssertions?

FluentAssertions is a popular assertion library that makes test assertions more readable.

```csharp
result.Should().Be(10);
name.Should().Contain("John");
items.Should().HaveCount(3);
```

### 115. Can FluentAssertions be used with xUnit, NUnit, and MSTest?

Yes. FluentAssertions can be used with all three frameworks.

## Data-Driven Tests

### 116. What is a data-driven test?

A data-driven test runs the same test logic with different input values.

### 117. Why are data-driven tests useful?

They reduce duplicate test code and make it easy to test many scenarios.

### 118. How do you write data-driven tests in xUnit?

Use `[Theory]` and `[InlineData]`.

```csharp
[Theory]
[InlineData(1, 2, 3)]
[InlineData(10, 20, 30)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.Equal(expected, a + b);
}
```

### 119. How do you write data-driven tests in NUnit?

Use `[TestCase]`.

```csharp
[TestCase(1, 2, 3)]
[TestCase(10, 20, 30)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.That(a + b, Is.EqualTo(expected));
}
```

### 120. How do you write data-driven tests in MSTest?

Use `[DataTestMethod]` and `[DataRow]`.

```csharp
[DataTestMethod]
[DataRow(1, 2, 3)]
[DataRow(10, 20, 30)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.AreEqual(expected, a + b);
}
```

### 121. When should we avoid data-driven tests?

Avoid them when each scenario needs very different setup or different assertions. In that case, separate tests may be clearer.

## Testing Async Code

### 122. How do you test async methods in .NET?

Use `async Task` test methods and `await` the method being tested.

```csharp
[Fact]
public async Task GetUserAsync_ReturnsUser()
{
    var user = await service.GetUserAsync(1);

    Assert.NotNull(user);
}
```

### 123. Why should we avoid async void in unit tests?

`async void` makes exceptions hard for the test runner to catch. Use `async Task` instead.

### 124. How do you mock async methods using Moq?

Use `ReturnsAsync`.

```csharp
repositoryMock
    .Setup(x => x.GetUserAsync(1))
    .ReturnsAsync(new User { Id = 1 });
```

### 125. How do you test an async method throws an exception?

Use the async exception assertion provided by the framework.

```csharp
await Assert.ThrowsAsync<InvalidOperationException>(
    () => service.GetUserAsync(1));
```

## Testing Web API Applications

### 126. How do you unit test a controller action?

Create the controller with mocked dependencies, call the action method, and assert the returned result.

```csharp
[Fact]
public void GetUser_WhenUserExists_ReturnsOk()
{
    var serviceMock = new Mock<IUserService>();
    serviceMock.Setup(x => x.GetUser(1)).Returns(new UserDto { Id = 1 });
    var controller = new UsersController(serviceMock.Object);

    var result = controller.GetUser(1);

    Assert.IsType<OkObjectResult>(result);
}
```

### 127. Should unit tests call real Web API endpoints?

No. Calling real endpoints is integration testing or end-to-end testing. Unit tests should call the controller or service method directly.

### 128. How do you test model validation in ASP.NET Core?

For unit tests, you can manually add model state errors.

```csharp
controller.ModelState.AddModelError("Name", "Name is required");
```

Then call the action and assert that it returns `BadRequest`.

### 129. How do you integration test ASP.NET Core Web API?

Use `WebApplicationFactory<TEntryPoint>` from `Microsoft.AspNetCore.Mvc.Testing`.

### 130. What is WebApplicationFactory?

`WebApplicationFactory` creates a test server for ASP.NET Core applications. It allows tests to send HTTP requests without hosting the app on a real server.

## Testing Entity Framework Core

### 131. Should unit tests use a real database?

Usually no. Unit tests should avoid real databases. Use mocks, fakes, or in-memory data where appropriate.

### 132. What is the EF Core InMemory provider?

The InMemory provider stores data in memory during tests. It is useful for simple tests but does not behave exactly like a real relational database.

### 133. What is better for realistic EF Core integration tests?

SQLite in-memory mode or a real test database is usually more realistic than EF Core InMemory provider.

### 134. Should DbContext be mocked?

Usually, it is better to test repository or service behavior using an in-memory database or SQLite instead of mocking `DbContext` directly.

### 135. Why can EF Core InMemory tests be misleading?

Because it does not enforce all relational database rules, SQL translation rules, constraints, and transactions like a real database.

## Best Practices

### 136. What should a unit test focus on?

A unit test should focus on one behavior or one rule.

### 137. Should unit tests depend on test execution order?

No. Each test should run independently.

### 138. Should unit tests use real time, like DateTime.Now?

Avoid direct use of real time in tests. Use an interface like `IDateTimeProvider` so the test can control the time.

### 139. Should unit tests use random values?

Avoid uncontrolled random values. If random data is needed, use fixed seeds or clear test data.

### 140. Should unit tests access files?

Usually no. File access makes tests slower and dependent on the environment. Wrap file access behind an interface or use temporary files carefully.

### 141. Should unit tests access external APIs?

No. External APIs make tests slow and unreliable. Mock the API client.

### 142. Should unit tests access a real database?

Usually no. Use integration tests for real database testing.

### 143. How many assertions should a unit test have?

There is no strict rule. A test can have multiple assertions if they verify the same behavior. Avoid testing many unrelated things in one test.

### 144. What is one assert per test?

It is a guideline that says each test should verify one main behavior. It does not always mean only one assertion line.

### 145. What is test isolation?

Test isolation means one test should not depend on another test or shared state.

### 146. What is shared state?

Shared state is data or objects used by multiple tests. If one test changes it, another test may fail unexpectedly.

### 147. How do you avoid shared state problems?

Create fresh objects for each test, avoid static mutable data, and clean up test data after each test.

### 148. What is a deterministic test?

A deterministic test gives the same result every time it runs with the same code.

### 149. What is a flaky test?

A flaky test sometimes passes and sometimes fails without code changes.

### 150. What causes flaky tests?

Common causes include:

- Timing issues
- Shared state
- Parallel execution
- Real network calls
- Real database dependency
- Random data
- Date/time dependency

### 151. How do you fix flaky tests?

Fix the root cause by removing timing dependency, isolating test data, mocking external services, and avoiding shared mutable state.

### 152. What is Arrange-Act-Assert benefit?

It makes tests easy to read and understand.

### 153. Should test code be clean?

Yes. Test code should be clean because it is part of the project and must be maintained.

### 154. Should private methods be unit tested?

Usually no. Test private methods through public methods. If a private method is very complex, it may be a sign that logic should move into a separate class.

### 155. Should we test simple properties?

Usually no, unless the property contains logic.

### 156. Should we test constructors?

Test constructors only when they contain important validation or initialization logic.

### 157. Should we test third-party libraries?

No. We usually test our own code, not third-party library behavior.

### 158. Should we write unit tests for every method?

Not always. Focus on business logic, edge cases, validation, calculations, and important workflows.

### 159. What is an edge case?

An edge case is an unusual or boundary condition.

Examples:

- Empty list
- Null value
- Maximum value
- Minimum value
- Zero
- Negative number

### 160. What is boundary testing?

Boundary testing checks values at the edges of allowed input.

Example: If age must be between 18 and 60, test 17, 18, 60, and 61.

## Advanced and Scenario-Based Questions

### 161. How do you test code that depends on DateTime.Now?

Create an abstraction.

```csharp
public interface IDateTimeProvider
{
    DateTime Now { get; }
}
```

In tests, pass a fake implementation that returns a fixed date.

### 162. How do you test code that sends emails?

Mock the email service and verify that the email method was called with correct values.

### 163. How do you test code that writes logs?

Usually, logs are not tested unless logging is a business requirement or needed for audit. If needed, mock `ILogger<T>` or use a test logger.

### 164. How do you test code that reads configuration?

Use `IOptions<T>` or an in-memory configuration object in tests.

### 165. How do you test validation logic?

Pass valid and invalid input values and assert the validation result or exception.

### 166. How do you test exception handling?

Make the dependency throw an exception, call the method, and assert that the method handles or rethrows it correctly.

### 167. How do you test retry logic?

Mock the dependency to fail first and succeed later. Then verify the number of calls and final result.

### 168. How do you test caching logic?

Mock the cache or use an in-memory cache. Check that data is retrieved from cache after the first call.

### 169. How do you test authorization logic?

For unit tests, test the policy or service logic separately. For full authorization behavior, use integration tests.

### 170. How do you test middleware in ASP.NET Core?

Create a `DefaultHttpContext`, build a fake `RequestDelegate`, call the middleware, and assert the response or context changes.

### 171. How do you test filters in ASP.NET Core?

Create the required filter context, execute the filter method, and assert the result.

### 172. How do you test background services?

Move the main logic into a separate service and unit test that service. For the hosted service loop, use integration tests or controlled cancellation tokens.

### 173. How do you test code that uses CancellationToken?

Pass a test `CancellationTokenSource`, cancel it when needed, and assert expected behavior.

### 174. How do you test protected methods?

Usually test through public behavior. If required, create a test subclass that exposes the protected method.

### 175. How do you test internal classes or methods?

Use `InternalsVisibleTo` to allow the test project to access internal members.

```csharp
[assembly: InternalsVisibleTo("MyProject.Tests")]
```

### 176. What is snapshot testing?

Snapshot testing compares current output with a previously saved expected output. It is useful for large object graphs or UI-like output, but snapshots should be reviewed carefully.

### 177. What is mutation testing?

Mutation testing changes small parts of the source code automatically and checks whether tests fail. It helps measure test quality.

### 178. What is TDD?

TDD means Test-Driven Development. In TDD, we write tests before writing the actual implementation.

### 179. What are the steps of TDD?

The TDD cycle is:

- Red: Write a failing test
- Green: Write minimum code to pass
- Refactor: Improve the code while keeping tests passing

### 180. What are the benefits of TDD?

Benefits include:

- Better design
- More testable code
- Early bug detection
- Clear requirements
- Safer refactoring

### 181. What are the disadvantages of TDD?

Possible disadvantages:

- Takes practice
- Can feel slower at first
- Poorly written tests can slow development
- Not every task is easy to drive with tests

### 182. What is BDD?

BDD means Behavior-Driven Development. It focuses on behavior using business-readable language.

Example:

```gherkin
Given a premium customer
When the customer places an order
Then a discount should be applied
```

### 183. What is SpecFlow?

SpecFlow is a BDD framework for .NET. It lets teams write tests in Gherkin syntax.

### 184. What is AutoFixture?

AutoFixture is a .NET library that creates test data automatically. It reduces manual object creation in tests.

### 185. What is the use of Bogus in unit testing?

Bogus creates fake test data such as names, emails, addresses, and phone numbers.

### 186. What is the difference between AutoFixture and Bogus?

AutoFixture creates objects automatically for tests.

Bogus creates realistic fake data.

### 187. What is the use of NSubstitute?

NSubstitute is a mocking library. It is often liked for its simple syntax.

```csharp
var service = Substitute.For<IEmailService>();
```

### 188. What is the use of FakeItEasy?

FakeItEasy is another mocking library for .NET. It creates fake objects and verifies calls.

### 189. What is the use of Moq?

Moq is a popular mocking library for .NET. It helps create mocks, set return values, and verify method calls.

### 190. What is the role of CI/CD in unit testing?

CI/CD runs tests automatically when code is pushed. This helps catch bugs before code is merged or deployed.

### 191. How do you run tests in Azure DevOps?

Use a pipeline task or command like:

```bash
dotnet test
```

### 192. How do you run tests in GitHub Actions?

Use a workflow step:

```yaml
- name: Run tests
  run: dotnet test
```

### 193. How do you collect code coverage in .NET?

You can use:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

You can also use tools like Coverlet and ReportGenerator.

### 194. What is Coverlet?

Coverlet is a popular code coverage tool for .NET.

### 195. What is ReportGenerator?

ReportGenerator converts coverage files into readable HTML reports.

### 196. What should be tested in service layer?

Test business rules, validations, calculations, decisions, and interactions with dependencies.

### 197. What should be tested in repository layer?

Repository tests are often integration tests because they involve database behavior.

### 198. What should be tested in controller layer?

Test whether the controller returns the correct response type, status code, and data for different service results.

### 199. What should be tested in domain layer?

Test domain rules, entity behavior, value objects, and business invariants.

### 200. What is the most important unit testing advice?

Write tests that are easy to read, fast to run, and focused on real business behavior. A useful test is better than a test that only increases coverage numbers.

## Quick Syntax Comparison

### Simple test

```csharp
// xUnit
[Fact]
public void Add_ShouldReturnSum()
{
    Assert.Equal(5, calculator.Add(2, 3));
}

// NUnit
[Test]
public void Add_ShouldReturnSum()
{
    Assert.That(calculator.Add(2, 3), Is.EqualTo(5));
}

// MSTest
[TestMethod]
public void Add_ShouldReturnSum()
{
    Assert.AreEqual(5, calculator.Add(2, 3));
}
```

### Parameterized test

```csharp
// xUnit
[Theory]
[InlineData(2, 3, 5)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.Equal(expected, calculator.Add(a, b));
}

// NUnit
[TestCase(2, 3, 5)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.That(calculator.Add(a, b), Is.EqualTo(expected));
}

// MSTest
[DataTestMethod]
[DataRow(2, 3, 5)]
public void Add_ShouldReturnSum(int a, int b, int expected)
{
    Assert.AreEqual(expected, calculator.Add(a, b));
}
```

### Setup before each test

```csharp
// xUnit
public CalculatorTests()
{
    calculator = new Calculator();
}

// NUnit
[SetUp]
public void Setup()
{
    calculator = new Calculator();
}

// MSTest
[TestInitialize]
public void Setup()
{
    calculator = new Calculator();
}
```

### Exception test

```csharp
// xUnit
Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));

// NUnit
Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));

// MSTest
Assert.ThrowsException<DivideByZeroException>(() => calculator.Divide(10, 0));
```

## Final Interview Tips

- Explain concepts in simple words.
- Give small examples.
- Mention Arrange, Act, Assert.
- Know the basic attributes of xUnit, NUnit, and MSTest.
- Understand mocking and dependency injection.
- Know how to test async methods and exceptions.
- Remember that unit tests should be fast, independent, and repeatable.
