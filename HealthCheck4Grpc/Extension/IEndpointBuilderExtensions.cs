using Grpc.Net.Client;
using HealthCheck4Grpc.Contract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck4Grpc.Extension;

public static class EndpointBuilderExtensions
{
    public static void AddGrpcHealthCheck(this IHealthChecksBuilder builder, string name, string url)
    {
        var channel = GrpcChannel.ForAddress(url);
        var service = channel.CreateGrpcHealthCheckClient();

        builder.AddAsyncCheck(name, async () =>
        {
            GrpcHealthCheckResponse response;
            try
            {
                response = await service.CheckAsync(new GrpcHealthCheckRequest());
            }
            catch (Exception)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy);
            }

            var status = response.Status switch
            {
                GrpcHealthStatus.Healthy => HealthStatus.Healthy,
                GrpcHealthStatus.Degraded => HealthStatus.Degraded,
                GrpcHealthStatus.Unhealthy => HealthStatus.Unhealthy,
                _ => HealthStatus.Unhealthy
            };

            return new HealthCheckResult(status);
        });
    }
}