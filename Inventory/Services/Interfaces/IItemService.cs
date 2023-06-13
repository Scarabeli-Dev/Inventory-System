using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IItemService : IGeralService
    {
        Task<PagingList<Item>> GetAllItemsAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Item>> GetItemsByAddressingAsync(int locationId, string filter, int pageindex = 1, string sort = "Name");
        Task<Item> GetItemByIdAsync(int id);
    }
}
