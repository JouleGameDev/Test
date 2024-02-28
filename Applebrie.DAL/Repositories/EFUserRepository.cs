using Applebrie.Common.Enums;
using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.Common.Models.ResponseModels;
using Applebrie.DAL.DTO;
using Applebrie.DAL.EF;
using Applebrie.DAL.Helpers.Extensions;
using Applebrie.DAL.Interfaces;
using Applebrie.DAL.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Applebrie.DAL.Repositories
{
    public class EFUserRepository : EFRepository, IUserRepository
    {
        private readonly SQLDBContextOptions _options;
        public EFUserRepository(IOptions<SQLDBContextOptions> options) : base(options.Value)
        {
            _options = options.Value;
        }


        public async Task<User> CreateUser(RequestUserCreateModel user)
        {
            var model = user.RequestUserCreateModelToDTO();

            using (EFRepository db = new EFRepository(_options))
            {
                model = db.Users.Add(model);
                await db.SaveChangesAsync();
            }

            return model.DTOToModel();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var result = new User();
            using (EFRepository db = new EFRepository(_options))
            {
                result = db.Users.Single(x => x.Id == id).DTOToModel();
            }

            return result;
        }

        public async Task<ResponseUserPagedResult> GetUsers(RequestUserFiltredListModel param)
        {
            var usersFiltredList = new List<User>();

            using (EFRepository db = new EFRepository(_options))
            {
                IQueryable<UserDTO> list = db.Users.OrderBy(u => u.Id);

                if (!string.IsNullOrWhiteSpace(param.Search))
                {
                    string like = $"%{param.Search}%";

                    list = list.Where(u => u.Name.Contains(like));
                }

                if (!string.IsNullOrWhiteSpace(param.OrderBy))
                {
                    switch (param.OrderBy.ToLower().Trim())
                    {
                        case "sex":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Sex) : list.OrderBy(o => o.Sex);
                            break;
                        case "name":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Name) : list.OrderBy(o => o.Name);
                            break;
                        case "role":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Role) : list.OrderBy(o => o.Role);
                            break;
                        case "status":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Status) : list.OrderBy(o => o.Status);
                            break;
                        case "id":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Id) : list.OrderBy(o => o.Id);
                            break;
                        case "created":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Created) : list.OrderBy(o => o.Created);
                            break;
                        case "updated":
                            list = (param.Desc.HasValue && param.Desc.Value) ? list.OrderByDescending(o => o.Updated) : list.OrderBy(o => o.Updated);
                            break;

                        default:
                            break;
                    }
                }

                list = list.Skip((param.Page.Value - 1) * param.PageSize.Value).Take(param.PageSize.Value);

                usersFiltredList = list
                     .Select(model => new User() { Name = model.Name, Sex = (Sex)model.Sex, Status = (Status)model.Status, Updated = DateTime.Now, Role = (Roles)model.Role, Id = model.Id, Created = model.Created })
                     .AsEnumerable().ToList();
            }

            return new ResponseUserPagedResult() { Count = usersFiltredList.Count, Users = usersFiltredList };
        }

        public async Task RemoveUser(int id)
        {
            using (EFRepository db = new EFRepository(_options))
            {
                var user = await db.Users.FindAsync(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<User> UpsertUser(RequestUserUpsertModel user)
        {
            var model = user.RequestUserUpsertModelToDTO();

            using (EFRepository db = new EFRepository(_options))
            {
                if (model.Id == -1)
                {
                    model.Created = DateTime.Now;
                    model = db.Users.Add(model);
                }
                else
                {
                    var userToUpdate = db.Users.FirstOrDefault(u => u.Id == model.Id);
                    if (userToUpdate != null)
                    {
                        userToUpdate.Name = model.Name;
                        userToUpdate.Sex = model.Sex;
                        userToUpdate.Status = model.Status;
                        userToUpdate.Role = model.Role;
                    }
                    else
                    {
                        model.Created = DateTime.Now;
                        model = db.Users.Add(model);
                    }
                }

                await db.SaveChangesAsync();
            }

            return model.DTOToModel();
        }
    }
}
