using System.Threading;
using System.Threading.Tasks;
using MediatR;
using sp.auth.app.interfaces;
using sp.auth.domain.account.events;

namespace sp.auth.app.account.events
{
    public class AuthenticateAccountNotificationHandler : INotificationHandler<AuthenticateSuccessAccountDomainEvent>
    {
        private readonly IHttpContextService _httpContextService;

        public AuthenticateAccountNotificationHandler(IHttpContextService httpContextService)
        {
            _httpContextService = httpContextService;
        }

        public async Task Handle(AuthenticateSuccessAccountDomainEvent notification, CancellationToken cancellationToken)
        {
            await _httpContextService.UpdateContext(notification.AccId,notification.Role,notification.SessionExpired,notification.IssuedOn);
        }
    }
}