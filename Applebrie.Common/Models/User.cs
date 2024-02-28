using Applebrie.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Applebrie.Common.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Status Status { get; set; }
        public Roles Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
