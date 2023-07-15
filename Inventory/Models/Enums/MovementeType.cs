using System.ComponentModel.DataAnnotations;

namespace Inventory.Models.Enums
{
    public enum MovementeType : int
    {
        [Display(Name ="Entrada")]
        E = 0,

        [Display(Name = "Saída")]
        S = 1,

        [Display(Name ="Sem Movimentação")]
        SM = 2,
    }
}
