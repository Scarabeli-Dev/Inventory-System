using Inventory.Models;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    public class StockTakingController : Controller
    {
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;

        public StockTakingController(IStockTakingService stockTakingService, IItemService itemService, IAddressingService addressingService, IWarehouseService warehouseService)
        {
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int itemId)
        {
            ViewBag.Item = await _itemService.GetItemByIdAsync(itemId);

            ViewBag.Warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            ViewBag.Addressing = await _addressingService.GetAllAsync<Addressing>();

            return View();
        }
    }
}
