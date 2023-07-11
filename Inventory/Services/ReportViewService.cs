using Inventory.Data;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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

        public async Task<PagingList<StockTakingReport>> ReportWithMovementation(string filter, int pageindex = 1, string sort = "ItemName", int warehouseId = 0, int stockSituation = -1, int addressingSituation = -1)
        {
            var result = _context.ViewStockTakingFinalReport
                                 .AsNoTracking()
                                 .AsQueryable();
            if (warehouseId != 0)
            {
                result = result.Where(w => (w.WarehouseAddressingId == warehouseId) ||
                                            w.WarehouseStocktakingId == warehouseId);
            }

            if (stockSituation != -1)
            {
                StockSituation stockSituationEnum = (StockSituation)stockSituation;

                result = result.Where(w => w.StockSituation == stockSituationEnum);
            }

            if (addressingSituation != -1)
            {
                AddressingSituation addressingSituationEnum = (AddressingSituation)addressingSituation;

                result = result.Where(w => w.AddressingSituation == addressingSituationEnum);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.ItemName.ToLower().Contains(filter.ToLower())) ||
                                           (p.ItemId.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "ItemName");

            foreach (var item in model)
            {
                item.StockTakings = await _stockTakingService.GetAllStockTakingByItemIdAsync(item.ItemId);
                item.Addressings = await _itemAddressingService.GetAllItemAddressingByItemIdsAsync(item.ItemId);


                /*if (item.StockTakings != null)
                {
                    if (item.SystemQuantity == item.QuantityStockTaking)
                    {
                        item.StockSituation = StockSituation.Regular;
                    }
                    else if (item.Divergence > 0)
                    {
                        item.StockSituation = StockSituation.HigherThanRegistered;
                    }
                    else if (item.Divergence < 0)
                    {
                        item.StockSituation = StockSituation.LowerThanRegistered;
                    }
                    else
                    {
                        item.StockSituation = StockSituation.ItemNoCount;
                        item.AddressingSituation = AddressingSituation.ItemNoCount;
                        continue;
                    }
                }
                else
                {
                    item.StockSituation = StockSituation.ItemNoCount;
                    item.AddressingSituation = AddressingSituation.ItemNoCount;
                    continue;
                }

                List<int> stockTakingIdsConference = new List<int>();
                List<int> addressingsIdsConference = new List<int>();

                foreach (var stockTakingConference in item.StockTakings)
                {
                    stockTakingIdsConference.Add(stockTakingConference.AddressingsInventoryStart.AddressingId);
                }

                foreach (var addressingsConference in item.Addressings)
                {
                    addressingsIdsConference.Add(addressingsConference.AddressingId);
                }

                if (addressingsIdsConference.SequenceEqual(stockTakingIdsConference))
                {
                    item.AddressingSituation = AddressingSituation.Regular;
                }
                else if (item.Addressings.Sum(q => q.Quantity) == 0)
                {
                    item.AddressingSituation = AddressingSituation.ItemNoAddressRecord;
                }
                else if (!addressingsIdsConference.SequenceEqual(stockTakingIdsConference))
                {
                    item.AddressingSituation = AddressingSituation.ItemStoredInMoreThanOneAddress;
                }*/
            }

            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<PageList<StockTakingReport>> FinalReport(PageParams pageParams)
        {
            //Create View
            List<StockTakingReport> view = new List<StockTakingReport>();

            foreach (var item in _itemService.Items.ToList())
            {
                StockTakingReport stockTakingReport = new StockTakingReport();

                stockTakingReport.ItemId = item.Id;
                stockTakingReport.ItemName = item.Name;
                stockTakingReport.UnitOfMeasurement = item.UnitOfMeasurement;

                if (item.InventoryMovement != null)
                {
                    var itemMovements = _inventoryMovementService.GetInventoryMovementsByItemId(item.Id);
                    decimal movementIn = 0;
                    decimal movementOut = 0;

                    foreach (var movement in itemMovements)
                    {
                        if (item.StockTaking.Any(d => d.StockTakingDate > movement.MovementDate))
                        {
                            if (movement.MovementeType == Models.Enums.MovementeType.E)
                            {
                                movementIn = movementIn + movement.Amount;
                            }
                            if (movement.MovementeType == Models.Enums.MovementeType.S)
                            {
                                movementOut = movementOut + movement.Amount;
                            }
                        }
                    }
                    stockTakingReport.SystemQuantity = item.Addressings.Sum(q => q.Quantity) + movementIn - movementOut;
                }
                else
                {
                    stockTakingReport.SystemQuantity = item.Addressings.Sum(q => q.Quantity);
                }

                if (item.StockTaking != null)
                {
                    stockTakingReport.QuantityStockTaking = item.StockTaking.Sum(q => q.StockTakingQuantity);
                }
                else
                {
                    stockTakingReport.QuantityStockTaking = 0;
                    stockTakingReport.StockSituation = ViewModels.ViewModelEnums.StockSituation.ItemNoCount;
                }

                stockTakingReport.Divergence = stockTakingReport.SystemQuantity - stockTakingReport.QuantityStockTaking;

                if (stockTakingReport.SystemQuantity == 0 && stockTakingReport.QuantityStockTaking == 0)
                {
                    continue;
                }
                if (stockTakingReport.SystemQuantity == stockTakingReport.QuantityStockTaking)
                {
                    stockTakingReport.StockSituation = ViewModels.ViewModelEnums.StockSituation.Regular;
                }
                if (stockTakingReport.Divergence > 0)
                {
                    stockTakingReport.StockSituation = ViewModels.ViewModelEnums.StockSituation.HigherThanRegistered;
                }
                if (stockTakingReport.Divergence < 0)
                {
                    stockTakingReport.StockSituation = ViewModels.ViewModelEnums.StockSituation.LowerThanRegistered;
                }

                view.Add(stockTakingReport);
            }

            var result = await PageList<StockTakingReport>.ListCreateAsync(view, pageParams.PageNumber, view.Count());

            return result;
            //return view;
        }

        //public async Task<PageList<StockTakingReport>> FinalReport(PageParams pageParams)
        //{
        //    List<StockTakingReport> view = new List<StockTakingReport>();

        //    var inventoryMovement = _inventoryMovementService.GetAllInventoryMovementsAsync();

        //    HashSet<int> stockTakingIdWithMovement = new HashSet<int>();
        //    HashSet<string> itemWithStockTaking = new HashSet<string>();

        //    foreach (var item in inventoryMovement)
        //    {
        //        StockTakingReport stockTakingWithMovement = new StockTakingReport();

        //        //Get Classes
        //        Item baseItem = _itemService.Items.FirstOrDefault(a => a.Addressings.Any(w => w.Addressing.WarehouseId == item.WarehouseId && (w.ItemId == item.ItemId)));
        //        StockTaking stockTackingItem = await _stockTakingService.GetStockTakingByWarehouseAndItemIdAsync(item.WarehouseId, item.ItemId);
        //        ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(baseItem.Id, stockTackingItem.AddressingsInventoryStart.AddressingId);

        //        stockTakingWithMovement.StockTaking.Add(stockTackingItem);

        //        //Add props
        //        stockTakingWithMovement.ItemId = item.ItemId;
        //        stockTakingWithMovement.ItemName = item.Item.Name;
        //        stockTakingWithMovement.QuantityStockTaking = stockTackingItem.StockTakingQuantity;
        //        stockTakingWithMovement.QuantityMovement = item.Amount;
        //        stockTakingWithMovement.MovementeType = item.MovementeType;
        //        stockTakingWithMovement.InitialQuantity = itemAddressing.Quantity;
        //        stockTakingWithMovement.UnitOfMeasurement = baseItem.UnitOfMeasurement;

        //        if (item.MovementDate < stockTackingItem.StockTakingDate)
        //        {

        //            if (item.MovementeType == Models.Enums.MovementeType.E)
        //            {
        //                stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement + stockTakingWithMovement.InitialQuantity;
        //            }
        //            else
        //            {
        //                stockTakingWithMovement.QuantityClosed = stockTakingWithMovement.QuantityMovement - stockTakingWithMovement.InitialQuantity;
        //            }
        //        }
        //        else
        //        {
        //            stockTakingWithMovement.QuantityClosed = stockTackingItem.StockTakingQuantity;
        //        }
        //        view.Add(stockTakingWithMovement);
        //        stockTakingIdWithMovement.Add(stockTackingItem.Id);
        //    }
        //    var allStockTaking = await _stockTakingService.GetAllStockTakingAsync();

        //    foreach (var item in allStockTaking)
        //    {
        //        itemWithStockTaking.Add(item.ItemId);
        //        if (stockTakingIdWithMovement.Contains(item.Id))
        //        {
        //            continue;
        //        }
        //        StockTakingReport stockTakingReport = new StockTakingReport();
        //        ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(item.ItemId, item.AddressingsInventoryStart.AddressingId);

        //        stockTakingReport.StockTaking.Add(item);


        //        stockTakingReport.ItemId = item.ItemId;
        //        stockTakingReport.ItemName = item.Item.Name;
        //        stockTakingReport.QuantityStockTaking = item.StockTakingQuantity;
        //        stockTakingReport.QuantityMovement = 0;
        //        stockTakingReport.MovementeType = Models.Enums.MovementeType.SM;
        //        stockTakingReport.QuantityClosed = item.StockTakingQuantity;
        //        stockTakingReport.UnitOfMeasurement = item.Item.UnitOfMeasurement;

        //        if (itemAddressing == null)
        //        {
        //            stockTakingReport.InitialQuantity = 0;
        //        }
        //        else
        //        {
        //            stockTakingReport.InitialQuantity = itemAddressing.Quantity;

        //        }
        //        view.Add(stockTakingReport);
        //    }

        //    var allItems = _itemService.Items.ToList();

        //    //foreach (var item in _itemService.Items.ToList())
        //    //{
        //    //    if (itemWithStockTaking.Contains(item.Id))
        //    //    {
        //    //        continue;
        //    //    }

        //    //    StockTakingReport stockTakingReport = new StockTakingReport();

        //    //    stockTakingReport.ItemId = item.Id;
        //    //    stockTakingReport.ItemName = item.Name;
        //    //    stockTakingReport.QuantityStockTaking = 0;
        //    //    stockTakingReport.QuantityMovement = 0;
        //    //    stockTakingReport.MovementeType = Models.Enums.MovementeType.SM;
        //    //    stockTakingReport.QuantityClosed = item.Addressings.Sum(t => t.Quantity);
        //    //    stockTakingReport.UnitOfMeasurement = item.UnitOfMeasurement;
        //    //    stockTakingReport.InitialQuantity = item.Addressings.Sum(t => t.Quantity);

        //    //    view.Add(stockTakingReport);
        //    //}

        //    var result = await PageList<StockTakingReport>.ListCreateAsync(view, pageParams.PageNumber, view.Count());

        //    return result;
        //    //return view;
        //}
    }
}