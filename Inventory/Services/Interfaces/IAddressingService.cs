using Inventory.Helpers;
using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingService : IGeralService
    {
        Task<Addressing> AddAddressingAsync(Addressing location);
        Task<PageList<Addressing>> GetAllPageListDataTable(PageParams pageParams);
        Task<PagingList<Addressing>> GetAllAddressingsByPagingAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Addressing>> GetAddressingsByWarehouseIdAsync(int warehouseId, string filter, int pageindex = 1, string sort = "Name");
        Task<List<Addressing>> GetAllAddressingsByWarehouseIdAsync(int warehouseId);
        Task<Addressing> GetAddressingbyAddressingAndWarehouseNamesAsync(string addressingName, string warehouseName);
        Task<List<Addressing>> GetAllAddressingsAsync();
        Task<Addressing> GetAddressingByIdAsync(int id);
        Task<Addressing> GetAddressingByItemIdAsync(string id);
        Task<bool> ImportAddressingAsync(string fileName, string destiny);
    }
}
