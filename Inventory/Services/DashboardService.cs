using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.DashboardViewModels;

namespace Inventory.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemAddressingService _itemAddressingService;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IAddressingsInventoryStartService _addressingsInventoryStartService;

        public DashboardService(IStockTakingService stockTakingService,
                                IItemAddressingService itemAddressingService,
                                IItemService itemService,
                                IAddressingService addressingService,
                                IAddressingsInventoryStartService addressingsInventoryStartService)
        {
            _stockTakingService = stockTakingService;
            _itemAddressingService = itemAddressingService;
            _itemService = itemService;
            _addressingService = addressingService;
            _addressingsInventoryStartService = addressingsInventoryStartService;
        }

        public ChartsViewModel DashboardData(int warehouseId)
        {
            ChartsViewModel model = new ChartsViewModel();
            var allItems = new List<Item>();

            if (warehouseId == 0)
            {
                allItems = _itemService.Items.ToList();
            }
            else
            {
                allItems = _itemService.Items.Where(w => w.Addressings.Any(w => w.Addressing.WarehouseId == warehouseId)).ToList();
            }

            model.TotalOfItems = allItems.Count();

            foreach (var item in allItems)
            {
                if (item.StockTaking.Count() > 0)
                {
                    model.TotalQuantityItemsStockTaking += item.StockTaking.Sum(q => q.StockTakingQuantity);
                    model.ItemsWithStockTakingAmount++;

                    if (item.StockTaking.Sum(q => q.StockTakingQuantity) == item.Addressings.Sum(q => q.Quantity))
                    {
                        model.ItemsWithCorrectAmount++;
                    }

                    foreach (var stockTakingConference in item.StockTaking)
                    {
                        if (stockTakingConference.PerishableItem.Count() > 0)
                        {
                            model.ItemPerishableAmount += stockTakingConference.PerishableItem.Sum(q => q.PerishableItemQuantity);

                            foreach (var perishableItem in stockTakingConference.PerishableItem)
                            {
                                if (perishableItem.ExpirationDate < DateTime.Now)
                                {
                                    model.ItemPerishableExpirateDate += perishableItem.PerishableItemQuantity;
                                }
                            }
                        }
                    }

                    var stockTakingIdsConference = item.StockTaking.Select(stockTaking => stockTaking.AddressingsInventoryStart.AddressingId);
                    var addressingsIdsConference = item.Addressings.Select(addressings => addressings.AddressingId);

                    if (addressingsIdsConference.SequenceEqual(stockTakingIdsConference))
                    {
                        model.ItemsWithAddressingRigth++;
                    }
                }
            }

            // Cálculos para as propriedades adicionais
            model.GaugeValueStockTaking = (decimal)model.ItemsWithCorrectAmount / model.ItemsWithStockTakingAmount;
            model.GaugeValueAddressing = (decimal)model.ItemsWithAddressingRigth / model.ItemsWithStockTakingAmount;
            model.GaugeStockDivergence = 1 - model.GaugeValueStockTaking;
            model.GaugeValueLostDate = model.ItemPerishableAmount != 0 ? model.ItemPerishableExpirateDate / model.ItemPerishableAmount : 0;
            model.RadarUserOfInventory = 1 - model.GaugeValueLostDate;
            model.RadarEffectivenessOfInventoryManagement = ((double)model.GaugeValueStockTaking * 0.35) + ((double)model.RadarUserOfInventory * 0.45) + ((double)model.GaugeValueAddressing * 0.2);

            return model;
        }

    }
}
