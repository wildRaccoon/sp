using System;
using MediatR;

namespace sp.auth.app.account.commands.create
{
    public class CreateAccountCommand : IRequest<bool>
    { 
        public string Alias { get; }
        public string Email { get; }
        public string Password { get; }

        public CreateAccountCommand(string alias, string email, string pass)
        {
            Alias = alias;
            Email = email;
            Password = pass;
        }
    }
}