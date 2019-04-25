using System;
using sp.auth.app.interfaces;

namespace sp.auth.persistence.services.token
{
    public class TokenService : ITokenService
    {
        public string IssueToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}