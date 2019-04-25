using System.Threading;
using System.Threading.Tasks;
using MediatR;
using sp.auth.app.interfaces;
using sp.auth.domain.account.events;

namespace sp.auth.app.account.events
{
    public class RenewAccountSessionNotificationHandler : INotificationHandler<RenewAccountSessionSuccessDomainEvent>
    {
        private readonly IHttpContextService _httpContextService;

        public RenewAccountSessionNotificationHandler(IHttpContextService httpContextService)
        {
            _httpContextService = httpContextService;
        }

        public async Task Handle(RenewAccountSessionSuccessDomainEvent notification, CancellationToken cancellationToken)
        {
            await _httpContextService.UpdateContext(notification.AccId,notification.Role,notification.SessionExpired,notification.IssuedOn);
        }
    }
}