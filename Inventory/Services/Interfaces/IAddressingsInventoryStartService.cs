using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingsInventoryStartService : IGeralService
    {
        Task<PagingList<AddressingsInventoryStart>> GetAllAddressingsStockTakingsByPageList(int inventaryStartId, string filter, int pageindex = 1, string sort = "Addressing");
        Task CreateAddressingsStockTakingAsync(int inventoryStartId);
        Task<AddressingsInventoryStart> GetAddressingsStockTakingAddressingByIdAsync(int addressingId);
        Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId);
    }
}
