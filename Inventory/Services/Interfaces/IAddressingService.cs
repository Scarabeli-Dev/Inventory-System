using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingService : IGeralService
    {
        Task<Addressing> AddAddressingAsync(Addressing location);
        Task<PagingList<Addressing>> GetAddressingsByWarehouseIdAsync(int stockId, string filter, int pageindex = 1, string sort = "Name");
        Task<Addressing> GetAddressingByIdAsync(int id);
    }
}
