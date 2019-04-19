using System;

namespace sp.auth.domain.account.exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg) : base(msg)
        {
            
        }
    }
}