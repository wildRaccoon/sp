using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using sp.auth.app.infra.ef;

namespace sp.auth.service.health
{
    public class DbConnectionHealthCheck : IHealthCheck
    {
        private readonly AuthDataContext _repo;

        public DbConnectionHealthCheck(AuthDataContext repo)
        {
            _repo = repo;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await _repo.Accounts.CountAsync();
                return HealthCheckResult.Healthy("Connection ok.");
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Connection failed. Error: {ex.Message}");
            }
        }
    }
}