using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Globalization;

namespace Inventory.Services
{
    public class ItemService : GeralService, IItemService
    {
        private readonly InventoryContext _context;

        public ItemService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingList<Item>> GetAllItemsPagingAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Addressings)
                                      .ThenInclude(l => l.Addressing)
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Id.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<PagingList<Item>> GetItemsByAddressingPagingAsync(int addressingnId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Addressings)
                                      .ThenInclude(l => l.Addressing)
                                      .Where(l => l.Addressings.Any(il => il.AddressingId == addressingnId))
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                          (p.Id.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<List<Item>> GetAllItemsByAddressingAsync(int addressingnId)
        {
            var result = await _context.Item.Include(l => l.Addressings)
                                      .ThenInclude(l => l.Addressing)
                                      .Where(l => l.Addressings.Any(il => il.AddressingId == addressingnId)).ToListAsync();

            return result;
        }

        public async Task<PagingList<Item>> GetItemsByWarehousePagingAsync(int warehouseId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Item.Include(l => l.Addressings)
                                      .ThenInclude(l => l.Addressing)
                                      .ThenInclude(w => w.Warehouse)
                                      .Where(a => a.Addressings.Any(a => a.Addressing.WarehouseId == warehouseId))
                                      .AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => (p.Name.ToLower().Contains(filter.ToLower())) ||
                                           (p.Id.ToLower().Contains(filter.ToLower())));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            var result = await _context.Item.Include(l => l.Addressings).ThenInclude(il => il.Addressing).FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<bool> ImportItemAsync(string fileName, string destiny)
        {
            List<ItemImport> items = new List<ItemImport>();

            //Read CSV
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", destiny, fileName);
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var item = csv.GetRecord<ItemImport>();
                    items.Add(item);
                }
            }
            List<Item> itemReturn = new List<Item>();

            foreach (var item in items)
            {
                Item itemInsert = new Item();
                itemInsert.Id = item.Id;
                itemInsert.Name = item.Name;
                itemInsert.UnitOfMeasurement = item.UnitOfMeasurement;
                itemInsert.Quantity = item.Quantity;

                _context.Item.Add(itemInsert);
                _context.SaveChanges();

                ItemsAddressings itemsAddressings = new ItemsAddressings();
                itemsAddressings.ItemId = itemInsert.Id;
                itemsAddressings.AddressingId = item.AddressingId;

                _context.ItemsAddressing.Add(itemsAddressings);
                _context.SaveChanges();
            }
            return true;
        }
    }
}
