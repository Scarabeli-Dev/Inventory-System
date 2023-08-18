using Inventory.Helpers;
using Inventory.ViewModels;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IReportViewService
    {
        PagingList<FinalReportViewModel> ReportWithMovementation(string filter, int pageSize = 10, int pageindex = 1, string sortExpression = "ItemName", int warehouseId = 0, int stockSituation = -2, int addressingSituation = -2);

        void ExportToCSV();
    }
}
