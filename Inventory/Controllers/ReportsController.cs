using FastReport;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ReflectionIT.Mvc.Paging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Inventory.Controllers
{
    [Route("Relatorios")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IInventoryMovementService _inventoryMovementService;
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IItemAddressingService _itemAddressingService;

        public ReportsController(IInventoryMovementService inventoryMovementService,
                                 IStockTakingService stockTakingService,
                                 IItemService itemService,
                                 IItemAddressingService itemAddressingService)
        {
            _inventoryMovementService = inventoryMovementService;
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _itemAddressingService = itemAddressingService;
        }

        [Route("Contagem-com-movimentacao")]
        public async Task<IActionResult> ReportWithMovementation(string filter, int pageindex = 1, string sort = "ItemName")
        {
            List<StockTakingReport> view = new List<StockTakingReport>();

            var inventoryMovement = _inventoryMovementService.GetAllInventoryMovementsAsync();

            HashSet<int> stockTakingIdWithMovement = new HashSet<int>();
            HashSet<string> itemWithStockTaking = new HashSet<string>();

            foreach (var item in inventoryMovement)
            {
                StockTakingReport stockTakingWithMovement = new StockTakingReport();

                Item baseItem = _itemService.Items.FirstOrDefault(a => a.Addressings.Any(w => w.Addressing.WarehouseId == item.WarehouseId && (w.ItemId == item.ItemId)));
                StockTaking stockTackingItem = await _stockTakingService.GetStockTakingByWarehouseAndItemIdAsync(item.WarehouseId, item.ItemId);
                ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(baseItem.Id, stockTackingItem.AddressingsInventoryStart.AddressingId);


                stockTakingWithMovement.ItemId = item.ItemId;
                stockTakingWithMovement.ItemName = item.Item.Name;
                stockTakingWithMovement.QuantityStockTaking = stockTackingItem.StockTakingQuantity;
                stockTakingWithMovement.QuantityMovement = item.Amount;
                stockTakingWithMovement.MovementeType = item.MovementeType;
                stockTakingWithMovement.InitialQuantity = itemAddressing.Quantity;

                if (item.MovementDate < stockTackingItem.StockTakingDate)
                {

                    if (item.MovementeType == Models.Enums.MovementeType.E)
                    {
                        stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement + stockTakingWithMovement.QuantityStockTaking;
                    }
                    else
                    {
                        stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement - stockTakingWithMovement.QuantityStockTaking;

                    }
                }
                else
                {
                    stockTakingWithMovement.QuantityClosed = stockTackingItem.StockTakingQuantity;
                }
                view.Add(stockTakingWithMovement);
                stockTakingIdWithMovement.Add(stockTackingItem.Id);
            }
            var allStockTaking = await _stockTakingService.GetAllAsync<StockTaking>();

            foreach (var item in allStockTaking)
            {
                itemWithStockTaking.Add(item.ItemId);
                if (stockTakingIdWithMovement.Contains(item.Id))
                {
                    continue;
                }
                StockTakingReport stockTakingReport = new StockTakingReport();
                ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(item.ItemId, item.AddressingsInventoryStart.AddressingId);


                stockTakingReport.ItemId = item.ItemId;
                stockTakingReport.ItemName = item.Item.Name;
                stockTakingReport.InitialQuantity = itemAddressing.Quantity;
                stockTakingReport.QuantityStockTaking = item.StockTakingQuantity;
                stockTakingReport.QuantityMovement = 0;
                stockTakingReport.MovementeType = Models.Enums.MovementeType.SM;
                stockTakingReport.QuantityClosed = item.StockTakingQuantity;
                view.Add(stockTakingReport);
            }
            view.AsNoTracking();
            var model = await PagingList.CreateAsync(view, 10, pageindex, sort, "Name");

            return View(model);
        }

    }
}
