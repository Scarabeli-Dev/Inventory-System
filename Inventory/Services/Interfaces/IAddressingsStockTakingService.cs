using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IAddressingsStockTakingService : IGeralService
    {
        Task CreateAddressingsStockTakingAsync(int inventoryStartId);
    }
}
