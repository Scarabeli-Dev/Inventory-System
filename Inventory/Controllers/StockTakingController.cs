using FastReport;
using Inventory.Helpers.Exceptions;
using Inventory.Models;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IPerishableItemService _perishableItemService;
        public string _destiny = "StockTaking";

        public StockTakingController(IStockTakingService stockTakingService,
                                     IItemService itemService,
                                     IAddressingService addressingService,
                                     IWarehouseService warehouseService,
                                     IInventoryStartService inventoryStartService,
                                     IAddressingsInventoryStartService addressingsInventoryStartService,
                                     IItemAddressingService itemAddressingService,
                                     IPerishableItemService perishableItemService)
        {
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _inventoryStartService = inventoryStartService;
            _addressingsInventoryStartService = addressingsInventoryStartService;
            _itemAddressingService = itemAddressingService;
            _perishableItemService = perishableItemService;
        }

        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "AddressingCountRealized", int warehouseId = 0, int countStatus = 0)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Todos", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            var addressings = new List<AddressingsInventoryStart>();
            if (warehouseId > 0)
            {
                addressings = _addressingsInventoryStartService.AddressingsInventoryStarts.Where(w => w.Addressing.WarehouseId == warehouseId).ToList();
            }
            else
            {
                addressings = _addressingsInventoryStartService.AddressingsInventoryStarts.ToList();
            }

            decimal itemInAddressing = 0;
            decimal itemCount = 0;
            foreach (var addressing in addressings)
            {
                itemInAddressing = itemInAddressing + addressing.Addressing.Item.Count();

                itemCount = itemCount + addressing.StockTaking.Count();
            }

            var generalProgress = (itemCount * 100) / itemInAddressing;
            var formattedProgress = generalProgress.ToString("F2");

            ViewBag.GeneralProgress = formattedProgress;


            List<SelectListItem> countStatusOptions = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "Todos" },
                    new SelectListItem { Value = "1", Text = "Pendente" },
                    new SelectListItem { Value = "2", Text = "Contado" },
                    new SelectListItem { Value = "3", Text = "Finalizado" }
                };

            ViewBag.StatusSelect = countStatus.ToString();
            ViewBag.CountStatusOptions = countStatusOptions;


            return View(await _addressingsInventoryStartService.GetAddressingsStockTakingsPagingAsync(filter, pageindex, sort, warehouseId, countStatus));
        }

        [Route("Lista/Deposito")]
        public async Task<IActionResult> IndexInventaryStart(int inventaryStartId, string filter, int pageindex = 1, string sort = "AddressingCountRealized")
        {
            return View(await _addressingsInventoryStartService.GetAddressingsStockTakingsByInventoryPagingAsync(inventaryStartId, filter, pageindex, sort));
        }

        [Route("Lista/Recontagem")]
        public async Task<IActionResult> IndexRecount(string filter, int pageindex = 1, string sort = "ItemId")
        {
            return View(await _stockTakingService.GetAllStocktakingWithRecount(filter, pageindex, sort));
        }

        [Route("Cadastro")]
        public async Task<IActionResult> ItemCount(string itemId, bool stockTakingCheched, string filter, int pageindex = 1, string sort = "Name")
        {
            var checkStockTacking = await _stockTakingService.GetAllStockTakingByItemIdAsync(itemId);

            if (checkStockTacking.Count() > 0 && stockTakingCheched == false && filter == null && pageindex == 1)
                return RedirectToAction("CheckStockTaking", "StockTaking", new { itemId = itemId });

            int changePage = 0;
            if (pageindex > 1 || filter != null)
            {
                changePage = 1;
            }

            var item = await _itemService.GetItemByIdAsync(itemId);
            ViewBag.item = item;
            ViewBag.ItemId = item.Id;

            var addressing = await _addressingService.GetAddressingByItemIdAsync(itemId);
            ViewBag.Addressing = addressing;

            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(addressing.WarehouseId);
            ViewBag.Addressings = addressings;

            var addressingsPage = await _addressingService.GetAllAddressingsByPagingForCountAsync(itemId, stockTakingCheched, filter, pageindex, sort);

            ViewBag.ChangePage = changePage;

            ViewBag.AddressingsPage = addressingsPage;

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
            try
            {
                var inventoryVerify = await _addressingsInventoryStartService.GetAddressingsStockTakingByAddressingIdAsync(stockTaking.AddressingsInventoryStartId);
                var stockTakingVerify = await _stockTakingService.GetStockTakingByAddressingAndItemIdAsync(inventoryVerify.AddressingId, stockTaking.ItemId);
                if (inventoryVerify == null)
                {
                    TempData["errorMessage"] = "contagem. Inventário não aberto para o dépósito";
                    return RedirectToAction("Index", "Warehouses");
                }
                if (stockTakingVerify != null)
                {
                    TempData["errorMessage"] = $"contagem. Já existe uma contagem para o item {stockTakingVerify.ItemId} - {stockTakingVerify.Item.Name} no endereçamento {stockTakingVerify.AddressingsInventoryStart.Addressing.Name}";
                    return RedirectToAction("Index", "Warehouses");
                }


                var itemCount = await _itemService.GetItemByIdForCountAsync(stockTaking.ItemId);

                if (ModelState.IsValid)
                {
                    if (stockTaking.IsPerishableItem)
                    {

                        foreach (var perishableItem in stockTaking.PerishableItem.ToList())
                        {
                            if (perishableItem.PerishableItemQuantity == 0)
                            {
                                stockTaking.PerishableItem.Remove(perishableItem);
                            }
                        }
                    }

                    stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                    await _stockTakingService.SaveStockTakingWithRecount(stockTaking);

                    TempData["successMessage"] = "Contagem do item " + itemCount.Id + "- " + itemCount.Name;
                    return RedirectToAction("Details", "Warehouses", new { id = inventoryVerify.Addressing.WarehouseId });
                }

                if (!ModelState.IsValid && (stockTaking.IsPerishableItem || (stockTaking.IsPerishableItem == false && stockTaking.PerishableItem.Count() > 0)))
                {
                    foreach (var perishableItem in stockTaking.PerishableItem.ToList())
                    {
                        if (perishableItem.PerishableItemQuantity == 0)
                        {
                            stockTaking.PerishableItem.Remove(perishableItem);
                        }
                    }
                    if (stockTaking.PerishableItem.Count() == 0)
                    {
                        stockTaking.IsPerishableItem = false;
                    }

                    stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                    await _stockTakingService.SaveStockTakingWithRecount(stockTaking);

                    TempData["successMessage"] = "Contagem do item " + itemCount.Id + "- " + itemCount.Name;
                    return RedirectToAction("Details", "Warehouses", new { id = stockTaking.AddressingsInventoryStart.Addressing.WarehouseId });
                }

                var item = await _itemService.GetItemByIdAsync(stockTaking.ItemId);
                ViewBag.item = item;

                var addressing = await _addressingService.GetAddressingByItemIdAsync(stockTaking.ItemId);
                ViewBag.Addressing = addressing;

                var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(addressing.WarehouseId);
                ViewBag.Addressings = addressings;

                ViewBag.Inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(addressing.Id);

                TempData["errorMessage"] = "contagem do item " + itemCount.Id + "- " + itemCount.Name;

                return View(stockTaking);
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction("Error", "Error", new
                {
                    message = e.Message
                });
            }
        }


        [Route("Editar")]
        public async Task<IActionResult> ItemCountEdit(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);
            if (stockTaking == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Contagem não encontrada" });
            }
            if (stockTaking.AddressingsInventoryStart.AddressingCountEnded == true)
            {
                TempData["errorMessage"] = $"Contagem do endereçamento {stockTaking.AddressingsInventoryStart.Addressing.Name} já encerrada";
                return RedirectToAction(nameof(Index));
            }
            if (stockTaking.ItemToRecount)
            {
                stockTaking.StockTakingQuantity = 0;
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
                var inventoryVerify = await _addressingsInventoryStartService.GetAddressingsStockTakingByAddressingIdAsync(stockTaking.AddressingsInventoryStartId);
                if (inventoryVerify.AddressingCountEnded)
                {
                    TempData["errorMessage"] = $"Contagem do endereçamento {stockTaking.AddressingsInventoryStart.Addressing.Name} já encerrada";
                    return RedirectToAction(nameof(Index));
                }

                var item = await _itemService.GetItemByIdForCountAsync(itemId);
                if (itemId != stockTaking.ItemId)
                {
                    return RedirectToAction("Error", "Error", new { message = "Contagem não encontrada" });
                }

                if (ModelState.IsValid)
                {
                    if (stockTaking.IsPerishableItem)
                    {

                        foreach (var perishableItem in stockTaking.PerishableItem.ToList())
                        {
                            if (perishableItem.PerishableItemQuantity == 0)
                            {
                                stockTaking.PerishableItem.Remove(perishableItem);
                            }
                        }
                    }
                    stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                    stockTaking.AddressingsInventoryStartId = inventoryVerify.Id;
                    await _stockTakingService.SaveStockTakingWithOutRecount(stockTaking);

                    var result = await _stockTakingService.GetStockTakingByIdAsync(stockTaking.Id);
                    TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;

                    return RedirectToAction("Details", "Warehouses", new { id = result.AddressingsInventoryStart.Addressing.WarehouseId });
                }

                if (!ModelState.IsValid && (stockTaking.IsPerishableItem || (stockTaking.IsPerishableItem == false && stockTaking.PerishableItem.Count() > 0)))
                {
                    foreach (var perishableItem in stockTaking.PerishableItem.ToList())
                    {
                        if (perishableItem.PerishableItemQuantity == 0)
                        {
                            stockTaking.PerishableItem.Remove(perishableItem);
                        }
                    }

                    if (stockTaking.PerishableItem.Count() == 0)
                    {
                        stockTaking.IsPerishableItem = false;
                    }

                    stockTaking.StockTakingQuantity = decimal.Parse(stockTaking.StockTakingQuantity.ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                    stockTaking.AddressingsInventoryStartId = inventoryVerify.Id;
                    await _stockTakingService.SaveStockTakingWithOutRecount(stockTaking);

                    TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;
                    return RedirectToAction("Details", "Warehouses", new { id = stockTaking.AddressingsInventoryStart.Addressing.WarehouseId });
                }

                ViewBag.item = item;

                var addressing = await _addressingService.GetAddressingByItemIdAsync(stockTaking.ItemId);
                ViewBag.Addressing = addressing;

                var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(addressing.WarehouseId);
                ViewBag.Addressings = addressings;

                ViewBag.Inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(addressing.Id);

                TempData["errorMessage"] = "contagem do item " + item.Id + "- " + item.Name;

                return View(stockTaking);
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction("Error", "Error", new { message = ex.Message });
            }

        }

        [HttpPost]
        [Route("Adiciona-Recontar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecount(int stockTakingId)
        {
            var stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);

            if (stockTaking == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Contagem não encontrada" });
            }
            try
            {
                if (await _stockTakingService.AddStockTakingForRecountAssync(stockTakingId))
                {
                    await _stockTakingService.SaveChangesAsync();

                    TempData["successMessage"] = "Recontagem do item " + stockTaking.ItemId + "- " + stockTaking.Item.Name;
                    return RedirectToAction("StockTakingReport", "StockTaking", new { addressingId = stockTaking.AddressingsInventoryStart.AddressingId });
                }
                TempData["errorMessage"] = "contagem do item " + stockTaking.ItemId + "- " + stockTaking.Item.Name;
                return RedirectToAction("StockTakingReport", "StockTaking", new { addressingId = stockTaking.AddressingsInventoryStart.AddressingId });
            }
            catch (Exception)
            {

                throw;
            }

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
                    return RedirectToAction("Error", "Error", new { message = "Contagem não encontrada" });
                }

                if (ModelState.IsValid)
                {

                    if (await _stockTakingService.SaveStockTakingWithRecount(stockTaking))
                    {
                        var result = await _stockTakingService.GetStockTakingByIdAsync(stockTaking.Id);
                        TempData["successMessage"] = "Contagem do item " + item.Id + "- " + item.Name;

                        return RedirectToAction("Details", "Warehouses", new { id = result.AddressingsInventoryStart.Addressing.WarehouseId });
                    }

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
            var stockTakingAddressing = await _addressingsInventoryStartService.GetAddressingsStockTakingByAddressingIdAsync(addressingId);
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
                    model.RecoutItem = stockTaking.ItemToRecount;
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
        [Route("Encerrar")]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [Route("Reabrir")]
        public async Task<IActionResult> ReopenAddressingInventory(int addressingId)
        {
            if (await _addressingsInventoryStartService.SetAddressingCountEndedFalseAsync(addressingId))
            {
                TempData["successMessage"] = "Contagem";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorMessage"] = "concluir contagem";
            return RedirectToAction("StockTakingReport", "StockTaking", new { itemId = addressingId });
        }

        [Route("Deletar")]
        public async Task<IActionResult> DeleteStockTaking(int stockTakingId)
        {
            StockTaking stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);
            var addressingId = stockTaking.AddressingsInventoryStart.AddressingId;

            if (stockTaking == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });

            }

            return View(stockTaking);
        }

        [HttpPost, ActionName("DeleteStockTaking")]
        [Route("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int stockTakingId)
        {
            StockTaking stockTaking = await _stockTakingService.GetStockTakingByIdAsync(stockTakingId);
            var addressingId = stockTaking.AddressingsInventoryStart.AddressingId;

            if (stockTaking == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });

            }

            _stockTakingService.Delete(stockTaking);

            await _stockTakingService.SaveChangesAsync();

            return RedirectToAction("StockTakingReport", "StockTaking", new { addressingId = addressingId });
        }

        [Route("Lista/Pereciveis")]
        public async Task<IActionResult> PerishableItems(string filter, int pageindex = 1, string sortExpression = "ExpirationDate", int warehouseId = 0, bool nullExpirationDate = false, DateTime? expirationDate = null)
        {
            var model = await _perishableItemService.GetAllPerishableItemsPagingListAsync(filter, pageindex, sortExpression, warehouseId, nullExpirationDate, expirationDate);

            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();
            var warehouseList = warehouses.Select(w => new SelectListItem { Text = w.Name, Value = w.Id.ToString() }).ToList();
            warehouseList.Insert(0, new SelectListItem { Text = "Todos", Value = 0.ToString() });
            ViewData["WarehouseId"] = new SelectList(warehouseList, "Value", "Text", warehouseId);

            ViewData["ExpirationDate"] = expirationDate != null ? ((DateTime)expirationDate).ToString("yyyy-MM-dd") : "";

            return View(model);
        }
    }
}
