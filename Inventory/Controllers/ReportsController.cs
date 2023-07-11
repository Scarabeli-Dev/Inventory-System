using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IReportViewService _reportViewService;
        private readonly IWarehouseService _warehouseService;

        public ReportsController(IReportViewService reportViewService, IWarehouseService warehouseService)
        {
            _reportViewService = reportViewService;
            _warehouseService = warehouseService;
        }

        //[Route("Contagem-com-movimentacao")]
        //public async Task<IActionResult> ReportWithMovementation(PageParams pageParams)
        //{
        //    return View(await _reportViewService.FinalReport(pageParams));
        //}

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "ItemName", int warehouseId = 0, int stockSituation = -1, int addressingSituation = -1)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Todos", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            return View(await _reportViewService.ReportWithMovementation(filter, pageindex, sort, warehouseId, stockSituation, addressingSituation));
        }

    }
}
