using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using System.Globalization;

namespace Inventory.Services
{
    public class WarehouseService : GeralService, IWarehouseService
    {
        private readonly InventoryContext _context;

        public WarehouseService(InventoryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagingList<Warehouse>> GetAllWarehousesAsync(string filter, int pageindex = 1, string sort = "Name")
        {
            var result = _context.Warehouse.Include(l => l.Addressings)
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

        public async Task<Warehouse> GetWarehouseByIdAsync(int? id)
        {
            var result = await _context.Warehouse.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<bool> ImportWarehouseAsync(string fileName, string destiny)
        {
            List<WarehouseImport> warehouses = new List<WarehouseImport>();

            //Read CSV
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", destiny, fileName);
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var warehouse = csv.GetRecord<WarehouseImport>();
                    warehouses.Add(warehouse);
                }
            }
            List<Warehouse> warehouseReturn = new List<Warehouse>();

            foreach (var item in warehouses)
            {
                Warehouse warehouseInsert = new Warehouse();
                warehouseInsert.Name = item.Name;

                _context.Warehouse.Add(warehouseInsert);
                _context.SaveChanges();
            }
            return true;
        }
    }
}
