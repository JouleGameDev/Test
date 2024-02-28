using Applebrie.BLL.Cache.Interface;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Applebrie.BLL.Cache.CacheRepository
{
    public class UserDictionaryCache : ICache<RequestUserFiltredListModel, ResponseUserPagedResult>
    {
        private ConcurrentDictionary<RequestUserFiltredListModel, ResponseUserPagedResult> _cache;
        public UserDictionaryCache()
        {
            _cache = new ConcurrentDictionary<RequestUserFiltredListModel, ResponseUserPagedResult>();
        }

        public Task AddObject(RequestUserFiltredListModel key, ResponseUserPagedResult result)
        {
            _cache.TryAdd(key, result);
            return Task.CompletedTask;
        }

        public Task ClearChache()
        {
            _cache = new ConcurrentDictionary<RequestUserFiltredListModel, ResponseUserPagedResult>();
            return Task.CompletedTask;
        }

        public Task<bool> TryGetObject(RequestUserFiltredListModel key, out ResponseUserPagedResult result)
        {
            result = new ResponseUserPagedResult();
            if (_cache.ContainsKey(key))
            {
                _cache.TryGetValue(key, out result);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
