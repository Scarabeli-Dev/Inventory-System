using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels.AccountVM
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nome de Usuário")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
