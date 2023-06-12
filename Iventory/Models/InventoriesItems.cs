namespace Inventory.Models
{
    public class InventoriesItems
    {
        public int InventoryCountId { get; set; }
        public InventoryCount InventoryCount { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
