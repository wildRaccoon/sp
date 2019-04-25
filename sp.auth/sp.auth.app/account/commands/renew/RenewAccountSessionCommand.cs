using MediatR;
using sp.auth.app.account.commands.common;

namespace sp.auth.app.account.commands.renew
{
    public class RenewAccountSessionCommand : IRequest<RenewTokenModel>
    {
        public long AccountId { get; set; }
        public string RenewToken { get; set; }
    }
}
