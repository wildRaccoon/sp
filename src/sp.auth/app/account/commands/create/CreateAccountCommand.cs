using System;
using MediatR;

namespace sp.auth.app.account.commands.create
{
    public class CreateAccountCommand : IRequest<Unit>
    { 
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}