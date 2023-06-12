using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class StockService : GeralService, IStockService
    {
        private readonly InventoryContext _context;

        public StockService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingList<Stock>> GetAllStocksAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Stock.Include(l => l.Locations)
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

        public async Task<Stock> GetStockByIdAsync(int? id)
        {
            var result = await _context.Stock.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}
