using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;

namespace Inventory.Controllers
{
    public class ItemsController : Controller
    {
        private readonly InventoryContext _context;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;

        public ItemsController(InventoryContext context, IItemService itemService, IAddressingService addressingService, IWarehouseService warehouseService)
        {
            _context = context;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
        }

        // GET: Items
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
              return View(await _itemService.GetAllItemsAsync(filter, pageindex, sort));
        }

        // GET: Items by addressing
        public async Task<IActionResult> ItemAddressingIndex(int addressingId, string filter, int pageindex = 1, string sort = "Name")
        {
            return View(await _itemService.GetItemsByAddressingAsync(addressingId, filter, pageindex, sort));
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var item = await _itemService.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UnitOfMeasurement,Quantity,ExpirationDate,FabricationDate,Observation")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            ViewBag.Addressing = await _addressingService.GetAllAsync<Addressing>();

            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UnitOfMeasurement,Quantity,ExpirationDate,FabricationDate,Observation")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _itemService.Update(item);
                    await _itemService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await _itemService.GetItemByIdAsync(id);
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
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }
            
            await _itemService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
