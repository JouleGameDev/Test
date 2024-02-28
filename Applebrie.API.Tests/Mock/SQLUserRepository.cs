using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Applebrie.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Applebrie.API.Tests.Mock
{
    public class SQLUserRepository_Mock : IUserRepository
    {
        public Task<User> CreateUser(RequestUserCreateModel user)
        {
            return Task.FromResult(new User()
            {
                Id = 999,
                Name = user.Name,
                Sex = user.Sex,
                Status = user.Status,
                Role = user.Role,
                Created = DateTime.Now,
                Updated = DateTime.Now
            });
        }

        public Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel param)
        {
            var result = new ResponseUserPagedResult();
            result.Count = 2;
            result.Users = new List<User>() { new User()
            {
                Id = 999,
                Name = "1",
                Sex = Common.Enums.Sex.Female,
                Status = Common.Enums.Status.Active,
                Role = Common.Enums.Roles.Admin,
                Created = DateTime.Now,
                Updated = DateTime.Now
            },
            new User()
            {
                Id = 9919,
                Name = "12",
                Sex = Common.Enums.Sex.Male,
                Status = Common.Enums.Status.Blocked,
                Role = Common.Enums.Roles.Other,
                Created = DateTime.Now,
                Updated = DateTime.Now
            }};

            return Task.FromResult(result);
        }

        public Task RemoveUser(int id)
        {
            return Task.CompletedTask;
        }

        public Task<User> UpsertUser(RequestUserUpsertModel user)
        {
            return Task.FromResult(new User()
            {
                Id = user.Id.Value,
                Name = user.Name,
                Sex = user.Sex,
                Status = user.Status,
                Role = user.Role,
                Updated = DateTime.Now                
            });
        }
    }
}
