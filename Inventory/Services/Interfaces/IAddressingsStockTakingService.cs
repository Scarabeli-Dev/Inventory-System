using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingsStockTakingService : IGeralService
    {
        Task<PagingList<AddressingsStockTaking>> GetAllAddressingsStockTakingsByPageList(int inventaryStartId, string filter, int pageindex = 1, string sort = "Addressing");
        Task CreateAddressingsStockTakingAsync(int inventoryStartId);
        Task<AddressingsStockTaking> GetAddressingsStockTakingAddressingByIdAsync(int addressingId);
        Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId);
    }
}
