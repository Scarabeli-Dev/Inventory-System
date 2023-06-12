using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.Models
{
    public class InventoryCount
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data de Validade")]
        public DateTime? InventoryDate { get; set; }

        [Display(Name = "Quantidade na Contagem")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public double? InventoryQuantity { get; set; }

        [Display(Name = "Observação")]
        [StringLength(250, ErrorMessage = "Máximo de {1} caracteres!")]
        public string? InventoryObservation { get; set; }

        [Display(Name = "Item")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
