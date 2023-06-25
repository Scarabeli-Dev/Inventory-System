namespace Inventory.ViewModels
{
    public class StockTakingList
    {
        public string AddressingName { get; set; }
        public int TotalItems { get; set; }
        public int ItemsCount { get; set; }
        public bool IsRealized { get; set; }
        public bool IsCompleted { get; set; }
    }
}
