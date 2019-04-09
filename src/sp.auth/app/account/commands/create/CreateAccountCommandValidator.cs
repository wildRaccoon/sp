using FluentValidation;
using System;
using System.Linq;

namespace sp.auth.app.account.commands.create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
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

            RuleFor(x => x.Password)
            .Must(val => {
                if (string.IsNullOrEmpty(val))
                {
                    return false;
                }
                
                var check = val.ToCharArray().ToList();
                
                var containUpperCase = check.Exists(c => char.IsLetter(c) && char.IsUpper(c));
                var containLowerCase = check.Exists(c => char.IsLetter(c) && char.IsLower(c));
                var containDigit = check.Exists(c => char.IsDigit(c));
                var containOther = check.Exists(c => !char.IsLetterOrDigit(c));

                return containUpperCase && containLowerCase && containDigit && containOther;
            });
        }
    }
}