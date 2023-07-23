namespace Inventory.ViewModels.DashboardViewModels
{
    public class ChartsViewModel
    {
        public int TotalOfItems { get; set; }
        public int ItemsWithCorrectAmount { get; set; }
        public int ItemsWithStockTakingAmount { get; set; }
        public int ItemsWithAddressingRigth { get; set; }
        public decimal ItemPerishableAmount { get; set; }
        public decimal ItemPerishableExpirateDate { get; set; }
        public decimal TotalQuantityItems { get; set; }
        public decimal TotalQuantityItemsStockTaking { get; set; }
        public decimal StockTakingComplet { get; set; }

        // Adicione as propriedades para as informações dos gráficos aqui
        public decimal GaugeValueStockTaking { get; set; }
        public decimal GaugeValueAddressing { get; set; }
        public decimal GaugeValueLostDate { get; set; }
        public decimal GaugeStockDivergence { get; set; }
        public decimal RadarUserOfInventory { get; set; }
        public double RadarEffectivenessOfInventoryManagement { get; set; }
    }
}
