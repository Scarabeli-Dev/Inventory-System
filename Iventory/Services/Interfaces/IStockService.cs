using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IStockService : IGeralService
    {
        Task<Stock> GetStockByIdAsync(int? id);
        Task<PagingList<Stock>> GetAllStocksAsync(string filter, int pageindex = 1, string sort = "Name");
    }
}
