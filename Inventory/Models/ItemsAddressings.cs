using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class ItemsAddressings
    {
        public int Id { get; set; }
        public int AddressingId { get; set; }
        public Addressing Addressing { get; set; }

        public string ItemId { get; set; }
        public Item Item { get; set; }

        [Display(Name = "Quantidade de Itens")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal Quantity { get; set; }
    }
}
