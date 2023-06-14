namespace Inventory.Models
{
    public class InventoryStart
    {
        public int Id { get; set; }
        public DateTime InventoryStartDate { get; set; }
        public DateTime StockTakingFinishDate { get; set; }
        public bool IsCompleted { get; set; }
        public IEnumerable<AddressingsStockTaking> Addressings { get; set; }
    }
}
