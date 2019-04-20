using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.account.commands.common;
using sp.auth.app.infra.config;
using sp.auth.app.infra.ef;
using sp.auth.app.interfaces;
using sp.auth.domain.account;
using sp.auth.domain.account.events;
using sp.auth.domain.account.exceptions;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommandHandler: IRequestHandler<AuthenticateAccountCommand, RenewTokenModel>
    {
        private readonly ITokenService _token;
        
        private readonly AuthenticateConfig _authConfig;
        private AuthDataContext _repo {get;}
        private IHashService _hash {get;}
        private IMediator _mediator {get;}
        
        public AuthenticateAccountCommandHandler(IMediator mediator, AuthDataContext repo,IHashService hash,ITokenService token, AuthenticateConfig authConfig)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _authConfig = authConfig ?? throw new ArgumentNullException(nameof(authConfig));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<RenewTokenModel> Handle(AuthenticateAccountCommand request, CancellationToken cancellationToken)
        {
            var acc = await _repo.Accounts.SingleOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
            var hashOfPass = _hash.Encode(request.Password);

            if (acc?.PasswordHash != hashOfPass)
            {
                throw new UnableAuthoriseAccountException($"Unable to authorise account:{request.Alias}");
            }

            var oldsession = await _repo.AccountSessions.SingleOrDefaultAsync(x => x.AccountId == acc.Id);

            if (oldsession != null)
            {
                _repo.Remove(oldsession);
            }

            var dtNow = DateTime.Now;

            var session = new AccountSession()
            {
                AccountId = acc.Id,
                RenewToken = _token.IssueToken(),
                IssuedOn = dtNow,
                SessionExpired = dtNow.AddSeconds(_authConfig.sessionExpiredInSec),
                RenewExpired = dtNow.AddSeconds(_authConfig.renewExpiredInSec)
            };

            _repo.AccountSessions.Add(session);
            await _repo.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AuthenticateSuccessAccountDomainEvent(acc.Id, session.IssuedOn, session.RenewToken, session.SessionExpired, Roles.Account), cancellationToken);

            return RenewTokenModel.Create(session);
        }
    }
}