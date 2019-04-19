using System;

namespace sp.auth.domain.account.exceptions
{
    public class UnableCreateAccountException : AccountException
    {
        public UnableCreateAccountException(string alias,string email) : base($"Unable to create account: {alias}|{email}")
        {
        }
    }
}