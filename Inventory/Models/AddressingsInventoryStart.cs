namespace Inventory.Models
{
    public class AddressingsInventoryStart
    {
        public int Id { get; set; }
        public int AddressingId { get; set; }
        public Addressing Addressing { get; set; }
        public int InventoryStartId { get; set; }
        public InventoryStart InventoryStart { get; set; }
        public bool AddressingCountRealized { get; set; }
        public bool AddressingCountEnded { get; set; }
        public IEnumerable<StockTaking> StockTaking { get; set; }
    }
}
