using HealthCheck4Grpc.Contract;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck4Grpc.AspNetCore.Extension;

internal static class HealthStatusExtensions
{
    public static GrpcHealthStatus ToGrpcHealthStatus(this HealthStatus status)
    {
        return status switch
        {
            HealthStatus.Healthy => GrpcHealthStatus.Healthy,
            HealthStatus.Degraded => GrpcHealthStatus.Degraded,
            HealthStatus.Unhealthy => GrpcHealthStatus.Unhealthy,
            _ => GrpcHealthStatus.Healthy
        };
    }
}