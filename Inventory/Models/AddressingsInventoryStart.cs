using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class AddressingsInventoryStart
    {
        public int Id { get; set; }

        [Display(Name = "Endereçamento")]
        public int AddressingId { get; set; }

        [Display(Name = "Endereçamento")]
        public Addressing Addressing { get; set; }

        [Display(Name = "Inventário")]
        public int InventoryStartId { get; set; }

        [Display(Name = "Inventário")]
        public InventoryStart InventoryStart { get; set; }

        [Display(Name = "Contagem Realizada")]
        public bool AddressingCountRealized { get; set; }

        [Display(Name = "Contagem Finalizada")]
        public bool AddressingCountEnded { get; set; }

        [Display(Name = "Contagens")]
        public IEnumerable<StockTaking> StockTaking { get; set; }
    }
}
