using System;
using sp.auth.domain.common;

namespace sp.auth.domain.account
{
    public class AccountSession : DomainEntity<string>
    {
        public DateTime RenewExpired {get; set;}
        public string RenewToken {get; set;}
        public string SessionToken {get; set;}
        public DateTime SessionExpired {get; set;}
    }
}