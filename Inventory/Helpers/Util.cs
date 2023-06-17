namespace Inventory.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public Util(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> SaveDocument(IFormFile documentFile, string destiny)
        {
            string documentName = new String(Path.GetFileNameWithoutExtension(documentFile.FileName).Take(10).ToArray()).Replace(' ', '-');

            documentName = $"{documentName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(documentFile.FileName)}";

            var documentPath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{destiny}", documentName);

            var directoryPath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{destiny}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var fileStream = new FileStream(documentPath, FileMode.Create))
            {
                await documentFile.CopyToAsync(fileStream);
            }

            return documentName;
        }

        public void DeleteDocument(string documentName, string destiny)
        {
            var documentPath = Path.Combine(_hostEnvironment.ContentRootPath, @$"Resources\{destiny}", documentName);
            if (System.IO.File.Exists(documentPath))
            {
                System.IO.File.Delete(documentPath);
            }
        }
    }
}
