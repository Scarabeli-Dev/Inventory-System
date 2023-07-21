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
    [Authorize(Roles = "Admin, Relatorio")]
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

            ViewData["StockSituation"] = new SelectList(GetEnumSelectListWithAll<StockSituation>("Todos"), "Value", "Text", stockSituation);
            ViewData["AddressingSituation"] = new SelectList(GetEnumSelectListWithAll<AddressingSituation>("Todos"), "Value", "Text", addressingSituation);



            return View(await _reportViewService.ReportWithMovementation(filter, pageindex, sortExpression, warehouseId, stockSituation, addressingSituation));
        }

        private List<SelectListItem> GetEnumSelectListWithAll<T>(string displayNameAll) where T : Enum
        {
            var selectList = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = displayNameAll } };

            selectList.AddRange(Enum.GetValues(typeof(T))
                .OfType<T>()
                .Select(enumValue => new SelectListItem
                {
                    Value = ((int)(object)enumValue).ToString(),
                    Text = enumValue.GetDisplayName()
                }));

            return selectList;
        }


    }
}