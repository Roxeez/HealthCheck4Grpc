using Grpc.Core;
using HealthCheck4Grpc.AspNetCore.Extension;
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
        
        var services = new List<GrpcServiceStatus>();
        foreach (var (name, serviceReport) in report.Entries)
        {
            services.Add(new GrpcServiceStatus
            {
                Name = name,
                Status = serviceReport.Status.ToGrpcHealthStatus()
            });
        }

        return new GrpcHealthCheckResponse
        {
            Status = report.Status.ToGrpcHealthStatus(),
            Services =
            {
                services
            }
        };
    }
}