using MediatR;
using sp.auth.app.infra.config;
using sp.auth.app.infra.ef;
using sp.auth.app.interfaces;
using sp.auth.domain.account.events;
using sp.auth.domain.account.exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.account.commands.common;

namespace sp.auth.app.account.commands.renew
{
    public class RenewAccountSessionCommandHandler : IRequestHandler<RenewAccountSessionCommand, RenewTokenModel>
    {
        private readonly ITokenService _token;
        private readonly AuthenticateConfig _authConfig;
        private readonly IHttpContextService _httpContextService;
        private AuthDataContext _repo;
        private IMediator _mediator;

        public RenewAccountSessionCommandHandler(IMediator mediator, AuthDataContext repo, ITokenService token, AuthenticateConfig authConfig, IHttpContextService httpContextService)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _authConfig = authConfig ?? throw new ArgumentNullException(nameof(authConfig));
            _httpContextService = httpContextService ?? throw new ArgumentNullException(nameof(httpContextService));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RenewTokenModel> Handle(RenewAccountSessionCommand request, CancellationToken cancellationToken)
        {
            var accIdFromContext = _httpContextService.GetAccountIdFromContext();

            if (accIdFromContext != request.AccountId)
            {
                throw new RenewAccountSessionException(request.AccountId, "operation not allowed");
            }

            var session = _repo.AccountSessions.SingleOrDefault(s => s.AccountId == request.AccountId);

            if(session == null)
            {
                throw new RenewAccountSessionException(request.AccountId, "session not found");
            }

            if (session.RenewToken != request.RenewToken)
            {
                throw new RenewAccountSessionException(request.AccountId, $"renew token not equal current:{session.RenewToken} requested:{request.RenewToken}");
            }

            if (session.RenewExpired < DateTime.Now)
            {
                _repo.Remove(session);
                await _repo.SaveChangesAsync(cancellationToken);
                throw new RenewAccountSessionException(request.AccountId, $"renew token expired");
            }

            session.RenewExpired = DateTime.Now.AddSeconds(_authConfig.renewExpiredInSec);
            session.SessionExpired = DateTime.Now.AddSeconds(_authConfig.sessionExpiredInSec);
            session.RenewToken = _token.IssueToken();

            var acc = await _repo.Accounts.SingleAsync(x => x.Id == accIdFromContext, cancellationToken);

            _repo.AccountSessions.Update(session);

            await _repo.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new RenewAccountSessionSuccessDomainEvent(request.AccountId, session.IssuedOn, session.RenewToken, session.SessionExpired, acc.Role), cancellationToken);

            return RenewTokenModel.Create(session);
        }
    }
}
