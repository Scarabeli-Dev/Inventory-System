using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IAddressingService _addressingService;

        public WarehousesController(IWarehouseService warehouseService, IAddressingService addressingService)
        {
            _warehouseService = warehouseService;
            _addressingService = addressingService;
        }

        // GET: Warehouses
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            var model = await _warehouseService.GetAllWarehousesAsync(filter, pageindex, sort);

            //if (model.Count() == 0) return View();

            return View(model);
        }

        // GET: Warehouses/Details/5
        public async Task<IActionResult> Details(int? id, string filter)
        {
            //if (id == null || _warehouseService.GetWarehouseByIdAsync(id) == null)
            //{
            //    return NotFound();
            //}

            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            ViewBag.Filter = filter;

            return View(warehouse);
        }

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _warehouseService.Add(warehouse);
                await _warehouseService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null || _warehouseService.GetWarehouseByIdAsync(id) == null)
            //{
            //    return NotFound();
            //}

            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            ViewData["WarehouseId"] = id;

            return View(warehouse);
        }

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _warehouseService.Update(warehouse);
                    await _warehouseService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await _warehouseService.GetWarehouseByIdAsync(id);
                    if (result == null)
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
            return View(warehouse);
        }

        // GET: Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse != null)
            {
                _warehouseService.Delete(warehouse);
            }

            await _warehouseService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool WarehouseExists(int id)
        //{
        //  return _warehouseService.Warehouse.Any(e => e.Id == id);
        //}
    }
}
