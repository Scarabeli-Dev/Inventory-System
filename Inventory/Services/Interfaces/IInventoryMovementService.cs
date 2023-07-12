using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IInventoryMovementService
    {
        List<InventoryMovement> GetInventoryMovementsByItemId(string itemId);
        Task<List<InventoryMovement>> GetAllInventoryMovementsAsync();
        Task<List<InventoryMovement>> ImportInventoryMovementsAsync(string fileName, string destiny);
    }
}
