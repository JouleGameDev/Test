using System.Collections.Generic;

namespace Applebrie.Common.Models.ResponseModels
{
    public class ResponseUserPagedResult
    {
        public int Count { get; set; } = 0;
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}
