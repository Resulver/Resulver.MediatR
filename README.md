[![NuGet Package](https://img.shields.io/nuget/v/Resulver.MediatR)](https://www.nuget.org/packages/Resulver.MediatR/)
# Resulver.MediatR NuGet Package

## Table of Contents
- [Overview](#overview)
- [Installation](#installation)
- [Usage](#usage)
   - [Implementing Request Handlers](#implementing-request-handlers)
   - [Example Usage](#example-usage)
- [Result and Error Handling](#result-and-error-handling)
- [Real-World Example](#real-world-example)
- [Best Practices](#best-practices)

## Overview

`Resulver.MediatR` is a lightweight extension for `MediatR` designed to seamlessly integrate `MediatR` with the `Resulver` package. It provides structured request handling with `Result` objects, ensuring a clean and predictable flow without relying on exceptions.

> **Before using `Resulver.MediatR`, it is recommended to read the [`Resulver`](https://www.nuget.org/packages/Resulver) documentation to understand how `Result` works and how to use it effectively.**

## Installation

To install `Resulver.MediatR`, use the following command:

```bash
dotnet add package Resulver.MediatR
```

Make sure that both `MediatR` and `Resulver` are installed in your project.

## Usage

### Implementing Request Handlers

`Resulver.MediatR` provides two interfaces for handling requests:

- `IResultBaseRequestHandler<TRequest, TResultContent>`: For requests that return a specific value within a `Result<TResultContent>`.
- `IResultBaseRequestHandler<TRequest>`: For requests that only return a `Result` without content.

#### Example 1: Handler with a Specific Return Value

```csharp
public class SumHandler : IResultBaseRequestHandler<SumRequest, int>
{
    public Task<Result<int>> Handle(SumRequest request, CancellationToken cancellationToken)
    {
        var sum = request.A + request.B;
        return Task.FromResult(new Result<int>(sum));
    }
}
```

#### Example 2: Handler Without a Specific Return Value

```csharp
public class RemoveUserHandler : IResultBaseRequestHandler<RemoveUserRequest>
{
    public Task<Result> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0)
        {
            return Task.FromResult(new Result(new UserNotFoundError()));
        }

        return Task.FromResult(new Result());
    }
}
```

### Example Usage

```csharp
public class SumRequest : IResultBaseRequest<int>
{
    public int A { get; }
    public int B { get; }

    public SumRequest(int a, int b)
    {
        A = a;
        B = b;
    }
}

public class MyController
{
    private readonly IMediator _mediator;

    public MyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> CalculateSum(int a, int b)
    {
        var request = new SumRequest(a, b);
        var result = await _mediator.Send(request);

        if (result.IsFailure)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Content);
    }
}
```

## Result and Error Handling

With `Resulver.MediatR`, you can handle errors using `ResultError` objects:

```csharp
public class UserNotFoundError : ResultError
{
    public UserNotFoundError() : base("User not found") { }
}

public class InvalidUserIdError : ResultError
{
    public InvalidUserIdError() : base("Invalid User ID") { }
}
```

### Handling Multiple Errors

```csharp
public Task<Result> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
{
    var errors = new List<ResultError>();

    if (request.UserId <= 0)
    {
        errors.Add(new InvalidUserIdError());
    }

    if (!errors.Any())
    {
        //....
        
        return Task.FromResult(new Result("User removed"));
    }

    return Task.FromResult(new Result(errors));
}
```

## Real-World Example

```csharp
public class AuthenticateRequest : IResultBaseRequest<User>
{
    public string Username { get; }
    public string Password { get; }

    public AuthenticateRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

public class AuthenticateHandler : IResultBaseRequestHandler<AuthenticateRequest, User>
{
    private readonly IUserRepository _userRepository;

    public AuthenticateHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<Result<User>> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
    {
        var user = _userRepository.Find(request.Username, request.Password);

        if (user == null)
        {
            return Task.FromResult(new UserNotFoundError());
        }

        return Task.FromResult(new Result<User>(user,"Authentication successful"));
    }
}
```

## Best Practices

- **Always check `IsSuccess` or `IsFailure`** before accessing the result content or errors.
- **Use meaningful messages and errors** to provide clear debugging information.
- **Avoid mixing exceptions with `Result`**, keeping control flow predictable.
- **Handle multiple errors when necessary**, leveraging the flexibility of `Result`.
- **Keep handlers simple**, ensuring they focus on a single responsibility.

By following these guidelines, you can effectively use `Resulver.MediatR` to write cleaner, more maintainable code.

