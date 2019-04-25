namespace sp.auth.domain.account.exceptions
{
    public class NotFoundException : AccountException
    {
        public NotFoundException(string msg) : base(msg)
        {
        }
    }
}