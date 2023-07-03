using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using System.Globalization;
using CsvHelper;
using Inventory.ViewModels.Imports;

namespace Inventory.Services
{
    public class InventoryMovementService : GeralService, IInventoryMovementService
    {
        private readonly InventoryContext _context;

        public InventoryMovementService(InventoryContext context) : base(context)
        {
            _context = context;
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

            foreach (var item in inventoryMovements)
            {
                InventoryMovement inventoryInsert = new InventoryMovement();
                inventoryInsert.ItemId = item.ItemId;
                inventoryInsert.MovementeType = item.MovementeType;
                inventoryInsert.WarehouseId = item.WarehouseId;
                inventoryInsert.Amount = item.Amount;
                inventoryInsert.MovementDate = item.MovementDate;
                inventoryInsert.ImportDate = importDate;

                _context.InventoryMovement.Add(inventoryInsert);
                await _context.SaveChangesAsync();

                inventoryReturn.Add(inventoryInsert);
            }

            return inventoryReturn;
        }

        public List<InventoryMovement> GetAllInventoryMovementsAsync()
        {
            return _context.InventoryMovement.ToList();
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
