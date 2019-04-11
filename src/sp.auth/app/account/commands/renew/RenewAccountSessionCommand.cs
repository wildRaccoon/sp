using MediatR;

namespace sp.auth.app.account.commands.renew
{
    public class RenewAccountSessionCommand : IRequest<Unit>
    {
        public long AccountId { get; set; }
        public string RenewToken { get; set; }
    }
}
