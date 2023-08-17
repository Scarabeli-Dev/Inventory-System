using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Collections.Immutable;

namespace Inventory.Services
{
    public class StockTakingService : GeralService, IStockTakingService
    {
        private readonly InventoryContext _context;
        private readonly IAddressingsInventoryStartService _addressingsStockTakingService;
        private readonly IPerishableItemService _perishableItemService;
        private readonly IItemAddressingService _itemAddressingService;

        public StockTakingService(InventoryContext context,
                                  IAddressingsInventoryStartService addressingsStockTakingService,
                                  IPerishableItemService perishableItemService,
                                  IItemAddressingService itemAddressingService) : base(context)
        {
            _context = context;
            _addressingsStockTakingService = addressingsStockTakingService;
            _perishableItemService = perishableItemService;
            _itemAddressingService = itemAddressingService;
        }

        public async Task<bool> SaveStockTakingWithOutRecount(StockTaking stockTaking)
        {
            // Define o fuso horário UTC-3
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); // Ou outro ID de fuso horário adequado

            //// Obtém a data e hora atual no fuso horário UTC-3
            //stockTaking.StockTakingDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            stockTaking.StockTakingDate = DateTime.Now;

            if (stockTaking.IsPerishableItem == true)
            {
                stockTaking.StockTakingQuantity = 0;
                var perishableItemsToDelete = new List<PerishableItem>();

                foreach (var item in stockTaking.PerishableItem)
                {
                    stockTaking.StockTakingQuantity = stockTaking.StockTakingQuantity + item.PerishableItemQuantity;

                    if (item.ItemBatch == null && item.PerishableItemQuantity == 0)
                    {
                        perishableItemsToDelete.Add(item);
                        await _perishableItemService.DeletePerishableItemAsync(item);
                    }
                }

                foreach (var itemDelete in perishableItemsToDelete)
                {
                    stockTaking.PerishableItem.Remove(itemDelete);
                }
            }

            _context.StockTaking.Update(stockTaking);
            await _context.SaveChangesAsync();

            await _addressingsStockTakingService.SetAddressingCountRealizedTrueAsync(stockTaking.AddressingsInventoryStartId);
            return true;
        }

        public async Task<bool> SaveStockTakingWithRecount(StockTaking stockTaking)
        {
            //// Define o fuso horário UTC-3
            //var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); // Ou outro ID de fuso horário adequado

            //// Obtém a data e hora atual no fuso horário UTC-3
            //stockTaking.StockTakingDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            stockTaking.StockTakingDate = DateTime.Now;
            stockTaking.NumberOfCount++;

            var addressingId = stockTaking.AddressingsInventoryStartId;

            var inventoryAddressing = await _addressingsStockTakingService.GetAddressingsStockTakingByAddressingIdAsync(stockTaking.AddressingsInventoryStartId);

            stockTaking.AddressingsInventoryStartId = inventoryAddressing.Id;


            var itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(stockTaking.ItemId, addressingId);
            if (itemAddressing != null)
            {
                if (itemAddressing.Quantity != stockTaking.StockTakingQuantity && stockTaking.StockTakingQuantity != stockTaking.StockTakingPreviousQuantity && stockTaking.IsPerishableItem == false)
                {
                    stockTaking.ItemToRecount = true;
                    stockTaking.StockTakingPreviousQuantity = stockTaking.StockTakingQuantity;
                    stockTaking.StockTakingQuantity = 0;
                }
                _context.Entry(itemAddressing).State = EntityState.Detached;
            }
            else if (0 != stockTaking.StockTakingQuantity && stockTaking.StockTakingQuantity != stockTaking.StockTakingPreviousQuantity && stockTaking.IsPerishableItem == false)
            {
                stockTaking.ItemToRecount = true;
                stockTaking.StockTakingPreviousQuantity = stockTaking.StockTakingQuantity;
                stockTaking.StockTakingQuantity = 0;
            }
            else
            {
                stockTaking.ItemToRecount = false;
            }

            if (stockTaking.IsPerishableItem == true)
            {
                stockTaking.StockTakingQuantity = 0;
                var perishableItemsToDelete = new List<PerishableItem>();

                foreach (var item in stockTaking.PerishableItem)
                {
                    stockTaking.StockTakingQuantity = stockTaking.StockTakingQuantity + item.PerishableItemQuantity;

                    if (item.ItemBatch == null && item.PerishableItemQuantity == 0)
                    {
                        perishableItemsToDelete.Add(item);
                        await _perishableItemService.DeletePerishableItemAsync(item);
                    }
                }

                foreach (var itemDelete in perishableItemsToDelete)
                {
                    stockTaking.PerishableItem.Remove(itemDelete);
                }
            }

            _context.Entry(inventoryAddressing).State = EntityState.Detached;

            _context.StockTaking.Update(stockTaking);
            await _context.SaveChangesAsync();


            await _addressingsStockTakingService.SetAddressingCountRealizedTrueAsync(addressingId);


            return true;
        }


        public async Task<bool> AddStockTakingForRecountAssync(int stockTakingId)
        {

            var stockTaking = await _context.StockTaking.FirstOrDefaultAsync(i => i.Id == stockTakingId);

            stockTaking.StockTakingPreviousQuantity = stockTaking.StockTakingQuantity;
            stockTaking.StockTakingQuantity = 0;
            stockTaking.ItemToRecount = true;

            if (stockTaking.PerishableItem != null)

                foreach (var item in stockTaking.PerishableItem)
                {
                    item.PerishableItemPreviousQuantity = item.PerishableItemQuantity;
                    item.PerishableItemQuantity = 0;
                    _context.Update<PerishableItem>(item);
                }

            _context.StockTaking.Update(stockTaking);

            return true;
        }

        public async Task<List<StockTaking>> GetAllStocktakingWithRecount(string filter, int pageindex = 1, string sort = "ItemId")
        {
            var result = _context.StockTaking.Include(i => i.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing).ThenInclude(w => w.Warehouse)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .Where(rc => rc.ItemToRecount == true)
                                             .AsNoTracking()
                                             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Item.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.AddressingsInventoryStart.Addressing.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Item.Id.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "ItemId");
            model.Action = "IndexRecount";
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<StockTaking> GetStockTakingByIdAsync(int stockTakingId)
        {
            return await _context.StockTaking.Include(i => i.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .FirstOrDefaultAsync(st => st.Id == stockTakingId);
        }

        public IEnumerable<StockTaking> GetStockTakingsEnumerableAsync()
        {
            return _context.StockTaking.ToList();
        }

        public async Task<List<StockTaking>> GetAllStockTakingByItemIdAsync(string itemId)
        {
            var result = await _context.StockTaking.Include(i => i.Item)
                                            .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                            .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                            .Include(p => p.PerishableItem)
                                            .Where(st => st.ItemId == itemId).ToListAsync();
            return result;
        }

        public async Task<List<StockTaking>> GetAllStockTakingImportByItemIdAsync(string itemId)
        {
            var result = await _context.StockTaking.Include(i => i.Item)
                                            .Include(a => a.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                            .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                            .Include(p => p.PerishableItem)
                                            .Where(st => st.ItemId == itemId && st.StockTakingObservation == "Contagem do item importada diretamente para o sistema").ToListAsync();
            return result;
        }

        public async Task<List<StockTaking>> GetAllStockTakingAsync()
        {
            return await _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .ToListAsync();

        }

        public async Task<PagingList<StockTaking>> GetStockTakingByPagingAsync(string filter, int pageindex = 1, string sort = "StockTakingDate")
        {
            var result = _context.StockTaking.Include(l => l.Item)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.Addressing)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .AsNoTracking()
                                             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Item.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.AddressingsInventoryStart.Addressing.Name.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "StockTakingDate");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<List<StockTaking>> GetStockTakingByAddressingAsync(int addressingId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .Where(a => a.AddressingsInventoryStart.AddressingId == addressingId).ToListAsync();
        }

        public async Task<StockTaking> GetStockTakingByAddressingAndItemIdAsync(int addressingId, string itemId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .FirstOrDefaultAsync(a => a.AddressingsInventoryStart.AddressingId == addressingId && a.ItemId == itemId);
        }

        public async Task<StockTaking> GetStockTakingByWarehouseAndItemIdAsync(int warehouseId, string itemId)
        {
            return await _context.StockTaking.Include(x => x.Item)
                                             .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                             .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                             .Include(p => p.PerishableItem)
                                             .FirstOrDefaultAsync(a => a.AddressingsInventoryStart.Addressing.WarehouseId == warehouseId && a.ItemId == itemId);
        }

        public int GetCountStockTakingByAddressingAsync(int addressingId)
        {
            return _context.StockTaking.Include(x => x.Item)
                                       .Include(a => a.AddressingsInventoryStart).ThenInclude(inv => inv.InventoryStart)
                                       .Include(i => i.AddressingsInventoryStart).ThenInclude(a => a.InventoryStart)
                                       .Include(p => p.PerishableItem)
                                       .Where(a => a.AddressingsInventoryStart.AddressingId == addressingId).Count();
        }
    }
}
