using Inventory.Data;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class AddressingsInventoryStartService : GeralService, IAddressingsInventoryStartService
    {
        private readonly InventoryContext _context;
        private readonly IAddressingService _addressingService;

        public AddressingsInventoryStartService(InventoryContext context,
                                                IAddressingService addressingService) : base(context)
        {
            _context = context;
            _addressingService = addressingService;
        }

        public IEnumerable<AddressingsInventoryStart> AddressingsInventoryStarts => _context.AddressingsInventoryStart
                                                                                            .Include(l => l.Addressing)
                                                                                            .ThenInclude(i => i.Item)
                                                                                            .Include(s => s.StockTaking)
                                                                                            .ThenInclude(i => i.Item);

        public async Task<PageList<AddressingsInventoryStart>> GetAllPageListDataTable(PageParams pageParams)
        {
            IQueryable<AddressingsInventoryStart> query = _context.AddressingsInventoryStart
                                                                  .Include(l => l.Addressing)
                                                                  .ThenInclude(i => i.Item)
                                                                  .ThenInclude(i => i.Item)
                                                                  .Include(s => s.StockTaking)
                                                                  .OrderByDescending(p => p.AddressingCountRealized)
                                                                  .ThenBy(p => p.AddressingCountEnded)
                                                                  .ThenBy(p => p.Addressing.Name);

            query = query.AsNoTracking()
                         .Where(a => a.Addressing.Name.ToLower().Contains(pageParams.Term.ToLower()));

            return await PageList<AddressingsInventoryStart>.CreateAsync(query, pageParams.PageNumber, query.Count());
        }

        public async Task<PagingList<AddressingsInventoryStart>> GetAddressingsStockTakingsPagingAsync(string filter, int pageindex = 1, string sort = "AddressingCountRealized", int warehouseId = 0, int countStatus = 0)
        {

            var result = _context.AddressingsInventoryStart
                                 .Include(l => l.Addressing).ThenInclude(i => i.Item)
                                 .Include(s => s.StockTaking).ThenInclude(p => p.PerishableItem)
                                 .Include(i => i.InventoryStart).ThenInclude(w => w.Warehouse)
                                 .AsNoTracking()
                                 .AsQueryable();

            if (countStatus == 1)
            {
                result = result.Where(p => p.AddressingCountRealized == false);
            }
            if (countStatus == 2)
            {
                result = result.Where(p => p.AddressingCountRealized == true && p.AddressingCountEnded == false);
            }
            if (countStatus == 3)
            {
                result = result.Where(p => p.AddressingCountEnded == true);
            }

            if (warehouseId > 0)
            {
                result = result.Where(w => w.Addressing.WarehouseId == warehouseId);
            }

            if (!string.IsNullOrWhiteSpace(filter))
            {
                if ("Contado".ToLower().Contains(filter.ToLower()))
                {
                    result = result.Where(p => p.AddressingCountRealized == true && p.AddressingCountEnded == false);
                }
                else if ("Finalizado".ToLower().Contains(filter.ToLower()))
                {
                    result = result.Where(p => p.AddressingCountEnded == true);
                }
                else
                {
                    result = result.Where(p => p.Addressing.Name.ToLower().Contains(filter.ToLower()));
                }
            }

            if (sort == "AddressingCountRealized")
            {
                result = result.OrderByDescending(p => p.AddressingCountEnded)
                               .ThenByDescending(p => p.AddressingCountRealized)
                               .ThenBy(p => p.Addressing.Name);
            }
            else
            {
                result = result.OrderBy(p =>
                    p.AddressingCountRealized ? 0 :
                    p.AddressingCountEnded ? 2 :
                    1);
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "AddressingCountRealized");
            model.RouteValue = new RouteValueDictionary { { "filter", filter}, { "warehouseId", warehouseId }, { "countStatus", countStatus } };

            return model;
        }

        public async Task<PagingList<AddressingsInventoryStart>> GetAddressingsStockTakingsByInventoryPagingAsync(int inventaryStartId, string filter, int pageindex = 1, string sort = "AddressingCountRealized")
        {
            var result = _context.AddressingsInventoryStart
                .Include(l => l.Addressing)
                .ThenInclude(i => i.Item)
                .Include(s => s.StockTaking)
                .Where(s => s.InventoryStartId == inventaryStartId)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                if ("Contado".ToLower().Contains(filter.ToLower()))
                {
                    result = result.Where(p => p.AddressingCountRealized == true);
                }
                else if ("Finalizado".ToLower().Contains(filter.ToLower()))
                {
                    result = result.Where(p => p.AddressingCountEnded == true);
                }
                else
                {
                    result = result.Where(p => p.Addressing.Name.ToLower().Contains(filter.ToLower()));
                }
            }

            if (sort == "AddressingCountRealized")
            {
                result = result.OrderByDescending(p => p.AddressingCountEnded)
                               .ThenByDescending(p => p.AddressingCountRealized)
                               .ThenBy(p => p.Addressing.Name);
            }
            else
            {
                result = result.OrderBy(p =>
                    p.AddressingCountRealized ? 0 :
                    p.AddressingCountEnded ? 2 :
                    1);
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "AddressingCountRealized");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task CreateAddressingsStockTakingAsync(int inventoryStartId, int warehouseId)
        {
            var addressings = await _addressingService.GetAllAddressingsByWarehouseIdAsync(warehouseId);

            List<AddressingsInventoryStart> addressingsStockTakingRange = new List<AddressingsInventoryStart>();

            foreach (var item in addressings)
            {
                AddressingsInventoryStart addressingsStockTaking = new AddressingsInventoryStart();

                addressingsStockTaking.InventoryStartId = inventoryStartId;
                addressingsStockTaking.AddressingId = item.Id;
                addressingsStockTaking.AddressingCountEnded = false;
                addressingsStockTaking.AddressingCountRealized = false;

                addressingsStockTakingRange.Add(addressingsStockTaking);

            }
            _context.AddRange(addressingsStockTakingRange);
            _context.SaveChanges();
        }

        public async Task<AddressingsInventoryStart> GetAddressingsStockTakingByAddressingIdAsync(int addressingId)
        {
            return await _context.AddressingsInventoryStart.Include(x => x.Addressing)
                                                        .ThenInclude(i => i.Item)
                                                        .Include(x => x.InventoryStart)
                                                        .AsNoTracking()
                                                        .FirstOrDefaultAsync(a => a.AddressingId == addressingId);
        }

        public async Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId)
        {
            var addressingsStockTaking = await GetAddressingsStockTakingByAddressingIdAsync(addressingId);

            int itemsCount = _context.AddressingsInventoryStart.Where(s => s.Id == addressingsStockTaking.Id)
                                                               .SelectMany(s => s.StockTaking)
                                                               .Count();

            int itemsInAddressing = _context.AddressingsInventoryStart.Where(s => s.AddressingId == addressingId)
                                                               .SelectMany(s => s.Addressing.Item)
                                                               .Count();

            if (itemsCount >= itemsInAddressing)
            {
                addressingsStockTaking.AddressingCountRealized = true;

                _context.Update(addressingsStockTaking);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> SetAddressingCountEndedTrueAsync(int addressingId)
        {
            var addressingsStockTaking = await _context.AddressingsInventoryStart.FirstOrDefaultAsync(i => i.Id == addressingId);
            if (addressingsStockTaking != null)
            {
                addressingsStockTaking.AddressingCountEnded = true;
                _context.Update(addressingsStockTaking);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> SetAddressingCountEndedFalseAsync(int addressingId)
        {
            var addressingsStockTaking = await _context.AddressingsInventoryStart.FirstOrDefaultAsync(i => i.Id == addressingId);
            if (addressingsStockTaking != null)
            {
                addressingsStockTaking.AddressingCountEnded = false;
                _context.Update(addressingsStockTaking);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
