using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IItemService : IGeralService
    {
        IEnumerable<Item> Items { get; }
        Task<PagingList<Item>> GetAllItemsPagingAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Item>> GetItemsByAddressingPagingAsync(int addressingId, string filter, int pageindex = 1, string sort = "Name");
        Task<List<Item>> GetAllItemsByAddressingAsync(int addressingnId);
        Task<PagingList<Item>> GetItemsByWarehousePagingAsync(int warehouseId, string filter, int pageindex = 1, string sort = "Name");
        Task<Item> GetItemByIdAsync(string id);
        Task<Item> GetItemByIdForCountAsync(string id);
        Task<bool> ImportItemAsync(string fileName, string destiny);
        Task<List<string>> GetAllItemIdsAsync();
       List<Item> GetAllItemsAsync();
    }
}
