using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace Applebrie.DAL.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetByIdAsync(int id);
        public Task<User> CreateUser(RequestUserCreateModel user);
        public Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel param);
        public Task<User> UpsertUser(RequestUserUpsertModel user);
        public Task RemoveUser(int id);
    }
}
