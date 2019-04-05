using sp.auth.domain.common;

namespace sp.auth.app.interfaces
{
    public interface ISingleQueryOfT<T>
        where T: DomainEntity<string>
    {
         T Query(T t);
    }
}