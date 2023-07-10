using Inventory.Helpers;
using Inventory.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IReportViewService
    {
        Task<PagingList<StockTakingReport>> ReportWithMovementation(string filter, int pageindex = 1, string sort = "ItemName");
        Task<PageList<StockTakingReport>> FinalReport(PageParams pageParams);
    }
}
