using System;
using MediatR;

namespace sp.auth.domain.account.events
{
    public class AuthenticateSuccessAccountDomainEvent : INotification
    {
        public long AccId { get; }
        
        public DateTime IssuedOn { get; }

        public AuthenticateSuccessAccountDomainEvent(long accId, DateTime issuedOn)
        {
            AccId = accId;
            IssuedOn = issuedOn;
        }
    }
}