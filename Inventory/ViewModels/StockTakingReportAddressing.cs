using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels
{
    public class StockTakingReportAddressing
    {
        [Key]
        public int StockTakingId { get; set; }

        [Display(Name = "Código do Item")]
        public string ItemId { get; set; }

        [Display(Name = "Nome do Item")]
        public string ItemName { get; set; }

        [Display(Name = "Quantidade Inicial")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemInitialQuantity { get; set; }

        [Display(Name = "Quantidade Da Contagem")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemStockTakingQuantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ItemStockTakingPreviousQuantity { get; set; }

        [Display(Name = "Quantidade Da Contagem")]
        public int NumberOfCount { get; set; }
    }
}
