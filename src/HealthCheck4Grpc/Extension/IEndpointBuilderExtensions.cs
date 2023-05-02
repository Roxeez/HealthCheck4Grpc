using System.Reflection;
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
                _ => HealthStatus.Healthy
            };

            /*
             * This is a trick to avoid cyclic dependencies
             * If the service in not healthy we try to check if this service is in the reasons
             * If this service is in the reasons and it is the only reason we ignore it and return healthy
             */
            if (status != HealthStatus.Healthy)
            {
                var currentService = response.Services.FirstOrDefault(x => x.Name == name);
                if (currentService is not null && response.Services.Count == 1)
                {
                    status = HealthStatus.Healthy;
                }
            }

            return new HealthCheckResult(status);
        });
    }
}