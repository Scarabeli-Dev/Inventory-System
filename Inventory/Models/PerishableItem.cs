using System.ComponentModel.DataAnnotations;
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

        public int StockTakingId { get; set; }
        public StockTaking StockTaking { get; set; }
    }
}