using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class ItemService : GeralService, IItemService
    {
        private readonly InventoryContext _context;

        public ItemService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingList<Item>> GetAllItemsAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Location)
                                      .ThenInclude(l => l.Location)
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Name.ToLower().Contains(filter.ToLower()));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<PagingList<Item>> GetItemsByLocationAsync(int stockId, int locationId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Location)
                                      .ThenInclude(l => l.Location)
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Name.ToLower().Contains(filter.ToLower()));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }
    }
}
