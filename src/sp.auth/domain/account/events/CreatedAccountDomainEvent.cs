using System;
using MediatR;

namespace sp.auth.domain.account.events
{
    public class CreatedAccountDomainEvent : INotification
    {
        public long Id {get; private set;}

        public DateTime CreatedOn { get; private set;} 

        public CreatedAccountDomainEvent(long id, DateTime createdOn)
        {
            Id = id;
            CreatedOn = createdOn;
        }
    }
}