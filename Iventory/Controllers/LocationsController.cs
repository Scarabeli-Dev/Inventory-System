using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Controllers
{
    public class LocationsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly ILocationService _locationService;

        public LocationsController(InventoryContext context, ILocationService locationService)
        {
            _context = context;
            _locationService = locationService;
        }

        // GET: Locations
        public async Task<IActionResult> Index(int stockId, string filter, int pageindex = 1, string sort = "Name")
        {
            var locations = await _locationService.GetLocationsByStockIdAsync(stockId, filter, pageindex = 1, sort = "Name");

            if (locations.Count() == 0) return View();

            ViewBag.StockId = stockId;

            return View(locations);
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.Stock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // GET: Locations/Create
        public IActionResult Create()
        {
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                await _locationService.AddLocationAsync(location);

                //_context.Add(location);
                //await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Stocks", new { id = location.StockId });
            }
            //ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", location.StockId);
            return RedirectToAction("Details", "Stocks", new { id = location.StockId });
            }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", location.StockId);
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StockId")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            ViewData["StockId"] = new SelectList(_context.Stock, "Id", "Name", location.StockId);
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .Include(l => l.Stock)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Location == null)
            {
                return Problem("Entity set 'InventoryContext.Location'  is null.");
            }
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
          return _context.Location.Any(e => e.Id == id);
        }
    }
}
