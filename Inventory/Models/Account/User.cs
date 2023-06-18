using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.Account
{
    public class User : IdentityUser<int>
    {
        [Display(Name = "Nome")]
        [StringLength(80, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string Name { get; set; }

        [Display(Name = "Ativo")]
        public bool Available { get; set; }
        public IEnumerable<UserRole> UserRole { get; set; }

    }
}
