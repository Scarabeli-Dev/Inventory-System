using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IItemService : IGeralService
    {
        Task<PagingList<Item>> GetAllItemsAsync(string filter, int pageindex = 1, string sort = "Name");
        Task<PagingList<Item>> GetItemsByLocationAsync(int stockId, int locationId, string filter, int pageindex = 1, string sort = "Name");
    }
}
