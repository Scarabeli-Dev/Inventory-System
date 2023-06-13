namespace Inventory.Models
{
    public class AddressingsStockTaking
    {
        public int Id { get; set; }
        public Addressing Addressing { get; set; }
        public StockTakingStart StockTakingStart { get; set; }
        public bool AddressingCountRealized { get; set; }
        public bool AddressingCountEnded { get; set; }
    }
}
