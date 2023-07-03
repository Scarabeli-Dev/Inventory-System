using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IInventoryMovementService
    {
        Task<List<InventoryMovement>> ImportInventoryMovementsAsync(string fileName, string destiny);
        List<InventoryMovement> GetAllInventoryMovementsAsync();
    }
}
