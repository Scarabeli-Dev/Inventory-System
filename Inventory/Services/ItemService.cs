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
            var result = _context.Item.Include(l => l.Addressing)
                                      .ThenInclude(l => l.Addressing)
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Id.ToString().Contains(filter)));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<PagingList<Item>> GetItemsByAddressingAsync(int locationId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Addressing)
                                      .ThenInclude(l => l.Addressing)
                                      .Where(l => l.Addressing.Any(il => il.AddressingId == locationId))
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Id.ToString().Contains(filter)));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
           var result = await _context.Item.Include(l => l.Addressing).ThenInclude(il => il.Addressing).FirstOrDefaultAsync(m => m.Id == id);

           return result;
        }
    }
}
