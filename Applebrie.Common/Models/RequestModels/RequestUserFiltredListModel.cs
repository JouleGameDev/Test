
namespace Applebrie.Common.Models.RequestModels
{
    public class RequestUserFiltredListModel
    {
        public string Search { get; set; }
        public string OrderBy { get; set; }
        public bool? Desc { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
