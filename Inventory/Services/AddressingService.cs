using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class AddressingService : GeralService, IAddressingService
    {
        private readonly InventoryContext _context;

        public AddressingService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Addressing> AddAddressingAsync(Addressing addressing)
        {

            _context.Add(addressing);

            await _context.SaveChangesAsync();

            return addressing;

        }

        public async Task<PagingList<Addressing>> GetAddressingsByWarehouseIdAsync(int stockId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Addressing.Include(l => l.Item)
                                          .ThenInclude(i => i.Item)
                                          .Where(s => s.WarehouseId == stockId)
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

        public async Task<Addressing> GetAddressingByIdAsync(int id)
        {
            var result = await _context.Addressing.Include(l => l.Item).ThenInclude(il => il.Item).FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }
    }
}
