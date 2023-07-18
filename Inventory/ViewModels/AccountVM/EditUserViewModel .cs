using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels.AccountVM
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Nome")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string Name { get; set; }

        [Display(Name = "Nome de Usuário")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string UserName { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Perfil de Usuário")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string Role { get; set; }
    }
}
