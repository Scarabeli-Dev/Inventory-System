using Inventory.Models.Enums;
using Inventory.Models;

namespace Inventory.ViewModels.Imports
{
    public class InventoryMovementImport
    {
        public string ItemId { get; set; }
        public MovementeType MovementeType { get; set; }
        public int WarehouseId { get; set; }
        public double Amount { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
