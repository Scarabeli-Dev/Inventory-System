using FastReport.Data;
using FastReport.Web;
using Inventory.Helpers;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    public class AddressingsInventoryStartController : Controller
    {
        private readonly IAddressingsInventoryStartService _addressingsInventoryStartService;
        private string _destiny = "AddressingInventory";

        public AddressingsInventoryStartController(IAddressingsInventoryStartService addressingsInventoryStartService)
        {
            _addressingsInventoryStartService = addressingsInventoryStartService;
        }


        public IActionResult CreateReport(int inventaryStartId = 1)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", _destiny, "StockTaking.frx");
            var reportFile = path;

            var freport = new FastReport.Report();

            List<StockTakingList> stockTakingList = new List<StockTakingList>();

            var stockTakings = _addressingsInventoryStartService.AddressingsInventoryStarts
                                                                .Where(s => s.InventoryStartId == inventaryStartId)
                                                                .OrderBy(l => l.AddressingCountRealized)
                                                                .ThenBy(l => l.AddressingCountEnded)
                                                                .ToList();

            foreach (var item in stockTakings)
            {
                StockTakingList stockTaking = new StockTakingList();
                stockTaking.AddressingName = item.Addressing.Name;
                stockTaking.TotalItems = item.Addressing.Item.Count();
                stockTaking.ItemsCount = item.StockTaking.Count();
                stockTaking.IsRealized = item.AddressingCountRealized;
                stockTaking.IsCompleted = item.AddressingCountEnded;

            }

            freport.Dictionary.RegisterBusinessObject(stockTakingList, "stockTakingList", 10, true);
            freport.Report.Save(reportFile);

            return Ok($"Relatório gerado: {path}");
        }

        [Route("Relatorio/ContagemEnderecamento")]
        public IActionResult Report(int inventaryStartId = 1)
        {
            var webReport = new WebReport();

            var mysqlDataConnection = new MySqlDataConnection();

            webReport.Report.Dictionary.AddChild(mysqlDataConnection);

            webReport.Report.Load(Path.Combine(Directory.GetCurrentDirectory(), "Resources", _destiny, "AddressingInventory.frx"));

            List<StockTakingList> stockTakingList = new List<StockTakingList>();

            var stockTakings = _addressingsInventoryStartService.AddressingsInventoryStarts
                                                                .Where(s => s.InventoryStartId == inventaryStartId)
                                                                .OrderBy(l => l.AddressingCountRealized)
                                                                .ThenBy(l => l.AddressingCountEnded)
                                                                .ToList();

            foreach (var item in stockTakings)
            {
                StockTakingList stockTaking = new StockTakingList();
                stockTaking.AddressingName = item.Addressing.Name;
                stockTaking.TotalItems = item.Addressing.Item.Count();
                stockTaking.ItemsCount = item.StockTaking.Count();
                stockTaking.IsRealized = item.AddressingCountRealized;
                stockTaking.IsCompleted = item.AddressingCountEnded;
                stockTakingList.Add(stockTaking);
            }

            var result = HelperFastReport.GetTable(stockTakingList, "Contagem");

            webReport.Report.RegisterData(result, "Enderecamento");
            return View(webReport);
        }
    }
}
