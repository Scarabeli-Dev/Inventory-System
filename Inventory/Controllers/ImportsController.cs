using Inventory.Helpers.Interfaces;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("Importacao")]
    public class ImportsController : Controller
    {
        private readonly IImportService _importService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IUtil _util;
        private string _destiny = "All";

        public ImportsController(IImportService importService,
                                 IHostEnvironment hostEnvironment,
                                 IUtil util)
        {
            _importService = importService;
            _hostEnvironment = hostEnvironment;
            _util = util;
        }

        [Route("Base")]
        public IActionResult ImportAll()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Base")]
        public async Task<IActionResult> ImportAll(IFormFile documentFile)
        {
            if (documentFile != null && documentFile.Length > 0)
            {
                // Salva o documento
                string documentName = await _util.SaveDocument(documentFile, _destiny);

                // Lê o arquivo CSV e cria os modelos
                string documentPath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources", _destiny, documentName);

                // Implemente o código para ler o arquivo CSV e criar os modelos
                if (await _importService.ImportAsync(documentPath, _destiny))
                {
                    // Deleta o arquivox
                    _util.DeleteDocument(documentName, _destiny);

                    // Retorna uma resposta de sucesso ou redireciona para outra página
                    TempData["successMessage"] = "Items";
                    return RedirectToAction("Index", "Warehouses");
                }
            }
            // Retorna uma resposta de erro ou redireciona para outra página
            TempData["errorMessage"] = "item";
            return BadRequest("Nenhum documento foi enviado.");
        }
    }
}
