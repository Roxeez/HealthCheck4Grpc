using Grpc.Net.Client;
using HealthCheck4Grpc.Contract;
using HealthCheck4Grpc.Extension;

var channel = GrpcChannel.ForAddress("http://localhost:6000");
var service = channel.CreateGrpcHealthCheckClient();

var response = await service.CheckAsync(new GrpcHealthCheckRequest());

Console.WriteLine(response.Status);