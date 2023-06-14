using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Services
{
    public class AddressingsStockTakingService : GeralService, IAddressingsStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IAddressingService _addressingService;

        public AddressingsStockTakingService(InventoryContext context, IAddressingService addressingService) : base(context)
        {
            _context = context;
            _addressingService = addressingService;
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
    }
}
