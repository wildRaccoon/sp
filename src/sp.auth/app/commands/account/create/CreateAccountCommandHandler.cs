using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using sp.auth.app.interfaces;
using sp.auth.domain.account;
using sp.auth.domain.account.events;
using sp.auth.app.ef;

namespace sp.auth.app.commands.account.create
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, bool>
    {
        AuthDataContext _repo {get;}
        IHashService _hash {get;}
        IMediator _mediator {get;}

        public CreateAccountCommandHandler(IMediator mediator, AuthDataContext repo,IHashService hash)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            //query items with the same Email or Alias
            var chk = _repo.Accounts.Count(x => x.Alias == request.Alias || x.Email == request.Email);

            if (chk > 0)
            {
                return false;
            }

            var acc = new Account(){
                Alias = request.Alias,
                Email = request.Email,
                IsLocked = false,
                CreatedOn = DateTime.Now,
                PasswordHash = _hash.Encode(request.Password)
            };

            _repo.Add(acc);

            await _repo.SaveChangesAsync(cancellationToken);

            await _mediator.Publish(new CreatedAccountDomainEvent(acc.Id, acc.CreatedOn));

            return true;
        }
    }
}