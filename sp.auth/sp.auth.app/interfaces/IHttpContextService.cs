using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sp.auth.app.interfaces
{
    public interface IHttpContextService
    {
        Task UpdateContext(long accountId, string role, DateTime expired, DateTime issuedOn);
        long GetAccountIdFromContext();
        ClaimsPrincipal Validate(bool validateLifetime);
    }
}