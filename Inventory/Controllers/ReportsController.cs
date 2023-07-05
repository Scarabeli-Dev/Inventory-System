using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("Relatorios")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IInventoryMovementService _inventoryMovementService;
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;

        public ReportsController(IInventoryMovementService inventoryMovementService,
                                 IStockTakingService stockTakingService,
                                 IItemService itemService)
        {
            _inventoryMovementService = inventoryMovementService;
            _stockTakingService = stockTakingService;
            _itemService = itemService;
        }

        [Route("Contagem-com-movimentacao")]
        public async Task<IActionResult> Index()
        {
            PageList<StockTakingWithMovement> view = new PageList<StockTakingWithMovement>();

            var inventoryMovement = _inventoryMovementService.GetAllInventoryMovementsAsync();

            foreach (var item in inventoryMovement)
            {
                StockTakingWithMovement stockTakingWithMovement = new StockTakingWithMovement();

                var baseItem = _itemService.Items.Where(a => a.Addressings.Any(w => w.Addressing.WarehouseId == item.WarehouseId && (w.ItemId == item.ItemId)));
                StockTaking stockTackingItem = await _stockTakingService.GetStockTakingByWarehouseAndItemIdAsync(item.WarehouseId, item.ItemId);

                stockTakingWithMovement.ItemId = item.ItemId;
                stockTakingWithMovement.ItemName = item.Item.Name;
                stockTakingWithMovement.QuantityStockTaking = stockTackingItem.StockTakingQuantity;
                stockTakingWithMovement.QuantityMovement = item.Amount;
                stockTakingWithMovement.MovementeType = item.MovementeType;
                stockTakingWithMovement.QuantityClosed = 0;
            }
            return View(view);
        }
    }
}
