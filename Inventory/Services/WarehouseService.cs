using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class WarehouseService : GeralService, IWarehouseService
    {
        private readonly InventoryContext _context;

        public WarehouseService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingList<Warehouse>> GetAllWarehousesAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Warehouse.Include(l => l.Addressings)
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

        public async Task<Warehouse> GetWarehouseByIdAsync(int? id)
        {
            var result = await _context.Warehouse.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}
