using System;
using MediatR;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommand : IRequest<Unit>
    {
        public string Alias { get; }
        public string Password { get; }

        public AuthenticateAccountCommand(string alias, string pass)
        {
            Alias = alias;
            Password = pass;
        }
    }
}