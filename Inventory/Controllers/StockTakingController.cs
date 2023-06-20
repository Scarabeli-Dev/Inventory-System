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
        private readonly IItemsStockTakingService _itemsStockTakingService;
        private readonly IAddressingsStockTakingService _addressingsStockTakingService;

        public StockTakingController(IStockTakingService stockTakingService, IItemService itemService, IAddressingService addressingService, IWarehouseService warehouseService, IInventoryStartService inventoryStartService, IItemsStockTakingService itemsStockTakingService, IAddressingsStockTakingService addressingsStockTakingService)
        {
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _inventoryStartService = inventoryStartService;
            _itemsStockTakingService = itemsStockTakingService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ItemCount(string itemId)
        {
            var itemStockCount = await _itemsStockTakingService.GetItemsStockTakingItemByIdAsync(itemId);

            if (itemStockCount == null)
            {
                throw new NotFoundException("Contagem de estoque não encontrada");
            }
            if (itemStockCount.ItemCountRealized == true)
            {
                return RedirectToAction(nameof(ItemCountEdit), new { itemId = itemId });
            }
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

                return RedirectToAction("Details", "Warehouses", new { id = stockTaking.Addressing.WarehouseId });
            }
            var item = await _itemService.GetItemByIdAsync(stockTaking.ItemId);

            ViewBag.item = item;
            return View(stockTaking);
        }

        public async Task<IActionResult> ItemCountEdit(string itemId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByItemIdAsync(itemId);

            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(stockTaking.Addressing.WarehouseId);
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
                Addressing wareHouseReturn = await _addressingService.GetAddressingByIdAsync(stockTaking.AddressingId);

                return RedirectToAction("Details", "Warehouses", new { id = wareHouseReturn.WarehouseId });
            }
            return View(stockTaking);
        }

        public async Task<IActionResult> StockTakingReport(int addressingId)
        {
            var stockTakingAddressing = await _addressingsStockTakingService.GetAddressingsStockTakingAddressingByIdAsync(addressingId);


            var stockTakingItems = await _itemsStockTakingService.GetItemsStockTakingItemByAddressingIdAsync(addressingId);


            List<StockTakingReportAddressing> result = new List<StockTakingReportAddressing>();

            foreach (var item in stockTakingItems)
            {
                StockTakingReportAddressing model = new StockTakingReportAddressing();

                model.StockTakingId = item.Id;
                model.ItemId = item.ItemId;
                model.ItemName = item.Item.Name;

                var itemVerify = await _itemService.GetItemByIdAsync(item.ItemId);

                var stockTaking = await _stockTakingService.GetStockTakingByItemIdAsync(item.ItemId);

                //if (!itemVerify.Addressings.Any(a => a.Addressing.Id == addressingId) && !itemVerify.Addressings.Any(a => a.Addressing.Id == stockTaking.AddressingId))
                if (!itemVerify.Addressings.Any(a => a.Addressing.Id == addressingId))
                {
                    model.ItemInitialAmount = 0;
                }
                model.ItemInitialAmount = item.Item.Quantity;


                if (stockTaking == null)
                {
                    model.ItemStockTakingAmount = 0;
                    model.NumberOfCount = 0;
                }
                else
                {
                    model.ItemStockTakingAmount = stockTaking.StockTakingQuantity;
                    model.NumberOfCount = stockTaking.NumberOfCount;
                }

                result.Add(model);
            }

            ViewBag.AddressingInfo = stockTakingAddressing;

            return View(result);
        }
    }
}
