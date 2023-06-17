using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Inventory.Services
{
    public class StockTakingService : GeralService, IStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IItemService _itemService;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsStockTakingService _addressingsStockTakingService;
        private readonly IItemsStockTakingService _itemsStockTakingService;

        public StockTakingService(InventoryContext context, IItemService itemService,
                                  IInventoryStartService inventoryStartService,
                                  IAddressingsStockTakingService addressingsStockTakingService,
                                  IItemsStockTakingService itemsStockTakingService) : base(context)
        {
            _context = context;
            _itemService = itemService;
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
            _itemsStockTakingService = itemsStockTakingService;
        }

        public async Task<bool> NewStockTakingAsync(StockTaking stockTaking)
        {
            stockTaking.StockTakingDate = DateTime.Now;

            var inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(stockTaking.AddressingId);

            stockTaking.InventoryStartId = inventory.Id;

            stockTaking.NumberOfCount ++;

            _context.StockTaking.Add(stockTaking);
            _context.SaveChanges();


            await _addressingsStockTakingService.SetAddressingCountRealizedTrueAsync(stockTaking.AddressingId);
            await _itemsStockTakingService.SetItemCountRealizedTrueAsync(stockTaking.ItemId, stockTaking.Id);


            return true;
        }

        public bool UpdateStockTaking(StockTaking stockTaking)
        {
            stockTaking.StockTakingDate = DateTime.Now;

            stockTaking.NumberOfCount ++;

            _context.StockTaking.Update(stockTaking);
            _context.SaveChanges();

            return true;
        }

        public async Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId)
        {
            return await _context.StockTaking.Include(i => i.Item).Include(a => a.Addressing).Include(inv => inv.InventoryStart).FirstOrDefaultAsync(st => st.Id == stockTakingId);
        }

        public async Task<StockTaking> GetStockTakingByItemIdAsync(string itemId)
        {
            return await _context.StockTaking.Include(i => i.Item).Include(a => a.Addressing).Include(inv => inv.InventoryStart).FirstOrDefaultAsync(st => st.ItemId == itemId);
        }
    }
}
