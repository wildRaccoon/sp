namespace sp.auth.domain.account.exceptions
{
    public class UnableAuthoriseAccountException : UnauthorizedException
    {
        public UnableAuthoriseAccountException(string alias) : base($"Unable to authorise account: {alias}")
        {
        }
    }
}