using Inventory.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Models
{
    public class InventoryMovement
    {
        public int Id { get; set; }


        [Key]
        [Display(Name = "Código do Item")]
        [StringLength(150, ErrorMessage = "Máximo de {1} caracteres!")]
        public string ItemId { get; set; }
        public Item Item { get; set; }

        [Display(Name = "Tipo de Movimentação")]
        public MovementeType MovementeType { get; set; }
        
        [Display(Name = "Depósito")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [Display(Name = "Quantidade Movimentada")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Display(Name = "Data da Movimentação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime MovementDate { get; set; }

        [Display(Name = "Data da Importação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ImportDate { get; set; }
    }
}
