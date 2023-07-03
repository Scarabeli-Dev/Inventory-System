using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Inventory.Helpers.Interfaces;
using Inventory.Helpers;

namespace Inventory.Controllers
{
    [Route("Enderecamento")]
    [Authorize]
    public class AddressingsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IAddressingService _addressingService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUtil _util;
        private string _destiny = "Adressing";

        public AddressingsController(InventoryContext context, IAddressingService addressingService, IWebHostEnvironment hostEnvironment, IUtil util)
        {
            _context = context;
            _addressingService = addressingService;
            _hostEnvironment = hostEnvironment;
            _util = util;
        }

        // GET: Addressings
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            var addressings = await _addressingService.GetAllAddressingsByPagingAsync(filter, pageindex, sort);

            return View(addressings);
        }

        [Route("Lista/Deposito")]
        public async Task<IActionResult> AddressingByWarehouse(int warehouseId, string filter, int pageindex = 1, string sort = "Name")
        {
            var addressings = await _addressingService.GetAddressingsByWarehouseIdAsync(warehouseId, filter, pageindex, sort);

            ViewBag.WarehouseId = warehouseId;

            return View(addressings);
        }

        [Route("Lista/Contagem")]
        public async Task<IActionResult> AddressingForCount([FromQuery] PageParams pageParams)
        {
            var addressings = await _addressingService.GetAllPageListDataTable(pageParams);
            Response.AddPagination(addressings.CurrentPage, addressings.PageSize, addressings.TotalCount, addressings.TotalPages);

            return View(addressings);
        }

        // GET: Addressings/Details/5
        [Route("Detalhes")]
        public async Task<IActionResult> Details(int id)
        {
            var addressing = await _addressingService.GetAddressingByIdAsync(id);

            if (addressing == null)
            {
                return NotFound();
            }

            return View(addressing);
        }

        // GET: Addressings/Create
        [Route("Cadastro")]
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        // POST: Addressings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Cadastro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Addressing addressing)
        {
            if (ModelState.IsValid)
            {
                _addressingService.Add(addressing);

                await _addressingService.SaveChangesAsync();
                TempData["successMessage"] = "Endereçamento " + addressing.Name;
                return RedirectToAction("Details", "Warehouses", new { id = addressing.WarehouseId });
            }
            TempData["errorMessage"] = "endereçamento " + addressing.Name;
            return RedirectToAction("Details", "Warehouses", new { id = addressing.WarehouseId });
        }

        // GET: Addressings/Edit/5
        [Route("Editar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Addressing == null)
            {
                return NotFound();
            }

            var addressing = await _context.Addressing.FindAsync(id);
            if (addressing == null)
            {
                return NotFound();
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", addressing.WarehouseId);
            return View(addressing);
        }

        // POST: Addressings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Editar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,WarehouseId")] Addressing addressing)
        {
            if (id != addressing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressingExists(addressing.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["successMessage"] = "endereçamento " + addressing.Name;
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", addressing.WarehouseId);
            TempData["errorMessage"] = "endereçamento " + addressing.Name;
            return View(addressing);
        }

        // GET: Addressings/Delete/5
        [Route("Deletar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Addressing == null)
            {
                return NotFound();
            }

            var addressing = await _context.Addressing
                .Include(l => l.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (addressing == null)
            {
                return NotFound();
            }

            return View(addressing);
        }

        // POST: Addressings/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Addressing == null)
            {
                return Problem("Entity set 'InventoryContext.Addressing'  is null.");
            }
            var addressing = await _context.Addressing.FindAsync(id);
            if (addressing != null)
            {
                _context.Addressing.Remove(addressing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressingExists(int id)
        {
            return _context.Addressing.Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("Importacao")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportAddressings(IFormFile documentFile)
        {
            if (documentFile != null && documentFile.Length > 0)
            {
                // Salva o documento
                string documentName = await _util.SaveDocument(documentFile, _destiny);

                // Lê o arquivo CSV e cria os modelos
                string documentPath = Path.Combine(_hostEnvironment.ContentRootPath, "Resources", _destiny, documentName);

                // Implemente o código para ler o arquivo CSV e criar os modelos
                if (await _addressingService.ImportAddressingAsync(documentPath, _destiny))
                {
                    // Deleta o arquivox
                    _util.DeleteDocument(documentName, _destiny);

                    // Retorna uma resposta de sucesso ou redireciona para outra página
                    return RedirectToAction(nameof(Index));

                }
            }
            // Retorna uma resposta de erro ou redireciona para outra página
            return BadRequest("Nenhum documento foi enviado.");
        }
    }
}
