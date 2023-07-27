using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

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

        public async Task<PagingList<PerishableItem>> GetAllPerishableItemsPagingListAsync(string filter, int pageindex = 1, string sortExpression = "ExpirationDate", int warehouseId = 0, bool nullExpirationDate = false, DateTime? expirationDate = null)
        {
            var result = _context.PerishableItem.Include(s => s.StockTaking).ThenInclude(i => i.Item)
                                                .Include(s => s.StockTaking).ThenInclude(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing).ThenInclude(w => w.Warehouse)
                                                .AsNoTracking().AsQueryable();

            if (warehouseId != 0)
            {
                result = result.Where(w => w.StockTaking.AddressingsInventoryStart.Addressing.WarehouseId == warehouseId);
            }

            if (expirationDate != null)
            {
                result = result.Where(e => e.ExpirationDate <= expirationDate);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.StockTaking.Item.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.StockTaking.AddressingsInventoryStart.Addressing.Name.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sortExpression, "ExpirationDate");
            model.Action = "PerishableItems";
            model.RouteValue = new RouteValueDictionary { { "filter", filter }, { "expirationDate", expirationDate }, { "warehouseId", warehouseId } };

            return model;
        }
    }
}
