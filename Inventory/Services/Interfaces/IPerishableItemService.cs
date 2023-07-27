using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IPerishableItemService : IGeralService
    {
        void Create(List<PerishableItem> perishableItem, int stockTakingId);
        Task<List<PerishableItem>> GetAllByStockTakingId(int stockTakingId);
        Task<bool> DeletePerishableItemAsync(PerishableItem perishableItem);
        Task<PagingList<PerishableItem>> GetAllPerishableItemsPagingListAsync(string filter, int pageindex = 1, string sortExpression = "ExpirationDate", int warehouseId = 0, bool nullExpirationDate = false, DateTime? expirationDate = null);
    }
}
