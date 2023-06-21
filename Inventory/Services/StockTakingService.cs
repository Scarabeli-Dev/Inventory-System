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
        private readonly IItemService _itemService;
        private readonly IInventoryStartService _inventoryStartService;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;

        public StockTakingService(InventoryContext context,
                                  IItemService itemService,
                                  IInventoryStartService inventoryStartService,
                                  IAddressingsInventoryStartService addressingsStockTakingService) : base(context)
        {
            _context = context;
            _itemService = itemService;
            _inventoryStartService = inventoryStartService;
            _addressingsStockTakingService = addressingsStockTakingService;
        }

        public async Task<bool> NewStockTakingAsync(StockTaking stockTaking)
        {
            stockTaking.StockTakingDate = DateTime.Now;

            var inventory = await _inventoryStartService.GetInventoryStartByAddressingAsync(stockTaking.AddressingsInventoryStartId);

            stockTaking.NumberOfCount++;

            _context.StockTaking.Add(stockTaking);
            _context.SaveChanges();

            await _addressingsStockTakingService.SetAddressingCountRealizedTrueAsync(stockTaking.AddressingsInventoryStartId);

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
            return await _context.StockTaking.Include(i => i.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .FirstOrDefaultAsync(st => st.Id == stockTakingId);
        }

        public async Task<List<StockTaking>> GetAllStockTakingByItemIdAsync(string itemId)
        {
            return await _context.StockTaking.Include(i => i.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Where(st => st.ItemId == itemId).ToListAsync();
        }

        public async Task<List<StockTaking>> GetAllStockTakingAsync()
        {
            return await _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .ToListAsync();

        }

        public async Task<PagingList<StockTaking>> GetStockTakingByPagingAsync(string filter, int pageindex = 1, string sort = "StockTakingDate")
        {
            var result = _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .AsNoTracking()
                                             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Item.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.AddressingsInventoryStart.Addressing.Name.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "StockTakingDate");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<List<StockTaking>> GetStockTakingByAddressingAsync(int addressingId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                             .Where(a => a.AddressingsInventoryStart.AddressingId == addressingId).ToListAsync();
        }

        public async Task<StockTaking> GetStockTakingByAddressingAndItemIdAsync(int addressingId, string itemId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                             .FirstOrDefaultAsync(a => a.AddressingsInventoryStart.AddressingId == addressingId && a.ItemId == itemId);
        }

        public int GetCountStockTakingByAddressingAsync(int addressingId)
        {
            return _context.StockTaking.Include(x => x.Item)
                                       .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                       .Where(a => a.AddressingsInventoryStart.AddressingId == addressingId).Count();
        }
    }
}
