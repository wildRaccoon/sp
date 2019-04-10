using FluentValidation;

namespace sp.auth.app.account.commands.authenticate
{
    public class AuthenticateAccountCommandValidator : AbstractValidator<AuthenticateAccountCommand>
    {
        public AuthenticateAccountCommandValidator()
        {
            RuleFor(x => x.Alias)
                .NotNull()
                .NotEmpty()
                .MaximumLength(50);
            
            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(50);
        }
    }
}