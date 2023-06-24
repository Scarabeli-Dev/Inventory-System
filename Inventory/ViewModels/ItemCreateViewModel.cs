using Inventory.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels
{
    public class ItemCreateViewModel
    {
        [Display(Name = "Código")]
        [StringLength(150, ErrorMessage = "Máximo de {1} caracteres!")]
        public string Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(150, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Unidade de Medida")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        [Display(Name = "Prateleira")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public List<int> AddressingsId { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public List<decimal> Quantity { get; set; }

        [Display(Name = "Observação")]
        [StringLength(250, ErrorMessage = "Máximo de {1} caracteres!")]
        public string Observation { get; set; }
    }
}
