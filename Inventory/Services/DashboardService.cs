using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.DashboardViewModels;
using Inventory.ViewModels.ViewModelEnums;

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

        public ChartsViewModel GaugeGrade()
        {
            ChartsViewModel model = new ChartsViewModel();

            var allItems = _itemService.Items.ToList();

            foreach (var item in allItems)
            {
                if (item.StockTaking.Count() > 0)
                {
                    model.ItemsWithStockTakingAmount++;

                    if (item.StockTaking.Sum(q => q.StockTakingQuantity) == item.Addressings.Sum(q => q.Quantity))
                    {
                        model.ItemsWithCorrectAmount++;
                    }
                }

            }
            return model;
        }
    }
}
