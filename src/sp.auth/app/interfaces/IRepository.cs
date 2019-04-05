using System.Threading.Tasks;
using sp.auth.domain.common;
using System.Linq;

namespace sp.auth.app.interfaces
{
    public interface IRepository<T>
        where T: DomainEntity<string>
    {
        void Add(T v);
        void Delete(T v);
        void Update(T v);
        Task<bool>  SaveAsync();
    }
}