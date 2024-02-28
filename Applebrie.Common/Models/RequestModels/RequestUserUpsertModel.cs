using Applebrie.Common.Enums;

namespace Applebrie.Common.Models.RequestModels
{
    public class RequestUserUpsertModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Status Status { get; set; }
        public Roles Role { get; set; }
    }
}
