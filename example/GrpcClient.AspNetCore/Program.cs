using HealthCheck4Grpc.Extension;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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