using System;
using MediatR;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommand : IRequest<Unit>
    {
        public string Alias { get; set;  }
        public string Password { get; set; }
    }
}