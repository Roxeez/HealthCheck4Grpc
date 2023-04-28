# HealthCheck4Grpc
Library made to easily add healthcheck to your gRpc services (with more flexibility than the standard gRpc healhcheck library)

### Client without ASP.NET Core
```csharp
var channel = GrpcChannel.ForAddress("http://localhost:6000");
var service = channel.CreateGrpcHealthCheckClient();

var response = await service.CheckAsync(new GrpcHealthCheckRequest());

Console.WriteLine(response.Status);
```

### Client
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(x =>
{
    x.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
});

builder.Services.AddGrpc();
builder.Services.AddHealthChecks().AddGrpcHealthCheck("MyService", "http://localhost:6000");

var app = builder.Build();

app.MapHealthChecks("/health");

app.Run();
```

### Server
```csharp
var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseKestrel(x =>
{
    x.ListenAnyIP(5000, o => o.Protocols = HttpProtocols.Http1);
    x.ListenAnyIP(6000, o => o.Protocols = HttpProtocols.Http2);
});

builder.Services.AddGrpc();
builder.Services.AddHealthChecks().AddCheck("Example", () => HealthCheckResult.Degraded());

var app = builder.Build();

app.MapGrpcHealthCheck();
app.MapHealthChecks("/health");

app.Run();
```
