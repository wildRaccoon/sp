using MediatR;
using sp.auth.app.account.commands.common;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommand : IRequest<RenewTokenModel>
    {
        public string Alias { get; set;  }
        public string Password { get; set; }
    }
}