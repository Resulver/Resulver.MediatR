[![NuGet Package](https://img.shields.io/nuget/v/Resulver)](https://www.nuget.org/packages/Resulver/)
[![NuGet](https://img.shields.io/nuget/dt/Resulver)](https://www.nuget.org/packages/Resulver)

### Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
- [Result Message](#result-message)
- [Result Errors](#result-errors)
- [Real-World Example](#real-world-example)
- [Best Practices](#best-practices)

---

### Overview

`Resulver` is a lightweight .NET library designed to simplify result handling in your application. It provides a clean,
structured way to return success results, messages, and errors without relying on exceptions for control flow. This
approach enhances code readability and improves performance by avoiding exception overhead.

---

### Installation

To install `Resulver`, use the following command in your terminal:

```bash
dotnet add package Resulver
```

Ensure that you have the required .NET SDK installed.

---

### Usage

Here’s a simple example of how to use `Resulver` to return and handle a result:

```csharp
public Result<int> Sum(int a, int b)
{
    // Perform the addition
    var sum = a + b;

    // Return the result wrapped in a Result object
    return new Result<int>(sum);
}

public void Writer()
{
    var result = Sum(3, 5);

    // Access and display the result content
    Console.WriteLine(result.Content);
}
```

---

### Result Message

You can include custom messages in your results to provide additional context:

```csharp
public Result<User> AddUser(User user)
{
    // Implementation logic

    return new Result<User>(user, message: "User Created");
}

public void Writer()
{
    var user = new User();

    var result = AddUser(user);

    // Display the custom message
    Console.WriteLine(result.Message);
}
```

---

### Result Errors

Handle errors elegantly by using `ResultError` classes:

```csharp
public class UserNotFoundError : ResultError
{
    public UserNotFoundError() : base("User not found") { }
}

public class UserIdIsNotValidError : ResultError
{
    public UserIdIsNotValidError() : base("User ID is not valid") { }
}

public Result RemoveUser(int userId)
{
    // Implementation logic

    // Return a single error
    return new UserNotFoundError();

    // Or return multiple errors
    return new Result(new UserNotFoundError(), new UserIdIsNotValidError());
}

public void Writer()
{
    var result = RemoveUser(1);

    // Check if the result is a failure
    if (result.IsFailure)
    {
        foreach (var error in result.Errors)
        {
            // Display each error message
            Console.WriteLine(error.Message);
        }
    }
}
```

---

### Real-World Example

Handling user authentication:

```csharp
public Result<User> Authenticate(string username, string password)
{
    var user = _userRepository.Find(username, password);
    
    if (user == null) return new UserNotFoundError();

    return new Result<User>(user, message: "Authentication successful");
}
```

---

### Best Practices

Here are some tips for working effectively with `Resulver`:

1. **Always Check `IsSuccess` or `IsFailure`:**
   Ensure you validate the state of the result before accessing its content or errors.

2. **Use Meaningful Messages and Errors:**
   Provide clear and specific messages to make debugging easier.

3. **Avoid Mixing Exceptions and Results:**
   Stick to the `Result` pattern for predictable and consistent flow control.

4. **Combine Multiple Errors When Needed:**
   Utilize the flexibility of `Result` to handle and return multiple errors when necessary.

By following these practices, you can make the most out of `Resulver` and write cleaner, more maintainable code.

