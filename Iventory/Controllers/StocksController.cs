using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.Services;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Controllers
{
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly ILocationService _locationService;

        public StocksController(IStockService stockService, ILocationService locationService)
        {
            _stockService = stockService;
            _locationService = locationService;
        }

        // GET: Stocks
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "Name")
        {
            var model = await _stockService.GetAllStocksAsync(filter, pageindex, sort);

            if (model.Count() == 0) return View();

            return View(model);
        }

        // GET: Stocks/Details/5
        public async Task<IActionResult> Details(int? id, string filter)
        {
            //if (id == null || _stockService.GetStockByIdAsync(id) == null)
            //{
            //    return NotFound();
            //}

            var stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            ViewBag.Filter = filter;

            return View(stock);
        }

        // GET: Stocks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                _stockService.Add(stock);
                await _stockService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null || _stockService.GetStockByIdAsync(id) == null)
            //{
            //    return NotFound();
            //}

            var stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            ViewData["StockId"] = id;

            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Stock stock)
        {
            if (id != stock.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _stockService.Update(stock);
                    await _stockService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var result = await _stockService.GetStockByIdAsync(id);
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
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var stock = await _stockService.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }

            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stock = await _stockService.GetStockByIdAsync(id);
            if (stock != null)
            {
                _stockService.Delete(stock);
            }

            await _stockService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool StockExists(int id)
        //{
        //  return _stockService.Stock.Any(e => e.Id == id);
        //}
    }
}
