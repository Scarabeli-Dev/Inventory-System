using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Depósito")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int StockId { get; set; }
        public Stock Stock { get; set; }

        [Display(Name = "Itens")]
        public IEnumerable<ItemsLocations> Item { get; set; }
    }
}