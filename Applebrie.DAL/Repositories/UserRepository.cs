using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Applebrie.DAL.Interfaces;
using Applebrie.DAL.Options;
using Applebrie.DAL.Procedures.SQLStoredProcedures;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Applebrie.DAL.Repositories
{
    public class UserRepository : SQLRepository, IUserRepository
    {
        public UserRepository(IOptions<SQLDBContextOptions> options) : base(options)
        { }

        public async Task<User> CreateUser(RequestUserCreateModel user)
        {
            return await ExecuteJsonResultProcedureAsync<User>(StoredProcedures.CreateUser_SP, user);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await ExecuteJsonResultProcedureAsync<User>(StoredProcedures.GetUser_SP, new { id = id });
        }

        public async Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel param)
        {
            return await ExecuteJsonResultProcedureAsync<ResponseUserPagedResult>(StoredProcedures.GetUsers_SP, param);
        }

        public async Task RemoveUser(int id)
        {
            await ExecuteJsonQueryAsync(StoredProcedures.RemoveUser_SP, new { id = id });
        }

        public async Task<User> UpsertUser(RequestUserUpsertModel user)
        {
            return await ExecuteJsonResultProcedureAsync<User>(StoredProcedures.UpsertUser_SP, user);
        }
    }
}
