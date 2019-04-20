namespace sp.auth.app.interfaces
{
    public interface ISecurityService
    {
        void UpdateContext(long AccountId, string role);
    }
}