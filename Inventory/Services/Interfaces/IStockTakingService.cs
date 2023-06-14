using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IStockTakingService : IGeralService
    {
        Task<bool> NewStockTakingAsync(StockTaking stockTaking, Item item);
    }
}
