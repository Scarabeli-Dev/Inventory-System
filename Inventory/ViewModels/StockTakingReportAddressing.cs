namespace Inventory.ViewModels
{
    public class StockTakingReportAddressing
    {
        public int StockTakingId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemInitialQuantity { get; set; }
        public decimal ItemStockTakingQuantity { get; set; }
        public int NumberOfCount { get; set; }
    }
}
