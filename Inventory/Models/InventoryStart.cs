using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class InventoryStart
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InventoryStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StockTakingFinishDate { get; set; }
        public bool IsCompleted { get; set; }
        public IEnumerable<AddressingsInventoryStart> Addressings { get; set; }
    }
}
