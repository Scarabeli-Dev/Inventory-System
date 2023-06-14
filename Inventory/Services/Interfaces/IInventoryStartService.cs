using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IInventoryStartService : IGeralService
    {
        Task CreateInventoryStartAsync(InventoryStart inventoryStart);
        Task<PagingList<InventoryStart>> GetAllInventoryStartsAsync(string filter, int pageindex = 1, string sort = "InventoryStartDate");
        Task<InventoryStart> GetInventoryStartByIdAsync(int id);
        Task<InventoryStart> GetInventoryStartByAddressingAsync(int addressingId);
    }
}
