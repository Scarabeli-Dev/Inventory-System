using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

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
    }
}
