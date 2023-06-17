using Inventory.Models.Enums;

namespace Inventory.Models
{
    public class InventoryMovement
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public MovementeType MovementeType { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public double Amount { get; set; }
        public DateTime MovementDate { get; set; }
        public DateTime ImportDate { get; set; }
    }
}
