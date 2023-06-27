using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class ItemsStockTakingService
    {
        //private readonly InventoryContext _context;
        //private readonly IItemService _itemService;

        //public ItemsStockTakingService(InventoryContext context, IItemService itemService) : base(context)
        //{
        //    _context = context;
        //    _itemService = itemService;
        //}

        //public async Task CreateItemsStockTakingAsync(int inventoryStartId)
        //{
        //    var items = await _itemService.GetAllAsync<Item>();


        //    foreach (var item in items)
        //    {
        //        ItemsStockTaking itemsStockTaking = new ItemsStockTaking();

        //        itemsStockTaking.InventoryStartId = inventoryStartId;
        //        itemsStockTaking.ItemId = item.Id;
        //        itemsStockTaking.ItemCountEnded = false;
        //        itemsStockTaking.ItemCountRealized = false;


        //        _context.Add(itemsStockTaking);
        //        _context.SaveChanges();
        //    }
        //}

        //public async Task<List<ItemsStockTaking>> GetItemsStockTakingItemByAddressingIdAsync(int addressingId)
        //{
        //    return await _context.ItemsStockTaking.Include(x => x.Item)
        //                                          .ThenInclude(a => a.Addressings)
        //                                          .ThenInclude(a => a.Addressing)
        //                                          .Include(x => x.InventoryStart)
        //                                          .Where(a => a.Item.Addressings.Any(a => a.Addressing.Id == addressingId)).ToListAsync();
        //}

        //public async Task<ItemsStockTaking> GetItemsStockTakingItemByIdAsync(string itemId)
        //{
        //    return await _context.ItemsStockTaking.Include(x => x.Item)
        //                                          .Include(x => x.InventoryStart)
        //                                          .FirstOrDefaultAsync(a => a.ItemId == itemId);
        //}

        //public async Task<bool> SetItemCountRealizedTrueAsync(string itemId, int stockTakingId)
        //{
        //    var itemsStockTaking = await GetItemsStockTakingItemByIdAsync(itemId);

        //    itemsStockTaking.ItemCountRealized = true;

        //    _context.Update(itemsStockTaking);
        //    _context.SaveChanges();

        //    return true;
        //}
    }
}
