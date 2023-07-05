using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Inventory.Models
{
    public class PerishableItem
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data de Fabricação")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FabricationDate { get; set; }

        [Display(Name = "Data de Validade")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Lote")]
        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres!")]
        public string ItemBatch { get; set; }

        [Display(Name = "Quantidade na Contagem")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PerishableItemQuantity { get; set; }

        public int StockTakingId { get; set; }
        public StockTaking StockTaking { get; set; }
    }
}