using Inventory.ViewModels.DashboardViewModels;

namespace Inventory.Services.Interfaces
{
    public interface IDashboardService
    {
        ChartsViewModel DashboardData(int warehouseId);
    }
}
