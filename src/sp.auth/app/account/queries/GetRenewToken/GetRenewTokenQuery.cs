using MediatR;

namespace sp.auth.app.account.queries.GetRenewToken
{
    public class GetRenewTokenQuery : IRequest<RenewTokenModel>
    {
        public long AccountId { get; set; }
    }
}