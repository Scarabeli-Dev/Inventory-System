using Inventory.Helpers.Exceptions;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Relational;
using System.Globalization;

namespace Inventory.Controllers
{
    [Authorize]
    public class StockTakingController : Controller
    {
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;

        public StockTakingController(IStockTakingService stockTakingService,
                                     IItemService itemService,
                                     IAddressingService addressingService,
                                     IWarehouseService warehouseService,
                                     IInventoryStartService inventoryStartService,
                                     IAddressingsInventoryStartService addressingsStockTakingService)
        {
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ItemCount(string itemId, bool stockTakingCheched)
        {
            var checkStockTacking = await _stockTakingService.GetAllStockTakingByItemIdAsync(itemId);

            if (checkStockTacking.Count() > 0 && stockTakingCheched == false)
                return RedirectToAction("CheckStockTaking", "StockTaking", new { itemId = itemId });


            var item = await _itemService.GetItemByIdAsync(itemId);

            ViewBag.item = item;

            var addressing = await _addressingService.GetAddressingByItemIdAsync(itemId);
            ViewBag.Addressing = addressing;

            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(addressing.WarehouseId);
            ViewBag.Addressings = addressings;

            ViewBag.Inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(addressing.Id);


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCount(StockTaking stockTaking)
        {
            if (ModelState.IsValid)
            {
                stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                await _stockTakingService.NewStockTakingAsync(stockTaking);
                await _stockTakingService.SaveChangesAsync();

                return RedirectToAction("Details", "Warehouses", new { id = stockTaking.AddressingsInventoryStart.Addressing.WarehouseId });
            }
            var item = await _itemService.GetItemByIdAsync(stockTaking.ItemId);

            ViewBag.item = item;
            return View(stockTaking);
        }

        public async Task<IActionResult> ItemCountEdit(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);

            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(stockTaking.AddressingsInventoryStart.Addressing.WarehouseId);
            ViewBag.Addressings = addressings;

            return View(stockTaking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCountEdit(string itemId, StockTaking stockTaking)
        {
            if (itemId != stockTaking.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _stockTakingService.UpdateStockTaking(stockTaking);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await _stockTakingService.GetStockTakingByIdAsync(stockTaking.Id);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Warehouses", new { id = stockTaking.AddressingsInventoryStart.Addressing.WarehouseId });
            }
            return View(stockTaking);
        }

        public async Task<IActionResult> CheckStockTaking(string itemId)
        {
            var itemStockTaking = await _stockTakingService.GetAllStockTakingByItemIdAsync(itemId);
            var item = await _itemService.GetItemByIdAsync(itemId);

            ViewBag.Item = item;
            return View(itemStockTaking);
        }


        public async Task<IActionResult> StockTakingReport(int addressingId)
        {
            var stockTakingAddressing = await _addressingsStockTakingService.GetAddressingsStockTakingAddressingByIdAsync(addressingId);


            var stockTakingItems = await _stockTakingService.GetStockTakingByAddressingAsync(addressingId);


            List<StockTakingReportAddressing> result = new List<StockTakingReportAddressing>();

            foreach (var item in stockTakingItems)
            {
                StockTakingReportAddressing model = new StockTakingReportAddressing();

                model.StockTakingId = item.Id;
                model.ItemId = item.ItemId;
                model.ItemName = item.Item.Name;

                var itemVerify = await _itemService.GetItemByIdAsync(item.ItemId);

                if (!itemVerify.Addressings.Any(a => a.Addressing.Id == addressingId))
                {
                    model.ItemInitialQuantity = 0;
                }

                model.ItemInitialQuantity = itemVerify.Addressings.FirstOrDefault(a => a.AddressingId == item.AddressingsInventoryStart.AddressingId).Quantity;


                var stockTaking = await _stockTakingService.GetStockTakingByAddressingAndItemIdAsync(addressingId, item.ItemId);

                if (stockTaking == null)
                {
                    model.ItemStockTakingQuantity = 0;
                    model.NumberOfCount = 0;
                }
                else
                {
                    model.ItemStockTakingQuantity = stockTaking.StockTakingQuantity;
                    model.NumberOfCount = stockTaking.NumberOfCount;
                }

                result.Add(model);
            }

            ViewBag.AddressingInfo = stockTakingAddressing;

            return View(result);
        }
    }
}
