using Grpc.Core;
using HealthCheck4Grpc.Contract;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;
using HealthStatusRpc = HealthCheck4Grpc.Contract.HealthStatus;

namespace HealthCheck4Grpc;

public class GrpcHealthCheck : Contract.GrpcHealthCheck.GrpcHealthCheckBase
{
    private readonly HealthCheckService service;

    public GrpcHealthCheck(HealthCheckService service)
    {
        this.service = service;
    }

    public override async Task<HealthCheckResponse> Check(HealthCheckRequest request, ServerCallContext context)
    {
        var report = await service.CheckHealthAsync();

        var status = report.Status switch
        {
            HealthStatus.Healthy => HealthStatusRpc.Healthy,
            HealthStatus.Degraded => HealthStatusRpc.Degraded,
            HealthStatus.Unhealthy => HealthStatusRpc.Unhealthy,
            _ => HealthStatusRpc.Healthy
        };

        return new HealthCheckResponse
        {
            Status = status
        };
    }
}