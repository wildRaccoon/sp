using System;

namespace sp.auth.domain.account
{
    public class Account
    {
        public long Id { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsLocked { get; set; }
        public DateTime CreatedOn {get; set;}
    }
}