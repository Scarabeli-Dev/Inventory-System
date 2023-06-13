using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class Addressing
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [StringLength(50, ErrorMessage = "Máximo de {1} caracteres!")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Name { get; set; }

        [Display(Name = "Depósito")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [Display(Name = "Itens")]
        public IEnumerable<ItemsAddressings> Item { get; set; }
        public IEnumerable<AddressingsStockTaking> StockTaking { get; set; }

    }
}