using FluentValidation;
using System;
using System.Linq;

namespace sp.auth.app.commands.account.create
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.Alias)
            .MaximumLength(50)
            .NotEmpty();
            
            RuleFor(x => x.Password)
            .MaximumLength(50)
            .NotEmpty();

            RuleFor(x => x.Password)
            .MinimumLength(8)
            .MaximumLength(50)
            .Must(val => { 
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