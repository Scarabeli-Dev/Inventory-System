namespace Inventory.Models
{
    public class StockTakingStart
    {
        public int Id { get; set; }
        public DateTime StockTakingStartDate { get; set; }
        public DateTime StockTakingFinishDate { get; set; }
        public bool IsCompleted { get; set; }
        public IEnumerable<AddressingsStockTaking> Addressings { get; set; }
    }
}
