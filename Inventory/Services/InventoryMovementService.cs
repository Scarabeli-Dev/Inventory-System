using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using System.Globalization;
using CsvHelper;
using Inventory.ViewModels;
using Inventory.ViewModels.Imports;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Services
{
    public class InventoryMovementService : GeralService, IInventoryMovementService
    {
        private readonly InventoryContext _context;
        private readonly IWarehouseService _warehouseService;
        private readonly IItemService _itemService;

        public InventoryMovementService(InventoryContext context, IWarehouseService warehouseService, IItemService itemService) : base(context)
        {
            _context = context;
            _warehouseService = warehouseService;
            _itemService = itemService;
        }

        public List<InventoryMovement> GetInventoryMovementsByItemId(string itemId)
        {
            return _context.InventoryMovement.Where(i => i.ItemId == itemId).ToList();
        }

        public async Task<List<InventoryMovement>> GetAllInventoryMovementsAsync()
        {
            return await _context.InventoryMovement.ToListAsync();
        }

        public async Task<List<InventoryMovement>> ImportInventoryMovementsAsync(string fileName, string destiny)
        {
            List<InventoryMovementImport> inventoryMovements = new List<InventoryMovementImport>();
            DateTime importDate = DateTime.Now;

            //Read CSV
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", destiny, fileName);
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var inventoryMovement = csv.GetRecord<InventoryMovementImport>();
                    inventoryMovements.Add(inventoryMovement);
                }
            }
            List<InventoryMovement> inventoryReturn = new List<InventoryMovement>();

            var allItemsIds = await _itemService.GetAllItemIdsAsync();

            // Criar dicionário para mapear nome do armazém ao ID
            Dictionary<string, int> warehouseIds = new Dictionary<string, int>();

            // Obter todos os armazéns existentes
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            // Popula o dicionário com os IDs dos armazéns
            foreach (var warehouse in warehouses)
            {
                warehouseIds[warehouse.Name] = warehouse.Id;
            }

            foreach (var item in inventoryMovements)
            {
                if (!allItemsIds.Contains(item.ItemId))
                {
                    continue;
                }
                InventoryMovement inventoryInsert = new InventoryMovement();
                inventoryInsert.ItemId = item.ItemId;
                inventoryInsert.MovementeType = item.MovementeType;
                inventoryInsert.WarehouseId = warehouseIds[item.WarehouseName];
                inventoryInsert.Amount = item.Amount;
                inventoryInsert.MovementDate = item.MovementDate;
                inventoryInsert.ImportDate = importDate;

                inventoryReturn.Add(inventoryInsert);
            }

            _context.InventoryMovement.AddRange(inventoryReturn);
            await _context.SaveChangesAsync();

            return inventoryReturn;
        }


        //public List<InventoryMovement> GenereteCSV(List<InventoryMovement> inventoryMovements, string destiny)
        //{
        //    //Read CSV
        //    var path = $"{Directory.GetCurrentDirectory()}{@"\Resources\"}" + destiny;
        //    using (var write = new StreamWriter(path))
        //    using (var csv = new CsvWriter(write, CultureInfo.InvariantCulture))
        //    {
        //        csv.WriteRecords(inventoryMovements);
        //    }
        //    return inventoryMovements;
        //}
    }
}
