using Inventory.Models;
using ReflectionIT.Mvc.Paging;

namespace Inventory.Services.Interfaces
{
    public interface IStockTakingService : IGeralService
    {
        Task<bool> NewStockTakingAsync(StockTaking stockTaking);
        bool UpdateStockTaking(StockTaking stockTaking);
        Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId);
        Task<StockTaking> GetStockTakingByItemIdAsync(string itemId);
        Task<List<StockTaking>> GetAllStockTakingAsync();
        Task<PagingList<StockTaking>> GetStockTakingByPagingAsync(string filter, int pageindex = 1, string sort = "StockTakingDate");
        Task<List<StockTaking>> GetStockTakingByAddressingAsync(int addressingId);
        int GetCountStockTakingByAddressingAsync(int addressingId);
    }
}
