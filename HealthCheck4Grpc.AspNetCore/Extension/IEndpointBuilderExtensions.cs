namespace HealthCheck4Grpc.Extension;

public static class EndpointBuilderExtensions
{
    public static void MapGrpcHealthCheck(this IEndpointRouteBuilder builder)
    {
        builder.MapGrpcService<GrpcHealthCheck>();
    }
}