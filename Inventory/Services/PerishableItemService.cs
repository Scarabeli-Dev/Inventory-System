using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class PerishableItemService : GeralService, IPerishableItemService
    {
        private readonly InventoryContext _context;

        public PerishableItemService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public void Create(List<PerishableItem> perishableItem, int stockTakingId)
        {
            foreach (var item in perishableItem)
            {
                item.StockTakingId = stockTakingId;
                _context.PerishableItem.Add(item);
                _context.SaveChanges();
            }
        }

        public async Task<List<PerishableItem>> GetAllByStockTakingId(int stockTakingId)
        {
            return await _context.PerishableItem.Include(s => s.StockTaking)
                                                .Where(si => si.StockTakingId == stockTakingId)
                                                .ToListAsync();
        }

        public async Task<bool> DeletePerishableItemAsync(PerishableItem perishableItem)
        {
            _context.PerishableItem.Remove(perishableItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
