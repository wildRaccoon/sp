using System;
using sp.auth.domain.common;

namespace sp.auth.domain.account
{
    public class Account : DomainEntity<string>
    {
        public string Alias { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedOn {get; set;}
    }
}