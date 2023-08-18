using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReflectionIT.Mvc.Paging;
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

        public async Task<IActionResult> Index(string filter, int pageSize = 10, int pageindex = 1, string sortExpression = "ItemName", int warehouseId = 0, int stockSituation = -2, int addressingSituation = -2)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Todos", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            var additionalOptions = new Dictionary<int, string>
            {
                { -1, "Apenas itens contados" }
            };

            ViewData["StockSituation"] = new SelectList(GetEnumSelectListWithAll<StockSituation>("Todos", additionalOptions), "Value", "Text", stockSituation);
            ViewData["AddressingSituation"] = new SelectList(GetEnumSelectListWithAll<AddressingSituation>("Todos"), "Value", "Text", addressingSituation);

            List<SelectListItem> pageSizeOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "15", Text = "15" },
                    new SelectListItem { Value = "20", Text = "20" },
                    new SelectListItem { Value = "25", Text = "25" },
                    new SelectListItem { Value = "30", Text = "30" },
                    new SelectListItem { Value = "35", Text = "35" },
                    new SelectListItem { Value = "40", Text = "40" },
                    new SelectListItem { Value = "45", Text = "45" },
                    new SelectListItem { Value = "50", Text = "50" }
                };

            ViewBag.PageSizeSelect = pageSize.ToString();
            ViewBag.PageSizeOptions = pageSizeOptions;

            return View(_reportViewService.ReportWithMovementation(filter, pageSize, pageindex, sortExpression, warehouseId, stockSituation, addressingSituation));
        }

        private List<SelectListItem> GetEnumSelectListWithAll<T>(string displayNameAll) where T : Enum
        {
            return GetEnumSelectListWithAll<T>(displayNameAll, new Dictionary<int, string>());
        }

        private List<SelectListItem> GetEnumSelectListWithAll<T>(string displayNameAll, Dictionary<int, string> additionalOptions) where T : Enum
        {
            var selectList = new List<SelectListItem> { new SelectListItem { Value = "-2", Text = displayNameAll } };

            foreach (var kvp in additionalOptions)
            {
                selectList.Add(new SelectListItem { Value = kvp.Key.ToString(), Text = kvp.Value });
            }

            selectList.AddRange(Enum.GetValues(typeof(T))
                .OfType<T>()
                .Select(enumValue => new SelectListItem
                {
                    Value = ((int)(object)enumValue).ToString(),
                    Text = enumValue.GetDisplayName()
                }));

            return selectList;
        }

        public IActionResult ExportCSV()
        {
            _reportViewService.ExportToCSV();

            var filePath = "report.csv"; // Nome do arquivo gerado pela função ExportToCSV

            // Retorna o arquivo para download
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "text/csv", "Relatorio.csv");
        }
    }
}