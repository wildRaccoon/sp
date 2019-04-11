using MediatR;
using System;

namespace sp.auth.domain.account.events
{
    public class RenewAccountSessionSuccessDomainEvent : INotification
    {
        public long AccId { get; }
        public DateTime IssuedOn { get; }
        public string RenewToken { get; }
        public DateTime SessionExpired { get; }
        public string Role { get; }

        public RenewAccountSessionSuccessDomainEvent(long accId, DateTime issuedOn, string renewToken, DateTime sessionExpired, string role)
        {
            AccId = accId;
            IssuedOn = issuedOn;
            RenewToken = renewToken;
            SessionExpired = sessionExpired;
            Role = role;
        }
    }
}
