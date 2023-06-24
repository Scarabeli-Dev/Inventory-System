using CsvHelper;
using Inventory.Data;
using Inventory.Helpers.Exceptions;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Collections.Generic;
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

            // Listas para adicionar
            List<Item> itemInsert = new List<Item>();
            List<ItemsAddressings> itemsAddressingsInsert = new List<ItemsAddressings>();

            // Listas para duplicados
            List<string> duplicateIds = GetDuplicateIds(items);
            List<string> fisrtOccurrence = new List<string>();

            foreach (var item in items)
            {

                if (duplicateIds.Contains(item.Id))
                {
                    if (fisrtOccurrence.Contains(item.Id))
                    {
                        itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null));
                        continue;
                    }
                    itemInsert.Add(await InsertImportItemAsync(item));
                    itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null));
                    fisrtOccurrence.Add(item.Id);
                    continue;
                }
                itemInsert.Add(await InsertImportItemAsync(item));
                itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(null, item));

            }


            await _context.Item.AddRangeAsync(itemInsert);
            await _context.ItemsAddressing.AddRangeAsync(itemsAddressingsInsert);
            await _context.SaveChangesAsync();

            //if (!await InsertRangeAsync(_context.Item, itemInsert))
            //{
            //    return false;
            //}
            //await InsertRangeAsync(_context.ItemsAddressing, itemsAddressingsInsert);
            return true;
        }

        private List<string> GetDuplicateIds(List<ItemImport> items)
        {
            List<string> duplicateIds = new List<string>();
            HashSet<string> uniqueIds = new HashSet<string>();

            foreach (var item in items)
            {
                if (!uniqueIds.Add(item.Id))
                {
                    // O ID já existe, adiciona à lista de IDs duplicados
                    duplicateIds.Add(item.Id);
                }
            }

            return duplicateIds;
        }

        private async Task<Item> InsertImportItemAsync(ItemImport item)
        {
            Item itemReturn = new Item();
            itemReturn.Id = item.Id;
            itemReturn.Name = item.Name;
            itemReturn.UnitOfMeasurement = item.UnitOfMeasurement;

            return itemReturn;
        }

        private async Task<ItemsAddressings> InsertOnlyItemAddressingImportItemAsync(ItemImport item, ItemImport itemReturn)
        {
            ItemsAddressings itemsAddressings = new ItemsAddressings();
            if (itemReturn != null)
            {
                itemsAddressings.ItemId = itemReturn.Id;
                itemsAddressings.AddressingId = itemReturn.AddressingId;
                itemsAddressings.Quantity = itemReturn.Quantity;
            }
            else
            {
                itemsAddressings.ItemId = item.Id;
                itemsAddressings.AddressingId = item.AddressingId;
                itemsAddressings.Quantity = item.Quantity;
            }

            return itemsAddressings;
        }

        private async Task<bool> InsertRangeAsync<T>(DbSet<T> dbSet, List<T> entities) where T : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    var entry = _context.Entry(entity);
                    if (entry.State == EntityState.Detached)
                    {
                        dbSet.Add(entity);
                    }
                    else
                    {
                        dbSet.Update(entity);
                    }
                }
                await _context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        private List<string> GetDuplicateIdsDataBase()
        {
            List<Item> result = _context.Item.ToList();
            List<string> duplicateIds = new List<string>();

            foreach (var item in result)
            {
                duplicateIds.Add(item.Id);
            }

            return duplicateIds;
        }
    }
}
