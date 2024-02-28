using Applebrie.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Applebrie.DAL.DTO
{
    [Table("Users", Schema = "users")]
    public class UserDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public string Name { get; set; }
        public Byte Sex { get; set; }
        public Byte Status { get; set; }
        public Byte Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
