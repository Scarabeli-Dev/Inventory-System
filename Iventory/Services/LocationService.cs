using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class LocationService : GeralService, ILocationService
    {
        private readonly InventoryContext _context;

        public LocationService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Location> AddLocationAsync(Location location)
        {

            _context.Add(location);

            await _context.SaveChangesAsync();

            return location;

        }

        public async Task<PagingList<Location>> GetLocationsByStockIdAsync(int stockId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Location.Include(l => l.Item)
                                          .Where(s => s.StockId == stockId)
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
