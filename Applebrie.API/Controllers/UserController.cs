using Applebrie.BLL.Interfaces;
using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Applebrie.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IUserEngine _userEngine;
        public UserController(IUserEngine userEngine)
        {
            _userEngine = userEngine;
        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<User> CreateUser(RequestUserCreateModel model)
        {
            try
            {
                return await _userEngine.CreateUser(model);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<User> GetUser(int id)
        {
            try
            {
                return await _userEngine.GetUserAsync(id);

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [Route("get_users")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel model)
        {
            try
            {
                return await _userEngine.GetUsers(model);

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task DeleteUser(int id)
        {
            try
            {
                await _userEngine.DeleteUser(id);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [Route("update")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<User> UpdateUser(RequestUserUpsertModel model)
        {
            try
            {
                return await _userEngine.UpdateUser(model);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
