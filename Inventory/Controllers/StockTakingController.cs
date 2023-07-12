using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Inventory.Controllers
{
    [Route("Contagem")]
    [Authorize]
    public class StockTakingController : Controller
    {
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsInventoryStartService _addressingsInventoryStartService;
        private readonly IItemAddressingService _itemAddressingService;
        public string _destiny = "StockTaking";

        public StockTakingController(IStockTakingService stockTakingService,
                                     IItemService itemService,
                                     IAddressingService addressingService,
                                     IWarehouseService warehouseService,
                                     IInventoryStartService inventoryStartService,
                                     IAddressingsInventoryStartService addressingsInventoryStartService,
                                     IItemAddressingService itemAddressingService)
        {
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _inventoryStartService = inventoryStartService;
            _addressingsInventoryStartService = addressingsInventoryStartService;
            _itemAddressingService = itemAddressingService;
        }

        public async Task<IActionResult> Index(int inventaryStartId, string filter, int pageindex = 1, string sort = "AddressingCountRealized")
        {
            return View(await _addressingsInventoryStartService.GetAddressingsStockTakingsPagingAsync(1, filter, pageindex, sort));
        }

        [Route("Lista/Recontagem")]
        public async Task<IActionResult> IndexRecount(string filter, int pageindex = 1, string sort = "ItemId")
        {
            return View(await _stockTakingService.GetAllStocktakingWithRecount(filter, pageindex, sort));
        }

        [Route("Cadastro")]
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

            var model = new StockTaking();
            model.PerishableItem = new List<PerishableItem>();

            return View(model);
        }

        [HttpPost]
        [Route("Cadastro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCount(StockTaking stockTaking)
        {
            var item = await _itemService.GetItemByIdAsync(stockTaking.ItemId);
            if (ModelState.IsValid)
            {
                stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                await _stockTakingService.SaveStockTakingWithRecount(stockTaking);

                TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;
                TempData["wait"] = "wait";
                return RedirectToAction("Details", "Warehouses", new { id = stockTaking.AddressingsInventoryStart.Addressing.WarehouseId });
            }

            ViewBag.item = item;

            TempData["errorMessage"] = "contagem do item " + item.Id + "- " + item.Name;

            return View(stockTaking);
        }

        [Route("Editar")]
        public async Task<IActionResult> ItemCountEdit(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);
            if (stockTaking.AddressingsInventoryStart.AddressingCountEnded == true)
            {
                TempData["errorMessage"] = "Contagem do endereçamento já encerrada";
                return RedirectToAction(nameof(Index));
            }
            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(stockTaking.AddressingsInventoryStart.Addressing.WarehouseId);
            ViewBag.Addressings = addressings;

            ViewBag.IsRecount = stockTaking.ItemToRecount;

            return View(stockTaking);
        }

        [HttpPost]
        [Route("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemCountEdit(string itemId, StockTaking stockTaking)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(itemId);
                if (itemId != stockTaking.ItemId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _stockTakingService.SaveStockTakingWithOutRecount(stockTaking);

                    var result = await _stockTakingService.GetStockTakingByIdAsync(stockTaking.Id);
                    TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;

                    return RedirectToAction("Details", "Warehouses", new { id = result.AddressingsInventoryStart.Addressing.WarehouseId });
                }
                TempData["errorMessage"] = "contagem do item " + item.Id + "- " + item.Name;
                return View(stockTaking);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o StockTaking: " + ex.Message);
                return View("Error");
            }

        }

        [HttpPost]
        [Route("Adiciona-Recontar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecount(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);

            if (await _stockTakingService.AddStockTakingForRecountAssync(stockTakingId))
            {
                TempData["successMessage"] = "Recontagem do item " + stockTaking.ItemId + "- " + stockTaking.Item.Name;
                return RedirectToAction(nameof(IndexRecount));
            }

            TempData["errorMessage"] = "contagem do item " + stockTaking.ItemId + "- " + stockTaking.Item.Name;
            return RedirectToAction(nameof(IndexRecount));
        }

        [Route("Recontar")]
        public async Task<IActionResult> ItemRecount(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);
            if (stockTaking.AddressingsInventoryStart.AddressingCountEnded == true)
            {
                return RedirectToAction(nameof(Index));
            }
            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(stockTaking.AddressingsInventoryStart.Addressing.WarehouseId);
            ViewBag.Addressings = addressings;

            return View(stockTaking);
        }

        [HttpPost]
        [Route("Recontar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemRecount(string itemId, StockTaking stockTaking)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(itemId);
                if (itemId != stockTaking.ItemId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _stockTakingService.SaveStockTakingWithRecount(stockTaking);

                    var result = await _stockTakingService.GetStockTakingByIdAsync(stockTaking.Id);
                    TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;

                    return RedirectToAction("Details", "Warehouses", new { id = result.AddressingsInventoryStart.Addressing.WarehouseId });
                }
                TempData["errorMessage"] = "contagem do item " + item.Id + "- " + item.Name;
                return View(stockTaking);
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao atualizar o StockTaking: " + ex.Message);
                return View("Error");
            }

        }

        [Route("Verificacao/Enderecamento")]
        public async Task<IActionResult> CheckStockTaking(string itemId)
        {
            var itemStockTaking = await _stockTakingService.GetAllStockTakingByItemIdAsync(itemId);
            var item = await _itemService.GetItemByIdAsync(itemId);

            ViewBag.Item = item;
            return View(itemStockTaking);
        }

        [Route("Relatorio")]
        public async Task<IActionResult> StockTakingReport(int addressingId)
        {
            // Local Variable
            List<StockTakingReportAddressing> result = new List<StockTakingReportAddressing>();

            List<string> itemStockTakingVerify = new List<string>();

            // Get
            var stockTakingAddressing = await _addressingsInventoryStartService.GetAddressingsStockTakingAddressingByIdAsync(addressingId);
            ViewBag.AddressingInfo = stockTakingAddressing;

            var stockTakingItems = await _stockTakingService.GetStockTakingByAddressingAsync(addressingId);

            var itemsAddressing = await _itemService.GetAllItemsByAddressingAsync(addressingId);

            foreach (var item in stockTakingItems)
            {
                StockTakingReportAddressing model = new StockTakingReportAddressing();

                itemStockTakingVerify.Add(item.ItemId);

                model.StockTakingId = item.Id;
                model.ItemId = item.ItemId;
                model.ItemName = item.Item.Name;

                var itemAddressingVerify = await _itemService.GetItemByIdAsync(item.ItemId);

                if (!itemAddressingVerify.Addressings.Any(a => a.Addressing.Id == addressingId))
                {
                    model.ItemInitialQuantity = 0;
                }
                else
                {
                    model.ItemInitialQuantity = itemAddressingVerify.Addressings.FirstOrDefault(a => a.AddressingId == item.AddressingsInventoryStart.AddressingId).Quantity;
                }



                var stockTaking = await _stockTakingService.GetStockTakingByAddressingAndItemIdAsync(addressingId, item.ItemId);

                if (stockTaking == null)
                {
                    model.ItemStockTakingQuantity = 0;
                    model.ItemStockTakingPreviousQuantity = 0;
                    model.NumberOfCount = 0;
                }
                else
                {
                    model.ItemStockTakingQuantity = stockTaking.StockTakingQuantity;
                    model.ItemStockTakingPreviousQuantity = stockTaking.StockTakingPreviousQuantity;
                    model.NumberOfCount = stockTaking.NumberOfCount;
                }

                result.Add(model);
            }

            foreach (var itemWithOutStockTaking in itemsAddressing)
            {
                if (!itemStockTakingVerify.Contains(itemWithOutStockTaking.Id))
                {
                    StockTakingReportAddressing model = new StockTakingReportAddressing();
                    var itemAddresing = await _itemAddressingService.GetItemAddressingByIdsAsync(itemWithOutStockTaking.Id, addressingId);
                    model.ItemId = itemWithOutStockTaking.Id;
                    model.ItemName = itemWithOutStockTaking.Name;
                    model.ItemInitialQuantity = itemAddresing.Quantity;
                    model.ItemStockTakingQuantity = 0;
                    model.NumberOfCount = 0;
                    result.Add(model);
                }
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CloseAddressingInventory(int addressingId)
        {
            if (await _addressingsInventoryStartService.SetAddressingCountEndedTrueAsync(addressingId))
            {
                TempData["successMessage"] = "Contagem";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorMessage"] = "concluir contagem";
            return RedirectToAction("StockTakingReport", "StockTaking", new { itemId = addressingId });
        }
    }
}
