using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IStockTakingService : IGeralService
    {
        Task<bool> NewStockTakingAsync(StockTaking stockTaking);
        bool UpdateStockTaking(StockTaking stockTaking);
        Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId);
        Task<StockTaking> GetStockTakingByItemIdAsync(string itemId);
    }
}
