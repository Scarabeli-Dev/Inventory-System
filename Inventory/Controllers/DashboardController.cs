using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IWarehouseService _warehouseService;

        public DashboardController(IDashboardService dashboardService, IWarehouseService warehouseService)
        {
            _dashboardService = dashboardService;
            _warehouseService = warehouseService;
        }

        public async Task<IActionResult> Index(int warehouseId)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Depósito", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            var model = _dashboardService.DashboardData(warehouseId);
            return View(model);
        }
    }
}
