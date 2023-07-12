using System.ComponentModel.DataAnnotations;

namespace Inventory.ViewModels.ViewModelEnums
{
    public enum AddressingSituation : int
    {
        [Display(Name = "Regular")]
        Regular = 0,
        
        [Display(Name = "Item Não Contado")] 
        ItemNoCount = 1,

        [Display(Name = "Item armazenado em mais de um endereçamento")]
        ItemStoredInMoreThanOneAddress = 2,
        
        [Display(Name = "Item sem registro de armazenamento")]
        ItemNoAddressRecord = 3,

        [Display(Name = "Item em endereçamento divergente")]
        ItemInDivergentAddress = 4,
    }
}
