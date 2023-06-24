using Inventory.Models.Enums;
using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MovementDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ImportDate { get; set; }
    }
}
