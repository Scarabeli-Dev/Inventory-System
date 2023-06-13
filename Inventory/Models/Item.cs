using Inventory.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Unidade de Medida")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        [Display(Name = "Prateleira")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public IEnumerable<ItemsAddressings> Addressing { get; set; }

        [Display(Name = "Quantidade de Itens")]
        public double? Quantity { get; set; }

        [Display(Name = "Observação")]
        [StringLength(250, ErrorMessage = "Máximo de {1} caracteres!")]
        public string Observation { get; set; }
    }
}
