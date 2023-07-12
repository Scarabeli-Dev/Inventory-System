using Inventory.Models;

namespace Inventory.Services.Interfaces
{
    public interface IItemAddressingService
    {
        Task<ItemsAddressings> GetItemAddressingByIdsAsync(string itemId, int addressingId);
        Task<List<ItemsAddressings>> GetAllItemAddressingByItemIdsAsync(string itemId);
        Task<List<ItemsAddressings>> GetAllItemsAddressingsAsync();
    }
}
