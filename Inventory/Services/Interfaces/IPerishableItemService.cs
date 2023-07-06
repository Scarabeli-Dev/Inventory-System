using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IPerishableItemService : IGeralService
    {
        void Create(List<PerishableItem> perishableItem, int stockTakingId);
        Task<List<PerishableItem>> GetAllByStockTakingId(int stockTakingId);
        Task<bool> DeletePerishableItemAsync(PerishableItem perishableItem);
    }
}
