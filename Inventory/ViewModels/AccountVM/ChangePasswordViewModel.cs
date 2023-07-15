using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels.AccountVM
{
    public class ChangePasswordViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Senha")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório!")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

    }
}
