using FluentValidation;
namespace sp.auth.app.account.queries.GetRenewToken
{
    public class GetRenewTokenQueryValidator : AbstractValidator<GetRenewTokenQuery>
    {
        public GetRenewTokenQueryValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().NotNull();
        }
    }
}