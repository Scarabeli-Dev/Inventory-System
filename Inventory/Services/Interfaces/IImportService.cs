namespace Inventory.Services.Interfaces
{
    public interface IImportService
    {
        Task<bool> ImportAsync(string fileName, string destiny);
    }
}
