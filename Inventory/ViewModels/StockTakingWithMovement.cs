using Inventory.Models.Enums;

namespace Inventory.ViewModels
{
    public class StockTakingWithMovement
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public MovementeType MovementeType { get; set; }
        public decimal QuantityStockTaking { get; set; }
        public decimal QuantityMovement { get; set; }
        public decimal QuantityClosed { get; set; }
    }
}
