using FluentValidation;

namespace sp.auth.app.account.commands.renew
{
    public class RenewAccountSessionCommandValidator : AbstractValidator<RenewAccountSessionCommand>
    {
        public RenewAccountSessionCommandValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty();

            RuleFor(x => x.RenewToken)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(100);
        }
    }
}
