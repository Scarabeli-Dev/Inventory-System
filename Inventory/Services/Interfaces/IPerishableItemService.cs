using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IPerishableItemService
    {
        void Create(List<PerishableItem> perishableItem, int stockTakingId);
    }
}
