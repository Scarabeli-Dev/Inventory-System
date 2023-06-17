using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IItemService : IGeralService
    {
        Task<PagingList<Item>> GetAllItemsAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Item>> GetItemsByAddressingAsync(int addressingId, string filter, int pageindex = 1, string sort = "Name");
        Task<List<Item>> GetAllItemsByAddressingAsync(int addressingnId);
        Task<Item> GetItemByIdAsync(string id);
        Task<bool> ImportItemAsync(string fileName, string destiny);
    }
}
