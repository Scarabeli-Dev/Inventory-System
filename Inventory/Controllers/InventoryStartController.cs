using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Controllers
{
    [Route("Inventario")]
    [Authorize(Roles = "Admin")]
    public class InventoryStartController : Controller
    {
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;
        private readonly IWarehouseService _warehouseService;

        public InventoryStartController(IInventoryStartService inventoryStartService, IAddressingsInventoryStartService addressingsStockTakingService, IWarehouseService warehouseService)
        {
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
            _warehouseService = warehouseService;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "InventoryStartDate")
        {
            ViewData["WarehouseId"] = new SelectList(_warehouseService.Warehouses, "Id", "Name");
            return View(await _inventoryStartService.GetAllInventoryStartsAsync(filter, pageindex, sort));
        }

        [Route("Cadastro")]
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList( _warehouseService.Warehouses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("Cadastro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryStart inventoryStart)
        {
            if (ModelState.IsValid)
            {
                await _inventoryStartService.CreateInventoryStartAsync(inventoryStart);

                TempData["successMessage"] = "Inventário";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorMessage"] = "Inventário";
            ViewData["WarehouseId"] = new SelectList(await _warehouseService.GetAllAsync<Warehouse>(), "Id", "Name", inventoryStart.WarehouseId);
            return View(inventoryStart);
        }

        [Route("Detalhe")]
        public async Task<IActionResult> Details(int id, string filter)
        {
            var inventoryStart = await _inventoryStartService.GetInventoryStartByIdAsync(id);

            if (inventoryStart == null)
            {
                return NotFound();
            }

            //List<AddressingsStockTaking> addressingsStockTakings = new List<AddressingsStockTaking>();

            //foreach (var item in inventoryStart.Addressings)
            //{
            //    addressingsStockTakings.Add(item);
            //}

            //List <AddressingsStockTaking> stockTakingOpen = new List<AddressingsStockTaking>();
            //List <AddressingsStockTaking> stockTakingRealized = new List<AddressingsStockTaking>();
            //List <AddressingsStockTaking> stockTakingEnded = new List<AddressingsStockTaking>();

            //foreach (var item in inventoryStart.Addressings)
            //{
            //    if (item.AddressingCountRealized == false && item.AddressingCountEnded == false)
            //    {
            //        stockTakingOpen.Add(item);
            //    }
            //    if (item.AddressingCountRealized == true && item.AddressingCountEnded == false)
            //    {
            //        stockTakingRealized.Add(item);
            //    }
            //    if (item.AddressingCountRealized == true && item.AddressingCountEnded == true)
            //    {
            //        stockTakingEnded.Add(item);
            //    }
            //}

            //ViewBag.StockTakingOpen = stockTakingOpen;
            //ViewBag.StockTakingRealized = stockTakingRealized;
            //ViewBag.StockTakingEnded = stockTakingEnded;


            ViewBag.Filter = filter;

            return View(inventoryStart);
        }

        [Route("Contagem/Enderecamento")]
        public async Task<IActionResult> AddressingCount(string filter, int pageSize = 10, int pageindex = 1, string sort = "Id")
        {
            return View(await _addressingsStockTakingService.GetAddressingsStockTakingsPagingAsync(filter, pageSize, pageindex, sort));
        }
    }
}
