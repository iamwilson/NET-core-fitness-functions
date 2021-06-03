using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
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

        private Random rnd = new Random();

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //Implement you logic here 

            int checkDigit = rnd.Next(1, 300);

            var status = checkDigit < 200 ? true : false;

            if (status)
                return Task.FromResult(HealthCheckResult.Healthy($"Healthy - Check Digit Is ({checkDigit})"));
            return Task.FromResult(HealthCheckResult.Unhealthy($"UnHealthy - Check Digit Is ({checkDigit})"));
        }

    }
}
