using System;
using MediatR;

namespace sp.auth.domain.account.events
{
    public class CreatedAccountDomainEvent : INotification
    {
        public string Id {get; private set;}

        public DateTime CreatedOn { get; private set;} 

        public CreatedAccountDomainEvent(string id, DateTime createdOn)
        {
            Id = id;
            CreatedOn = createdOn;
        }
    }
}