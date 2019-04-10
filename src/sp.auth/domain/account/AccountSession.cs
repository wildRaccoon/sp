using System;

namespace sp.auth.domain.account
{
    public class AccountSession
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string RenewToken {get; set;}
        public DateTime IssuedOn {get; set;}
        public DateTime SessionExpired {get; set;}
        public DateTime RenewExpired {get; set;}
    }
}