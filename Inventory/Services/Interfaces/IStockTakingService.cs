using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IStockTakingService : IGeralService
    {
        Task<bool> SaveStockTakingWithOutRecount(StockTaking stockTaking);
        Task<bool> SaveStockTakingWithRecount(StockTaking stockTaking);
        Task<bool> AddStockTakingForRecountAssync(int stockTakingId);
        Task<List<StockTaking>> GetAllStocktakingWithRecount(string filter, int pageindex = 1, string sort = "ItemId");
        Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId);
        IEnumerable<StockTaking> GetStockTakingsEnumerableAsync();
        Task<List<StockTaking>> GetAllStockTakingByItemIdAsync(string itemId);
        Task<List<StockTaking>> GetAllStockTakingAsync();
        Task<PagingList<StockTaking>> GetStockTakingByPagingAsync(string filter, int pageindex = 1, string sort = "StockTakingDate");
        Task<List<StockTaking>> GetStockTakingByAddressingAsync(int addressingId);
        Task<StockTaking> GetStockTakingByAddressingAndItemIdAsync(int addressingId, string itemId);
        Task<StockTaking> GetStockTakingByWarehouseAndItemIdAsync(int warehouseId, string itemId);
        int GetCountStockTakingByAddressingAsync(int addressingId);
    }
}
