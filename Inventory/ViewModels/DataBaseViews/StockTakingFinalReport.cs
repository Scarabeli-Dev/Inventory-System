using Inventory.Models.Enums;

namespace Inventory.ViewModels.DataBaseViews
{
    public class StockTakingFinalReport
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

    }
}
