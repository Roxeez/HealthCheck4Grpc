using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace HealthCheck4Grpc.AspNetCore.Extension;

public static class EndpointBuilderExtensions
{
    public static void MapGrpcHealthCheck(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<GrpcHealthCheck>();
    }
}