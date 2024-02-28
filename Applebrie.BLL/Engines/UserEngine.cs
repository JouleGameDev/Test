using Applebrie.BLL.Cache.Interface;
using Applebrie.BLL.Helpers;
using Applebrie.BLL.Interfaces;
using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Applebrie.DAL.Interfaces;
using System.Data.Entity.Core.Mapping;
using System.Threading.Tasks;

namespace Applebrie.BLL.Engines
{
    public class UserEngine : IUserEngine
    {
        private IUserRepository _userRepository;
        private ICache<RequestUserFiltredListModel, ResponseUserPagedResult> _userCache;

        public UserEngine(IUserRepository userRepository,
                          ICache<RequestUserFiltredListModel, ResponseUserPagedResult> userCache)
        {
            _userRepository = userRepository;
            _userCache = userCache;
        }

        public async Task<User> CreateUser(RequestUserCreateModel model)
        {
            await _userCache.ClearChache();
            return await _userRepository.CreateUser(model);
        }

        public async Task DeleteUser(int id)
        {
            await _userCache.ClearChache();
            await _userRepository.RemoveUser(id);
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel model)
        {
            model.Validate();
            
            var result = new ResponseUserPagedResult();
            var isCached = await _userCache.TryGetObject(model, out result);

            if (!isCached)
            {
                result = await _userRepository.GetUsers(model);
                _ = _userCache.AddObject(model, result);

                return result;
            }

            return result;
        }

        public async Task<User> UpdateUser(RequestUserUpsertModel model)
        {
            await _userCache.ClearChache();
            return await _userRepository.UpsertUser(model);
        }
    }
}
