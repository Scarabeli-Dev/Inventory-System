namespace Inventory.ViewModels.ViewModelEnums
{
    public enum StockSituation : int
    {
        Regular = 0,
        ItemNoCount = 1,
        HigherThanRegistered = 2,
        LowerThanRegistered = 3,
    }
}
