using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class InventoryStartService : GeralService, IInventoryStartService
    {
        private readonly InventoryContext _context;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;
        private readonly IAddressingService _addressingService;

        public InventoryStartService(InventoryContext context, IAddressingsInventoryStartService addressingsStockTakingService, IAddressingService addressingService) : base(context)
        {
            _context = context;
            _addressingsStockTakingService = addressingsStockTakingService;
            _addressingService = addressingService;
        }

        public async Task CreateInventoryStartAsync(InventoryStart inventoryStart)
        {
            var result = await _context.InventoryStart.AddAsync(inventoryStart);
            await _context.SaveChangesAsync();
            await _addressingsStockTakingService.CreateAddressingsStockTakingAsync(result.Entity.Id, inventoryStart.WarehouseId);
        }

        public async Task<PagingList<InventoryStart>> GetAllInventoryStartsAsync(string filter, int pageindex = 1, string sort = "InventoryStartDate")
        {
            var result = _context.InventoryStart.Include(l => l.Addressings)
                                      .ThenInclude(l => l.Addressing)
                                      .Include(w => w.Warehouse)
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Addressings.Any(a => a.Addressing.Name.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "InventoryStartDate");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<InventoryStart> GetInventoryStartByIdAsync(int id)
        {
            return await _context.InventoryStart.Include(l => l.Addressings).ThenInclude(il => il.Addressing).Include(w => w.Warehouse).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<InventoryStart> GetInventoryStartByAddressingAsync(int addressingId)
        {
            var addressing = await _addressingService.GetAddressingByIdAsync(addressingId);

            var inventoryStart = await _context.InventoryStart.Include(l => l.Addressings)
                                                              .ThenInclude(il => il.Addressing)
                                                              .ThenInclude(s => s.StockTaking)
                                                              .Include(w => w.Warehouse)
                                                              .FirstOrDefaultAsync(i => i.IsCompleted != true);
            return inventoryStart;
        }
    }
}
