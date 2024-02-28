using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using System.Threading.Tasks;

namespace Applebrie.BLL.Interfaces
{
    public interface IUserEngine
    {
        public Task<User> GetUserAsync(int id);
        public Task<User> CreateUser(RequestUserCreateModel model);
        public Task DeleteUser(int id);
        public Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel model);
        public Task<User> UpdateUser(RequestUserUpsertModel model);

    }
}
