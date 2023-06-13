namespace Inventory.Models
{
    public class StockTakingItems
    {
        public int Id { get; set; }
        public int StockTakingId { get; set; }
        public StockTaking StockTaking { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
