# Examples.Web.Infrastructure.GrpcClient

A library that provides gRPC client infrastructure for ASP.NET Core applications.
It auto-generates client stubs from `.proto` files via `Grpc.Tools` and registers them in the DI container with built-in interceptors for logging and unhandled exception handling.

## Features

- **DI registration** — one-call setup via `AddGrpcClients()`
- **Configuration-driven** — bind `GrpcClientOptions` from `appsettings.json`
- **Interceptors** — `LoggingInterceptor` and `UnhandledExceptionInterceptor` applied automatically
- **Service abstraction** — `IGreeterService` wraps the generated `Greeter.GreeterClient`

## Getting Started

### 1. Add configuration

```json
// appsettings.json
"GrpcClient": {
  "Clients": {
    "Greeter": {
      "BaseAddress": "https://localhost:7271",
      "TimeoutSeconds": 5,
      "AllowUntrustedCertificate": true
    }
  }
}
```

### 2. Register services

```csharp
builder.Services.AddGrpcClients(builder.Configuration);
```

### 3. Use the service

```csharp
public class MyController(IGreeterService greeter) : ControllerBase
{
    [HttpGet]
    public async Task<string> Get(string name)
        => await greeter.GetMessageAsync(name);
}
```
