using System.Threading.Tasks;

namespace Applebrie.BLL.Cache.Interface
{
    public interface ICache<T, R>
    {
        public Task<bool> TryGetObject(T key, out R result);

        public Task AddObject(T key, R result);

        public Task ClearChache();
        
    }
}
