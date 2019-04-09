using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.infra.ef;
using sp.auth.app.interfaces;
using sp.auth.domain.account;
using sp.auth.domain.account.events;
using sp.auth.domain.account.exceptions;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommandHandler: IRequestHandler<AuthenticateAccountCommand, Unit>
    {
        private readonly ITokenService _token;
        private AuthDataContext _repo {get;}
        private IHashService _hash {get;}
        private IMediator _mediator {get;}
        
        public AuthenticateAccountCommandHandler(IMediator mediator, AuthDataContext repo,IHashService hash,ITokenService token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<Unit> Handle(AuthenticateAccountCommand request, CancellationToken cancellationToken)
        {
            var acc = await _repo.Accounts.SingleOrDefaultAsync(x => x.Alias == request.Alias, cancellationToken: cancellationToken);
            var hashOfPass = _hash.Encode(request.Password);

            if (acc?.PasswordHash != hashOfPass)
            {
                throw new UnableAuthoriseAccountException($"Unable to authorise account:{request.Alias}");
            }

            var accSession = new AccountSession()
            {
                AccountId = acc.Id,
                RenewToken = _token.IssueToken(),
                SessionToken = _token.IssueToken(),
                IssuedOn = DateTime.Now
            };

            _repo.AccountSessions.Add(accSession);
            await _repo.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new AuthenticateSuccessAccountDomainEvent(acc.Id, accSession.IssuedOn), cancellationToken);

            return Unit.Value;
        }
    }
}