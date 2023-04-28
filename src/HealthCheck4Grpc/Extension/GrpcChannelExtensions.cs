using Grpc.Net.Client;
using HealthCheck4Grpc.Contract;

namespace HealthCheck4Grpc.Extension;

public static class GrpcChannelExtensions
{
    public static GrpcHealthCheck.GrpcHealthCheckClient CreateGrpcHealthCheckClient(this GrpcChannel channel)
    {
        return new GrpcHealthCheck.GrpcHealthCheckClient(channel);
    }
}