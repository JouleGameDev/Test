using Applebrie.Common.Enums;

namespace Applebrie.Common.Models.RequestModels
{
    public class RequestUserCreateModel
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Status Status { get; set; }
        public Roles Role { get; set; }
    }
}
