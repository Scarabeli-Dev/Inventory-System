using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels
{
    public class StockTakingReportAddressing
    {
        public int StockTakingId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemInitialQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemStockTakingQuantity { get; set; }
        public int NumberOfCount { get; set; }
    }
}
