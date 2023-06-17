namespace Inventory.ViewModels
{
    public class StockTakingReportAddressing
    {
        public int StockTakingId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal ItemInitialAmount { get; set; }
        public decimal ItemStockTakingAmount { get; set; }
        public int NumberOfCount { get; set; }
    }
}
