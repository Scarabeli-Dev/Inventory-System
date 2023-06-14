using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Services
{
    public class StockTakingService : GeralService, IStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IItemService _itemService;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsStockTakingService _addressingsStockTakingService;

        public StockTakingService(InventoryContext context, IItemService itemService,
                                  IInventoryStartService inventoryStartService,
                                  IAddressingsStockTakingService addressingsStockTakingService) : base(context)
        {
            _context = context;
            _itemService = itemService;
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public async Task<bool> NewStockTakingAsync(StockTaking stockTaking, Item item)
        {
            stockTaking.StockTakingDate = DateTime.Now;
            stockTaking.ItemId = item.Id;

            var inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(stockTaking.AddressingId);

            stockTaking.InventoryStartId = inventory.Id;

            _context.StockTaking.Add(stockTaking);
            _context.SaveChanges();

            await _addressingsStockTakingService.SetAddressingCountRealizedTrueAsync(inventory.Id);


            return true;
        }
    }
}
