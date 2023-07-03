using CsvHelper;
using Inventory.Data;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Globalization;

namespace Inventory.Services
{
    public class AddressingService : GeralService, IAddressingService
    {
        private readonly InventoryContext _context;

        public AddressingService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Addressing> AddAddressingAsync(Addressing addressing)
        {

            _context.Add(addressing);

            await _context.SaveChangesAsync();

            return addressing;

        }

        public async Task<PageList<Addressing>> GetAllPageListDataTable(PageParams pageParams)
        {
            IQueryable<Addressing> query = _context.Addressing.Include(l => l.Item)
                                                              .ThenInclude(i => i.Item)
                                                              .Include(w => w.Warehouse);

            query = query.AsNoTracking()
                         .Where(a => a.Name.ToLower().Contains(pageParams.Term.ToLower()));

            return await PageList<Addressing>.CreateAsync(query, pageParams.PageNumber, query.Count());
        }

        public async Task<PagingList<Addressing>> GetAllAddressingsByPagingAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Addressing.Include(l => l.Item)
                                          .ThenInclude(i => i.Item)
                                          .AsNoTracking()
                                          .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Name.ToLower().Contains(filter.ToLower()));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<PagingList<Addressing>> GetAddressingsByWarehouseIdAsync(int warehouseId, string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Addressing.Include(l => l.Item)
                                          .ThenInclude(i => i.Item)
                                          .Where(s => s.WarehouseId == warehouseId)
                                          .AsNoTracking()
                                          .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                result = result.Where(p => p.Name.ToLower().Contains(filter.ToLower()));
            }

            var model = await PagingList.CreateAsync(result, 10, pageindex, sort, "Name");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return model;
        }

        public async Task<List<Addressing>> GetAllAddressingsByWarehouseIdAsync(int warehouseId)
        {
            var result = await _context.Addressing.Include(l => l.Item)
                                                  .ThenInclude(i => i.Item)
                                                  .Where(s => s.WarehouseId == warehouseId)
                                                  .ToListAsync();


            return result;
        }

        public async Task<Addressing> GetAddressingByIdAsync(int id)
        {
            var result = await _context.Addressing.Include(l => l.Item).ThenInclude(il => il.Item).FirstOrDefaultAsync(m => m.Id == id);

            return result;
        }

        public async Task<Addressing> GetAddressingByItemIdAsync(string id)
        {
            var result = await _context.Addressing.Include(l => l.Item).ThenInclude(il => il.Item).FirstOrDefaultAsync(m => m.Item.Any(a => a.Item.Id == id));

            return result;
        }

        public async Task<bool> ImportAddressingAsync(string fileName, string destiny)
        {
            List<AddressingImport> addressings = new List<AddressingImport>();

            //Read CSV
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", destiny, fileName);
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var item = csv.GetRecord<AddressingImport>();
                    addressings.Add(item);
                }
            }

            // Listas para adicionar
            List<Addressing> addressingInsert = new List<Addressing>();

            foreach (var item in addressings)
            {

                Addressing addressingReturn = new Addressing();
                addressingReturn.Name = item.Name;
                addressingReturn.WarehouseId = item.WarehouseId;

                addressingInsert.Add(addressingReturn);
            }

            await _context.Addressing.AddRangeAsync(addressingInsert);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
