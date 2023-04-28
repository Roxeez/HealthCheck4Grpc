using HealthCheck4Grpc.AspNetCore.Extension;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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