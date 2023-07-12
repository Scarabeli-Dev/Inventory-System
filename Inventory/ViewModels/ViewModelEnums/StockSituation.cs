using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels.ViewModelEnums
{
    public enum StockSituation : int
    {
        [Display(Name = "Regular")]
        Regular = 0,

        [Display(Name = "Item Não Contado")]
        ItemNoCount = 1,
        
        [Display(Name = "Quantidade contada inferior ao sistema")]
        HigherThanRegistered = 2,

        [Display(Name = "Quantidade contada superior ao sistema")]
        LowerThanRegistered = 3,
    }
}
