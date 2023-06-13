using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Controllers
{
    public class AddressingsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IAddressingService _addressingService;

        public AddressingsController(InventoryContext context, IAddressingService addressingService)
        {
            _context = context;
            _addressingService = addressingService;
        }

        // GET: Addressings
        public async Task<IActionResult> Index(int stockId, string filter, int pageindex = 1, string sort = "Name")
        {
            var addressings = await _addressingService.GetAddressingsByWarehouseIdAsync(stockId, filter, pageindex = 1, sort = "Name");

            if (addressings.Count() == 0) return View();

            ViewBag.WarehouseId = stockId;

            return View(addressings);
        }

        // GET: Addressings/Details/5
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
        public IActionResult Create()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name");
            return View();
        }

        // POST: Addressings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Addressing addressing)
        {
            if (ModelState.IsValid)
            {
                await _addressingService.AddAddressingAsync(addressing);

                //_context.Add(addressing);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Warehouses", new { id = addressing.WarehouseId });
            }
            //ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", addressing.WarehouseId);
            return RedirectToAction("Details", "Warehouses", new { id = addressing.WarehouseId });
            }

        // GET: Addressings/Edit/5
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "Id", "Name", addressing.WarehouseId);
            return View(addressing);
        }

        // GET: Addressings/Delete/5
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
    }
}
