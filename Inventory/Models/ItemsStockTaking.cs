namespace Inventory.Models
{
    public class ItemsStockTaking
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public Item Item { get; set; }
        public int InventoryStartId { get; set; }
        public InventoryStart InventoryStart { get; set; }
        public bool ItemCountRealized { get; set; }
        public bool ItemCountEnded { get; set; }
    }
}
