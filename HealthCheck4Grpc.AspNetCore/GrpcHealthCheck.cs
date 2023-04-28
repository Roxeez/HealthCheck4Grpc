using Grpc.Core;
using HealthCheck4Grpc.Contract;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck4Grpc.AspNetCore;

public class GrpcHealthCheck : Contract.GrpcHealthCheck.GrpcHealthCheckBase
{
    private readonly HealthCheckService service;

    public GrpcHealthCheck(HealthCheckService service)
    {
        this.service = service;
    }

    public override async Task<GrpcHealthCheckResponse> Check(GrpcHealthCheckRequest request, ServerCallContext context)
    {
        var report = await service.CheckHealthAsync();

        var status = report.Status switch
        {
            HealthStatus.Healthy => GrpcHealthStatus.Healthy,
            HealthStatus.Degraded => GrpcHealthStatus.Degraded,
            HealthStatus.Unhealthy => GrpcHealthStatus.Unhealthy,
            _ => GrpcHealthStatus.Healthy
        };

        return new GrpcHealthCheckResponse
        {
            Status = status
        };
    }
}