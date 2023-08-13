using Inventory.Helpers.Interfaces;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Importacao")]
    public class ImportsController : Controller
    {
        private readonly IImportService _importService;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IUtil _util;
        private readonly IStockTakingService _stockTakingService;
        private string _destiny = "All";

        public ImportsController(IImportService importService,
                                 IHostEnvironment hostEnvironment,
                                 IUtil util,
                                 IStockTakingService stockTakingService)
        {
            _importService = importService;
            _hostEnvironment = hostEnvironment;
            _util = util;
            _stockTakingService = stockTakingService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.TryGetValue("ImportBases", out var itemsIdsBytes) && itemsIdsBytes.Length > 0)
            {
                var itemsIdsString = Encoding.UTF8.GetString(itemsIdsBytes);
                List<string> itemsIds = JsonConvert.DeserializeObject<List<string>>(itemsIdsString);

                List<StockTaking> items = new List<StockTaking>();
                HashSet<string> itemsRead = new HashSet<string>();
                foreach (var item in itemsIds)
                {
                    if (itemsRead.Contains(item))
                    {
                        continue;
                    }
                    itemsRead.Add(item);
                    var itemStockTaking = await _stockTakingService.GetAllStockTakingImportByItemIdAsync(item);

                    foreach (var itemCount in itemStockTaking)
                    {
                        items.Add(itemCount);
                    }
                }
                return View(items);
            }

            return View();
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
                    TempData["successMessage"] = "Itens";
                    return RedirectToAction("Index", "Warehouses");
                }
            }
            // Retorna uma resposta de erro ou redireciona para outra página
            TempData["errorMessage"] = "item";
            return RedirectToAction(nameof(ImportAll));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Contados")]
        public async Task<IActionResult> ImportWithStockTaking(IFormFile documentFile)
        {
            if (documentFile != null && documentFile.Length > 0)
            {
                // Salva o documento
                string documentName = await _util.SaveDocument(documentFile, _destiny);

                // Lê o arquivo CSV e cria os modelos
                string documentPath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources", _destiny, documentName);

                // Implemente o código para ler o arquivo CSV e criar os modelos
                List<string> itemsIds = await _importService.ImportItemsWithStockTaking(documentPath, _destiny);
                if (itemsIds != null)
                {
                    HttpContext.Session.SetString("ImportBases", JsonConvert.SerializeObject(itemsIds));
                    // Deleta o arquivo
                    _util.DeleteDocument(documentName, _destiny);

                    TempData["successMessage"] = "Itens";
                    return RedirectToAction(nameof(Index));
                }
            }
            // Retorna uma resposta de erro ou redireciona para outra página
            TempData["errorMessage"] = "item";
            return RedirectToAction(nameof(ImportAll));
        }
    }
}
