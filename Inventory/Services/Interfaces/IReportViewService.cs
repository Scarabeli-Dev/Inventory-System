using Inventory.Helpers;
using Inventory.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IReportViewService
    {
        Task<PagingList<StockTakingReport>> ReportWithMovementation(string filter, int pageindex = 1, string sortExpression = "ItemName", int warehouseId = 0, int stockSituation = -1, int addressingSituation = -1);
        PageList<StockTakingReport> FinalReport(PageParams pageParams);
    }
}
