using Applebrie.DAL.DTO;
using Applebrie.DAL.Interfaces;
using Applebrie.DAL.Options;
using System.Data.Entity;

namespace Applebrie.DAL.EF
{
    public class EFRepository : DbContext, IRepository
    {
        public EFRepository(SQLDBContextOptions options) : base(options.DbConnectionString)
        {
            Database.SetInitializer<EFRepository>(null);
        }

        public virtual DbSet<UserDTO> Users { get; set; }

    }
}
