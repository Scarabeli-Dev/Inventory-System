using Inventory.Models;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Collections.Generic;

namespace Inventory.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InventoryStartController : Controller
    {
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;

        public InventoryStartController(IInventoryStartService inventoryStartService, IAddressingsInventoryStartService addressingsStockTakingService)
        {
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "InventoryStartDate")
        {
            return View(await _inventoryStartService.GetAllInventoryStartsAsync(filter, pageindex, sort));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InventoryStart inventoryStart)
        {
            if (ModelState.IsValid)
            {
                await _inventoryStartService.CreateInventoryStartAsync(inventoryStart);

                return RedirectToAction(nameof(Index));
            }
            return View(inventoryStart);
        }

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

        public async Task<IActionResult> AddressingCount(int inventaryStartId, string filter, int pageindex = 1, string sort = "Id")
        {
            return View(await _addressingsStockTakingService.GetAddressingsStockTakingsPagingAsync(inventaryStartId, filter, pageindex, sort));
        }
    }
}
