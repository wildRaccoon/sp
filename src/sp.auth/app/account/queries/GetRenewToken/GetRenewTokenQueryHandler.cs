using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.infra.ef;
using sp.auth.domain.account.exceptions;

namespace sp.auth.app.account.queries.GetRenewToken
{
    public class GetRenewTokenQueryHandler : IRequestHandler<GetRenewTokenQuery, RenewTokenModel>
    {
        private AuthDataContext _repo {get;}
        
        public GetRenewTokenQueryHandler(AuthDataContext repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        
        public async Task<RenewTokenModel> Handle(GetRenewTokenQuery request, CancellationToken cancellationToken)
        {
            var session = await _repo.AccountSessions.SingleOrDefaultAsync(x => x.AccountId == request.AccountId);

            if (session == null)
            {
                throw  new NotFoundException($"Query renew token for AccountId:{request.AccountId}");
            }

            return RenewTokenModel.Create(session);
        }
    }
}