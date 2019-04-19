using System;
using System.Collections.Generic;
using System.Text;

namespace sp.auth.domain.account.exceptions
{
    public class RenewAccountSessionException : UnauthorizedException
    {
        public RenewAccountSessionException(long accId, string reason) : base($"Unable to renew session account: {accId} {reason}")
        {
        }
    }
}
