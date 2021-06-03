using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace FitnessFunctions.HealthChecks
{
    public class CustomHealthCheck: IHealthCheck
    {
        // constructor
        public CustomHealthCheck()
        {
            
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //Implement you logic here    
            var healthy = true;
            if (healthy)
                return Task.FromResult(HealthCheckResult.Healthy());
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }

    }
}
