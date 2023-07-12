using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

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

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sortExpression = "ItemName", int warehouseId = 0, int stockSituation = -1, int addressingSituation = -1)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Todos", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            var stockSituationList = new List<SelectListItem>{
    new SelectListItem
    {
        Value = "-1",
        Text = "Todos"
    }
};

            stockSituationList.AddRange(Enum.GetValues(typeof(StockSituation))
                .OfType<StockSituation>()
                .Select(enumValue => new SelectListItem
                {
                    Value = ((int)enumValue).ToString(),
                    Text = enumValue.GetDisplayName()
                }));

            ViewData["StockSituation"] = new SelectList(stockSituationList, "Value", "Text", stockSituation);


            var addressingSituationList = new List<SelectListItem>{
    new SelectListItem
    {
        Value = "-1",
        Text = "Todos"
    }
};
            addressingSituationList.AddRange(Enum.GetValues(typeof(AddressingSituation))
                .OfType<AddressingSituation>()
                .Select(enumValue => new SelectListItem
                {
                    Value = ((int)enumValue).ToString(),
                    Text = enumValue.GetDisplayName()
                }));

            ViewData["AddressingSituation"] = new SelectList(addressingSituationList, "Value", "Text", addressingSituation);


            return View(await _reportViewService.ReportWithMovementation(filter, pageindex, sortExpression, warehouseId, stockSituation, addressingSituation));
        }

    }
}