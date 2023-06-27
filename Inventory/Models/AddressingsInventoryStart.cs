using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.Models
{
    public class AddressingsInventoryStart
    {
        public int Id { get; set; }

        [Display(Name = "Endereçamento")]
        public int AddressingId { get; set; }
        public Addressing Addressing { get; set; }

        [Display(Name = "Inventário")]
        public int InventoryStartId { get; set; }
        public InventoryStart InventoryStart { get; set; }

        [Display(Name = "Contagem Realizada")]
        public bool AddressingCountRealized { get; set; }

        [Display(Name = "Contagem Finalizada")]
        public bool AddressingCountEnded { get; set; }
        public IEnumerable<StockTaking> StockTaking { get; set; }
    }
}
