using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class ItemAddressingService : IItemAddressingService
    {
        private readonly InventoryContext _context;

        public ItemAddressingService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<ItemsAddressings> GetItemAddressingByIdsAsync(string itemId, int addressingId)
        {
            return await _context.ItemsAddressing.Include(i => i.Item)
                                                 .Include(a => a.Addressing)
                                                 .FirstOrDefaultAsync(i => (i.ItemId == itemId) &&
                                                                           (i.AddressingId == addressingId));
        }

        public async Task<List<ItemsAddressings>> GetAllItemAddressingByItemIdsAsync(string itemId)
        {
            return await _context.ItemsAddressing.Include(i => i.Item)
                                                 .Include(a => a.Addressing)
                                                 .ThenInclude(w => w.Warehouse)
                                                 .Where(i => i.ItemId == itemId)
                                                .ToListAsync();
        }
    }
}
