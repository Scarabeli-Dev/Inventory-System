using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services
{
    public class AddressingsStockTakingService : GeralService, IAddressingsStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IAddressingService _addressingService;
        private readonly IItemsStockTakingService _itemsStockTakingService;

        public AddressingsStockTakingService(InventoryContext context, IAddressingService addressingService, IItemsStockTakingService itemsStockTakingService) : base(context)
        {
            _context = context;
            _addressingService = addressingService;
            _itemsStockTakingService = itemsStockTakingService;
        }

        public async Task<PagingList<AddressingsStockTaking>> GetAllAddressingsStockTakingsByPageList(int inventaryStartId, string filter, int pageindex = 1, string sort = "Id")
        {
            var result = _context.AddressingsStockTaking.Include(l => l.Addressing)
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
                AddressingsStockTaking addressingsStockTaking = new AddressingsStockTaking();

                addressingsStockTaking.InventoryStartId = inventoryStartId;
                addressingsStockTaking.AddressingId = item.Id;
                addressingsStockTaking.AddressingCountEnded = false;
                addressingsStockTaking.AddressingCountRealized = false;


                _context.Add(addressingsStockTaking);
                _context.SaveChanges();
            }
        }

        public async Task<AddressingsStockTaking> GetAddressingsStockTakingAddressingByIdAsync(int addressingId)
        {
            return await _context.AddressingsStockTaking.Include(x => x.Addressing)
                                                        .Include(x => x.InventoryStart)
                                                        .FirstOrDefaultAsync(a => a.AddressingId == addressingId);
        }

        public async Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId)
        {
            var addressingsStockTaking = await GetAddressingsStockTakingAddressingByIdAsync(addressingId);
            var stockTakingItems = await _itemsStockTakingService.GetItemsStockTakingItemByAddressingIdAsync(addressingId);

            int itemsCount = 0;
            int itemsInAddressing = 0;

            foreach (var item in stockTakingItems)
            {
                if (item.ItemCountRealized == true)
                {
                    itemsCount++;
                }
            }

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
