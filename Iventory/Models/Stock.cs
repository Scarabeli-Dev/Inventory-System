using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}
