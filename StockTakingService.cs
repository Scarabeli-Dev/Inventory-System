using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class StockTakingService : GeralService, IStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsStockTakingService _addressingsStockTakingService;

        public StockTakingService(InventoryContext context,
                                  IInventoryStartService inventoryStartService,
                                  IAddressingsStockTakingService addressingsStockTakingService) : base(context)
        {
            _context = context;
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public async Task<bool> NewStockTakingAsync(StockTaking stockTaking)
        {
            stockTaking.StockTakingDate = DateTime.Now;

            var inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(stockTaking.AddressingId);

            stockTaking.InventoryStartId = inventory.Id;

            stockTaking.NumberOfCount++;

            _context.StockTaking.Add(stockTaking);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateStockTaking(StockTaking stockTaking)
        {
            stockTaking.StockTakingDate = DateTime.Now;

            stockTaking.NumberOfCount++;

            _context.StockTaking.Update(stockTaking);
            _context.SaveChanges();

            return true;
        }

        public async Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId)
        {
            return await _context.StockTaking.Include(i => i.Item).Include(a => a.Addressing).Include(inv => inv.InventoryStart).FirstOrDefaultAsync(st => st.Id == stockTakingId);
        }

        public async Task<StockTaking> GetStockTakingByItemIdAsync(string itemId)
        {
            return await _context.StockTaking.Include(i => i.Item).Include(a => a.Addressing).Include(inv => inv.InventoryStart).FirstOrDefaultAsync(st => st.ItemId == itemId);
        }

        public async Task<List<StockTaking>> GetAllStockTakingAsync()
        {
            return await _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.Addressing)
                                             .ToListAsync();

        }

        public async Task<PagingList<StockTaking>> GetAllStockTakingByPagingAsync(string filter, int pageindex = 1, string sort = "StockTakingDate")
        {
            var result = _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.Addressing)
                                             .AsNoTracking()
                                             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Item.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Addressing.Name.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "StockTakingDate");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<List<StockTaking>> GetStockTakingByAddressingAsync(int addressingId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.Addressing)
                                             .Include(x => x.InventoryStart)
                                             .Where(a => a.AddressingId == addressingId).ToListAsync();
        }
    }
}
