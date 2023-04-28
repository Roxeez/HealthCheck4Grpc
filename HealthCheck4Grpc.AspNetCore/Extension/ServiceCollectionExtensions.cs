using Microsoft.Extensions.DependencyInjection;

namespace HealthCheck4Grpc.Extension;

public static class ServiceCollectionExtensions
{
    public static IHealthChecksBuilder AddGrpcHealthCheck(this IServiceCollection services)
    {
        return services.AddHealthChecks();
    }
}