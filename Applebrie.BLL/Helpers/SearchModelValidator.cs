using Applebrie.Common.Models.RequestModels;

namespace Applebrie.BLL.Helpers
{
    public static class SearchModelValidator
    {
        public static RequestUserFiltredListModel Validate(this RequestUserFiltredListModel model)
        {
            if (!model.Page.HasValue || model.Page.Value <= 0)
                model.Page = 1;
            if (!model.PageSize.HasValue || model.PageSize <= 0)
                model.PageSize = 25;

            if (string.IsNullOrWhiteSpace(model.Search))
                model.Search = string.Empty;
            if (string.IsNullOrWhiteSpace(model.OrderBy))
                model.OrderBy = string.Empty;

            if (!model.Desc.HasValue)
                model.Desc = false;

            return model;
        }
    }
}
