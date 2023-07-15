using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Inventory.Helpers.Interfaces;
using Inventory.ViewModels;
using System.Diagnostics;
using Inventory.Helpers.Exceptions;
using Inventory.Services;

namespace Inventory.Controllers
{
    [Route("Itens")]
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUtil _util;
        private string _destiny = "Item";

        public ItemsController(IItemService itemService, IAddressingService addressingService, IWarehouseService warehouseService, IWebHostEnvironment hostEnvironment, IUtil util)
        {
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _hostEnvironment = hostEnvironment;
            _util = util;
        }

        // GET: Items
        [Authorize(Roles = "Admin, Gerente")]
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            return View(await _itemService.GetAllItemsPagingAsync(filter, pageindex, sort));
        }

        // GET: Items by addressing
        [Route("Lista/Enderecamento")]
        public async Task<IActionResult> ItemAddressingIndex(int addressingId, string filter, int pageindex = 1, string sort = "Name")
        {
            return View(await _itemService.GetItemsByAddressingPagingAsync(addressingId, filter, pageindex, sort));
        }

        // GET: Items by warehouse
        [Route("Lista/Deposito")]
        public async Task<IActionResult> ItemWarehouseIndex(int warehouseId, string filter, int pageindex = 1, string sort = "Name")
        {
            ViewBag.WarehouseId = warehouseId;
            return View(await _itemService.GetItemsByWarehousePagingAsync(warehouseId, filter, pageindex, sort));
        }

        // GET: Items/Details/5
        [Route("Detalhe")]
        public async Task<IActionResult> Details(string id)
        {

            var item = await _itemService.GetItemByIdAsync(id);

            if (item == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });
            }

            return View(item);
        }

        // GET: Items/Create
        [Route("Cadastro")]
        public IActionResult Create()
        {
            var locations = _addressingService.GetAllAsync<Addressing>();
            ViewBag.Addressing = locations;

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cadastro")]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitOfMeasurement,Quantity,ExpirationDate,FabricationDate,Observation")] ItemCreateViewModel itemVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _itemService.Add(itemVm);
                    await _itemService.SaveChangesAsync();
                    TempData["successMessage"] = "Item " + itemVm.Name;
                    return RedirectToAction(nameof(Index));
                }
                TempData["errorMessage"] = "item " + itemVm.Name;
                return View();
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction("Error", "Error", new { message = e.Message });
            }
        }


        // GET: Items/Edit/5
        [Route("Editar")]
        public async Task<IActionResult> Edit(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });
            }

            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            ViewBag.Addressing = await _addressingService.GetAllAsync<Addressing>();

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UnitOfMeasurement,Quantity,ExpirationDate,FabricationDate,Observation")] Item item)
        {
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            ViewBag.Addressing = await _addressingService.GetAllAsync<Addressing>();

            if (id != item.Id)
            {
                return RedirectToAction("Error", "Error", new { message = "Id do Item não corresponde ao selecionado" });
            }

            try
            {
                _itemService.Update(item);
                await _itemService.SaveChangesAsync();

                TempData["successMessage"] = "Item " + item.Name;
                return RedirectToAction(nameof(Index));
            }
            catch (DbConcurrencyException)
            {
                TempData["errorMessage"] = "item " + item.Name;
                return View(item);
            }
        }

        // GET: Items/Delete/5
        [Route("Deletar")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return RedirectToAction("Error", "Error", new { message = "Item não encontrado" });

            }
            _itemService.Delete(item);

            await _itemService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Importacao")]
        public async Task<IActionResult> ImportItems(IFormFile documentFile)
        {
            if (documentFile != null && documentFile.Length > 0)
            {
                // Salva o documento
                string documentName = await _util.SaveDocument(documentFile, _destiny);

                // Lê o arquivo CSV e cria os modelos
                string documentPath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources", _destiny, documentName);

                // Implemente o código para ler o arquivo CSV e criar os modelos
                if (await _itemService.ImportItemAsync(documentPath, _destiny))
                {
                    // Deleta o arquivox
                    _util.DeleteDocument(documentName, _destiny);

                    // Retorna uma resposta de sucesso ou redireciona para outra página
                    TempData["successMessage"] = "Items";
                    return RedirectToAction(nameof(Index));
                }
            }
            // Retorna uma resposta de erro ou redireciona para outra página
            TempData["errorMessage"] = "item";
            return BadRequest("Nenhum documento foi enviado.");
        }

        [Route("Relatorio")]
        public async Task<IActionResult> CreateReport()
        {
            //var pathReport = Path.Combine(_webHostEnvironment.WebRootPath, @"reports\StockTaking.frx");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", _destiny, "Item.frx");
            var reportFile = path;
            var freport = new FastReport.Report();

            var ItemList = await _itemService.GetAllAsync<Item>();

            freport.Dictionary.RegisterBusinessObject(ItemList, "ItemList", 10, true);
            freport.Report.Save(reportFile);

            return Ok($"Relatório gerado: {path}");
        }
    }
}
