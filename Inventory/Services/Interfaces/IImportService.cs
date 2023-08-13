using Inventory.ViewModels.Imports;

namespace Inventory.Services.Interfaces
{
    public interface IImportService
    {
        Task<bool> ImportAsync(string fileName, string destiny);
        Task<List<string>> ImportItemsWithStockTaking(string fileName, string destiny);
    }
}
