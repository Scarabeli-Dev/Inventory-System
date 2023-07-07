using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class ReportViewService : IReportViewService
    {
        private readonly InventoryContext _context;
        private readonly IInventoryMovementService _inventoryMovementService;
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IItemAddressingService _itemAddressingService;

        public ReportViewService(IInventoryMovementService inventoryMovementService,
                                 IStockTakingService stockTakingService,
                                 IItemService itemService,
                                 IItemAddressingService itemAddressingService,
                                 InventoryContext context)
        {
            _inventoryMovementService = inventoryMovementService;
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _itemAddressingService = itemAddressingService;
            _context = context;
        }

        public async Task<PagingList<StockTakingReport>> ReportWithMovementation(string filter, int pageindex = 1, string sort = "ItemName")
        {
            var result = _context.StockTakingReportView
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.ItemName.ToLower().Contains(filter.ToLower())) ||
                                           (p.ItemId.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "ItemName");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        private async Task<List<StockTakingReport>> CreateViewModel()
        {
            List<StockTakingReport> view = new List<StockTakingReport>();

            var inventoryMovement = _inventoryMovementService.GetAllInventoryMovementsAsync();

            HashSet<int> stockTakingIdWithMovement = new HashSet<int>();
            HashSet<string> itemWithStockTaking = new HashSet<string>();

            foreach (var item in inventoryMovement)
            {
                StockTakingReport stockTakingWithMovement = new StockTakingReport();

                //Get Classes
                Item baseItem = _itemService.Items.FirstOrDefault(a => a.Addressings.Any(w => w.Addressing.WarehouseId == item.WarehouseId && (w.ItemId == item.ItemId)));
                StockTaking stockTackingItem = await _stockTakingService.GetStockTakingByWarehouseAndItemIdAsync(item.WarehouseId, item.ItemId);
                ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(baseItem.Id, stockTackingItem.AddressingsInventoryStart.AddressingId);

                //Add props
                stockTakingWithMovement.ItemId = item.ItemId;
                stockTakingWithMovement.ItemName = item.Item.Name;
                stockTakingWithMovement.QuantityStockTaking = stockTackingItem.StockTakingQuantity;
                stockTakingWithMovement.QuantityMovement = item.Amount;
                stockTakingWithMovement.MovementeType = item.MovementeType;
                stockTakingWithMovement.InitialQuantity = itemAddressing.Quantity;
                stockTakingWithMovement.UnitOfMeasurement = baseItem.UnitOfMeasurement;

                if (item.MovementDate < stockTackingItem.StockTakingDate)
                {

                    if (item.MovementeType == Models.Enums.MovementeType.E)
                    {
                        stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement + stockTakingWithMovement.InitialQuantity;
                    }
                    else
                    {
                        stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement - stockTakingWithMovement.InitialQuantity;
                    }
                }
                else
                {
                    stockTakingWithMovement.QuantityClosed = stockTackingItem.StockTakingQuantity;
                }
                view.Add(stockTakingWithMovement);
                stockTakingIdWithMovement.Add(stockTackingItem.Id);
            }
            var allStockTaking = await _stockTakingService.GetAllStockTakingAsync();

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
                stockTakingReport.QuantityStockTaking = item.StockTakingQuantity;
                stockTakingReport.QuantityMovement = 0;
                stockTakingReport.MovementeType = Models.Enums.MovementeType.SM;
                stockTakingReport.QuantityClosed = item.StockTakingQuantity;
                stockTakingReport.UnitOfMeasurement = item.Item.UnitOfMeasurement;

                if (itemAddressing == null)
                {
                    stockTakingReport.InitialQuantity = 0;
                }
                else
                {
                    stockTakingReport.InitialQuantity = itemAddressing.Quantity;

                }

                view.Add(stockTakingReport);
            }

            return view;
        }
    }
}