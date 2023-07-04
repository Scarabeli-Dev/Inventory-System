using Inventory.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels
{
    public class StockTakingWithMovement
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public MovementeType MovementeType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityStockTaking { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityMovement { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityClosed { get; set; }
    }
}
