using Inventory.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels
{
    public class StockTakingReport
    {
        [Display(Name = "Código do Item")]
        public string ItemId { get; set; }

        [Display(Name = "Nome do Item")]
        public string ItemName { get; set; }

        [Display(Name = "Quantidade Inicial")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal InitialQuantity { get; set; }

        [Display(Name = "Quantidade Contada")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityStockTaking { get; set; }

        [Display(Name = "Quantidade MOvida")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityMovement { get; set; }

        [Display(Name = "Tipo de Movimentação")]
        public MovementeType MovementeType { get; set; }

        [Display(Name = "Quantidade Final")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantityClosed { get; set; }
    }
}
