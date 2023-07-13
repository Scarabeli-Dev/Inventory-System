using Inventory.Helpers;
using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingsInventoryStartService : IGeralService
    {
        IEnumerable<AddressingsInventoryStart> AddressingsInventoryStarts { get; }
        Task<PageList<AddressingsInventoryStart>> GetAllPageListDataTable(PageParams pageParams);
        Task<PagingList<AddressingsInventoryStart>> GetAddressingsStockTakingsPagingAsync(string filter, int pageindex = 1, string sort = "Id");
        Task<PagingList<AddressingsInventoryStart>> GetAddressingsStockTakingsByInventoryPagingAsync(int inventaryStartId, string filter, int pageindex = 1, string sort = "Id");
        Task CreateAddressingsStockTakingAsync(int inventoryStartId, int warehouseId);
        Task<AddressingsInventoryStart> GetAddressingsStockTakingAddressingByIdAsync(int addressingId);
        Task<bool> SetAddressingCountRealizedTrueAsync(int addressingId);
        Task<bool> SetAddressingCountEndedTrueAsync(int addressingId);
    }
}
