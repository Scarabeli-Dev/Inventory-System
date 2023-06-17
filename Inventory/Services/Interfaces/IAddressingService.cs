using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingService : IGeralService
    {
        Task<Addressing> AddAddressingAsync(Addressing location);
        Task<PagingList<Addressing>> GetAllAddressingsByPagingAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Addressing>> GetAddressingsByWarehouseIdAsync(int warehouseId, string filter, int pageindex = 1, string sort = "Name");
        Task<List<Addressing>> GetAllAddressingsByWarehouseIdAsync(int warehouseId);
        Task<Addressing> GetAddressingByIdAsync(int id);
        Task<Addressing> GetAddressingByItemIdAsync(string id);
        Task<bool> ImportAddressingAsync(string fileName, string destiny);
    }
}
