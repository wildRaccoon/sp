namespace sp.auth.domain.common
{
    public abstract class DomainEntity<T>
        where T: class
    {
        public T Id {get; set;}
    }
}