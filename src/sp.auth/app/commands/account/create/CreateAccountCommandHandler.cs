using System;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using sp.auth.app.interfaces;
using sp.auth.domain.account;
using sp.auth.domain.account.events;
using sp.auth.app.interfaces.queries;

namespace sp.auth.app.commands.account.create
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, bool>
    {
        IRepository<Account> _repo {get;}
        IHashService _hash {get;}
        IMediator _mediator {get;}
        IUniqueAccountQuery _uniqueAccountQuery {get;}

        public CreateAccountCommandHandler(IMediator mediator,IRepository<Account> repo,IHashService hash, IUniqueAccountQuery uniqueAccountQuery)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hash = hash ?? throw new ArgumentNullException(nameof(hash));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _uniqueAccountQuery = uniqueAccountQuery ?? throw new ArgumentNullException(nameof(uniqueAccountQuery));
        }
        public async Task<bool> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var acc = new Account(){
                Alias = request.Alias,
                Email = request.Email,
                IsLocked = false,
                CreatedOn = DateTime.Now,
                PasswordHash = _hash.Encode(request.Password)
            };

            //query items with the same Email or Alias
            var chk = _uniqueAccountQuery.Query(acc);

            if(chk != null)
            {
                var changeProp = chk.Email == request.Email ? "email" : "alias";
                await _mediator.Publish(new FailToCreateAccountDomainEvent($"Please use other {changeProp}."));
                return false;
            }

            _repo.Add(acc);

            var created = await _repo.SaveAsync();

            if(created)
            {
                await _mediator.Publish(new CreatedAccountDomainEvent(acc.Id,acc.CreatedOn));
            }

            return created;
        }
    }
}