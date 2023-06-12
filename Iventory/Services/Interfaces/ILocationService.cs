using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface ILocationService : IGeralService
    {
        Task<Location> AddLocationAsync(Location location);
        Task<PagingList<Location>> GetLocationsByStockIdAsync(int stockId, string filter, int pageindex = 1, string sort = "Name");
    }
}
