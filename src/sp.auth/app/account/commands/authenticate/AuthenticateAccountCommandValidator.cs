using FluentValidation;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommandValidator : AbstractValidator<AuthenticateAccountCommand>
    {
        public AuthenticateAccountCommandValidator()
        {
            RuleFor(x => x.Alias)
                .MaximumLength(50)
                .NotEmpty();
            
            RuleFor(x => x.Password)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}