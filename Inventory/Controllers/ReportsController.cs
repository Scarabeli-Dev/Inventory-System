using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Controllers
{
    [Route("Relatorios")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportViewService _reportViewService;

        public ReportsController(IReportViewService reportViewService)
        {
            _reportViewService = reportViewService;
        }

        //[Route("Contagem-com-movimentacao")]
        //public async Task<IActionResult> ReportWithMovementation(PageParams pageParams)
        //{
        //    return View(await _reportViewService.FinalReport(pageParams));
        //}

        [Route("Contagem-com-movimentacao")]
        public async Task<IActionResult> ReportWithMovementation(string filter, int pageindex = 1, string sort = "ItemName")
        {
            return View(await _reportViewService.ReportWithMovementation(filter, pageindex, sort));
        }
    }
}
