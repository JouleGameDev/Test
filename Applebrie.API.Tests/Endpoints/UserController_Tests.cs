using Applebrie.API.Controllers;
using Applebrie.API.Tests.Mock;
using Applebrie.BLL.Cache.CacheRepository;
using Applebrie.BLL.Engines;
using Applebrie.Common.Models.RequestModels;
using NUnit.Framework;

namespace Applebrie.API.Tests.Endpoints
{
    public class UserController_Tests
    {
        [Test]
        public void Test_CreatingUser()
        {
            // Arrange
            var cache = new UserDictionaryCache();
            var engine = new UserEngine(new SQLUserRepository_Mock(), cache);
            var controller = new UserController(engine);
            var userCreateModel = new RequestUserCreateModel() { 
            Name = "name",
            Sex = Common.Enums.Sex.Female,
            Role = Common.Enums.Roles.Admin,
            Status = Common.Enums.Status.Active
            };

            // Act
            var result = controller.CreateUser(userCreateModel).Result;

            // Assert
            Assert.NotNull(result.Created);
        }
        
        [Test]
        public void Test_GettingUsers()
        {
            // Arrange
            var cache = new UserDictionaryCache();
            var engine = new UserEngine(new SQLUserRepository_Mock(), cache);
            var controller = new UserController(engine);
            var userCreateModel = new RequestUserFiltredListModel();

            // Act
            var result = controller.GetUsers(userCreateModel).Result;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 2);
        }

        [Test]
        public void Test_UpdaitingUser()
        {
            // Arrange
            var cache = new UserDictionaryCache();
            var engine = new UserEngine(new SQLUserRepository_Mock(), cache);
            var controller = new UserController(engine);
            var userCreateModel = new RequestUserUpsertModel()
            {
                Id = 1,
                Name = "name",
                Sex = Common.Enums.Sex.Female,
                Role = Common.Enums.Roles.Admin,
                Status = Common.Enums.Status.Active
            };

            // Act
            var result = controller.UpdateUser(userCreateModel).Result;

            // Assert
            Assert.NotNull(result.Updated);
            Assert.That(userCreateModel.Name, Is.EqualTo(result.Name).NoClip);
        }

        [Test]
        public void Test_DeletingUser()
        {
            // Arrange
            var cache = new UserDictionaryCache();
            var engine = new UserEngine(new SQLUserRepository_Mock(), cache);
            var controller = new UserController(engine);
            
            // Act
            Assert.DoesNotThrow(()=>controller.DeleteUser(1));

            // Assert
        }

    }
}
