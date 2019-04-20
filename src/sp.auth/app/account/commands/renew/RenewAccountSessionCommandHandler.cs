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
using sp.auth.app.account.commands.common;

namespace sp.auth.app.account.commands.renew
{
    public class RenewAccountSessionCommandHandler : IRequestHandler<RenewAccountSessionCommand, RenewTokenModel>
    {
        private readonly ITokenService _token;

        private readonly AuthenticateConfig _authConfig;
        private AuthDataContext _repo { get; }
        private IHashService _hash { get; }
        private IMediator _mediator { get; }

        public RenewAccountSessionCommandHandler(IMediator mediator, AuthDataContext repo, IHashService hash, ITokenService token, AuthenticateConfig authConfig)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _authConfig = authConfig ?? throw new ArgumentNullException(nameof(authConfig));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RenewTokenModel> Handle(RenewAccountSessionCommand request, CancellationToken cancellationToken)
        {
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
                throw new RenewAccountSessionException(request.AccountId, $"renew token expired");
            }

            session.RenewExpired = DateTime.Now.AddSeconds(_authConfig.renewExpiredInSec);
            session.SessionExpired = DateTime.Now.AddSeconds(_authConfig.sessionExpiredInSec);
            session.RenewToken = _token.IssueToken();

            _repo.AccountSessions.Update(session);

            await _repo.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new RenewAccountSessionSuccessDomainEvent(request.AccountId, session.IssuedOn, session.RenewToken, session.SessionExpired, Roles.Account), cancellationToken);

            return RenewTokenModel.Create(session);
        }
    }
}
