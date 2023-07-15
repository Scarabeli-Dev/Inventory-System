using Inventory.Models.Enums;
using Inventory.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels.Imports
{
    public class InventoryMovementImport
    {
        public string ItemId { get; set; }
        public MovementeType MovementeType { get; set; }
        public string WarehouseName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime MovementDate { get; set; }
    }
}
