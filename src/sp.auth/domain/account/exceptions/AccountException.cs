using System;

namespace sp.auth.domain.account.exceptions
{
    public class AccountException : Exception
    {
        public AccountException(string msg) : base(msg)
        {
            
        }
    }
}