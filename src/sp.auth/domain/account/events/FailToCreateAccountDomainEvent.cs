using MediatR;

namespace sp.auth.domain.account.events
{
    public class FailToCreateAccountDomainEvent : INotification
    {
        public string Reason {get; private set;}

        public FailToCreateAccountDomainEvent(string reason)
        {
            Reason = reason;
        }
    }
}