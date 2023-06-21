using Inventory.Data;
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

        public async Task<PagingList<AddressingsInventoryStart>> GetAllAddressingsStockTakingsByPageList(int inventaryStartId, string filter, int pageindex = 1, string sort = "Id")
        {
            var result = _context.AddressingsInventoryStart.Include(l => l.Addressing)
                                                        .Where(s => s.InventoryStartId == inventaryStartId)
                                                        .OrderBy(l => l.AddressingCountRealized)
                                                        .ThenBy(l => l.AddressingCountEnded)
                                                        .AsNoTracking()
                                                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Addressing.Name.ToLower().Contains(filter.ToLower()));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Id");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task CreateAddressingsStockTakingAsync(int inventoryStartId)
        {
            var addressings = await _addressingService.GetAllAsync<Addressing>();


            foreach (var item in addressings)
            {
                AddressingsInventoryStart addressingsStockTaking = new AddressingsInventoryStart();

                addressingsStockTaking.InventoryStartId = inventoryStartId;
                addressingsStockTaking.AddressingId = item.Id;
                addressingsStockTaking.AddressingCountEnded = false;
                addressingsStockTaking.AddressingCountRealized = false;


                _context.Add(addressingsStockTaking);
                _context.SaveChanges();
            }
        }

        public async Task<AddressingsInventoryStart> GetAddressingsStockTakingAddressingByIdAsync(int addressingId)
        {
            return await _context.AddressingsInventoryStart.Include(x => x.Addressing)
                                                        .Include(x => x.InventoryStart)
                                                        .FirstOrDefaultAsync(a => a.AddressingId == addressingId);
        }

        public async Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId)
        {
            var addressingsStockTaking = await GetAddressingsStockTakingAddressingByIdAsync(addressingId);

            int itemsCount = _context.AddressingsInventoryStart.Include(s => s.StockTaking).Count();

            int itemsInAddressing = 0;

            if (itemsCount == itemsInAddressing)
            {
                addressingsStockTaking.AddressingCountRealized = true;

                _context.Update(addressingsStockTaking);
                _context.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
