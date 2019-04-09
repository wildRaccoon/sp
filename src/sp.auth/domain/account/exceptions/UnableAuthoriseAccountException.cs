using System;

namespace sp.auth.domain.account.exceptions
{
    public class UnableAuthoriseAccountException : Exception
    {
        public UnableAuthoriseAccountException(string alias) : base($"Unable to authorise account: {alias}")
        {
        }
    }
}