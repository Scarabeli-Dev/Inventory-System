namespace Inventory.Helpers
{
    public interface IUtil
    {
        Task<string> SaveDocument(IFormFile documentFile, string destiny);
        void DeleteDocument(string documentName, string destiny);
    }
}
