using Applebrie.Common.Enums;
using Applebrie.Common.Models;
using Applebrie.Common.Models.RequestModels;
using Applebrie.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applebrie.DAL.Helpers.Extensions
{
    public static class ModelsExtensions
    {
        public static UserDTO RequestUserCreateModelToDTO(this RequestUserCreateModel model)
        {
            return new UserDTO() { Name = model.Name, Sex = ((Byte)model.Sex), Status = ((Byte)model.Status), Created = DateTime.Now, Updated = DateTime.Now, Role = ((Byte)model.Role) };
        }

        public static UserDTO RequestUserUpsertModelToDTO(this RequestUserUpsertModel model)
        {
            return new UserDTO() { Name = model.Name, Sex = ((Byte)model.Sex), Status = ((Byte)model.Status), Updated = DateTime.Now, Role = ((Byte)model.Role), Id = model.Id.HasValue ? model.Id.Value : -1 };
        }
        
        public static User DTOToModel(this UserDTO model)
        {
            return new User() { Name = model.Name, Sex = (Sex)model.Sex, Status = (Status)model.Status, Updated = DateTime.Now, Role = (Roles)model.Role, Id = model.Id, Created = model.Created };
        }
    }
}
