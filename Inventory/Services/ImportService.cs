using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
using NuGet.Packaging;
using System.Globalization;

namespace Inventory.Services
{
    public class ImportService : IImportService
    {
        private readonly InventoryContext _context;
        private readonly IItemService _itemService;
        private readonly IAddressingService _addressingService;
        private readonly IWarehouseService _warehouseService;
        private readonly IItemAddressingService _itemAddressingService;

        public ImportService(InventoryContext context,
                             IItemService itemService,
                             IAddressingService addressingService,
                             IWarehouseService warehouseService,
                             IItemAddressingService itemAddressingService)
        {
            _context = context;
            _itemService = itemService;
            _addressingService = addressingService;
            _warehouseService = warehouseService;
            _itemAddressingService = itemAddressingService;
        }

        public async Task<bool> ImportAsync(string fileName, string destiny)
        {
            List<ImportBase> importsList = new List<ImportBase>();

            //Read CSV
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", destiny, fileName);
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var importList = csv.GetRecord<ImportBase>();
                    importsList.Add(importList);
                }
            }

            // Listas para adicionar
            List<ItemBaseImport> itemBaseImport = new List<ItemBaseImport>();
            List<AddressingBaseImport> addressingsBaseImport = new List<AddressingBaseImport>();
            List<Warehouse> warehouseInsert = new List<Warehouse>();


            // Listas para duplicados
            HashSet<string> warehouseNames = new HashSet<string>();
            HashSet<string> addressingNames = new HashSet<string>();

            //Adicionar existentes do sistema
            Dictionary<string, int> warehouseIds = new Dictionary<string, int>();
            foreach (var warehouse in await _warehouseService.GetAllAsync<Warehouse>())
            {
                warehouseIds[warehouse.Name] = warehouse.Id;
                warehouseNames.Add(warehouse.Name);
            }

            Dictionary<string, int> addressingsIds = new Dictionary<string, int>();
            foreach (var addressing in await _addressingService.GetAllAddressingsAsync())
            {
                string name = addressing.Name + addressing.Warehouse.Name;

                addressingsIds[name] = addressing.Id;
                addressingNames.Add(name);
            }

            foreach (var item in importsList)
            {
                Warehouse warehouse = new Warehouse();
                AddressingBaseImport addressingBaseInsert = new AddressingBaseImport();
                ItemBaseImport itemBaseInsert = new ItemBaseImport();

                if (!warehouseNames.Contains(item.WarehouseName))
                {
                    warehouseNames.Add(item.WarehouseName);
                    warehouse.Name = item.WarehouseName;
                    warehouseInsert.Add(warehouse);
                }

                if (!addressingNames.Contains(item.AddressingName + item.WarehouseName))
                {
                    addressingNames.Add(item.AddressingName + item.WarehouseName);
                    addressingBaseInsert.AddressingName = item.AddressingName;
                    addressingBaseInsert.WarehouseName = item.WarehouseName;
                    addressingsBaseImport.Add(addressingBaseInsert);
                }

                itemBaseInsert.Id = item.Id;
                itemBaseInsert.ItemName = item.ItemName;
                itemBaseInsert.AddressingName = item.AddressingName;
                itemBaseInsert.WarehouseName = item.WarehouseName;
                itemBaseInsert.UnitOfMeasurement = item.UnitOfMeasurement;
                itemBaseInsert.Quantity = item.Quantity;
                itemBaseImport.Add(itemBaseInsert);

            }

            await InsertWarehouses(warehouseInsert);
            await InsertAddressings(addressingsBaseImport, warehouseIds);
            await InsertItems(itemBaseImport, addressingNames, addressingsIds);

            return true;
        }

        public async Task<bool> InsertWarehouses(List<Warehouse> warehouseInsert)
        {
            await _context.Warehouse.AddRangeAsync(warehouseInsert);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> InsertAddressings(List<AddressingBaseImport> addressingsBaseImport)
        //{
        //    List<Addressing> addressingsInsert = new List<Addressing>();

        //    foreach (var item in addressingsBaseImport)
        //    {
        //        Addressing addressingInsert = new Addressing();

        //        var warehouse = await _warehouseService.GetWarehouseByName(item.WarehouseName);

        //        addressingInsert.Name = item.AddressingName;
        //        addressingInsert.WarehouseId = warehouse.Id;
        //        addressingsInsert.Add(addressingInsert);
        //    }

        //    await _context.Addressing.AddRangeAsync(addressingsInsert);
        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        public async Task<bool> InsertAddressings(List<AddressingBaseImport> addressingsBaseImport, Dictionary<string, int> warehouseIds)
        {
            List<Addressing> addressingsInsert = new List<Addressing>();

            foreach (var item in addressingsBaseImport)
            {
                Addressing addressingInsert = new Addressing();

                // Obter o ID do armazém do dicionário
                int warehouseId = warehouseIds[item.WarehouseName];

                addressingInsert.Name = item.AddressingName;
                addressingInsert.WarehouseId = warehouseId;
                addressingsInsert.Add(addressingInsert);
            }

            await _context.Addressing.AddRangeAsync(addressingsInsert);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> InsertItems(List<ItemBaseImport> itemBaseImport)
        //{
        //    List<Item> itemInsert = new List<Item>();
        //    List<ItemsAddressings> itemsAddressingsInsert = new List<ItemsAddressings>();

        //    // Listas para duplicados
        //    List<string> duplicateIds = GetDuplicateIds(itemBaseImport);
        //    List<string> fisrtOccurrence = new List<string>();

        //    foreach (var item in itemBaseImport)
        //    {

        //        if (duplicateIds.Contains(item.Id))
        //        {
        //            if (fisrtOccurrence.Contains(item.Id))
        //            {
        //                itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null));
        //                continue;
        //            }
        //            itemInsert.Add(await InsertImportItemAsync(item));
        //            itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null));
        //            fisrtOccurrence.Add(item.Id);
        //            continue;
        //        }
        //        itemInsert.Add(await InsertImportItemAsync(item));
        //        itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(null, item));

        //    }


        //    await _context.Item.AddRangeAsync(itemInsert);
        //    await _context.ItemsAddressing.AddRangeAsync(itemsAddressingsInsert);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public async Task<bool> InsertItems(List<ItemBaseImport> itemBaseImport, HashSet<string> addressingNames, Dictionary<string, int> addressingsIds)
        {
            //Verify item exist
            //List<string> idsExist = new List<string>();
            var idsExist = new HashSet<string>(await _itemService.GetAllItemIdsAsync());


            List<Item> itemInsert = new List<Item>();
            List<ItemsAddressings> itemsAddressingsInsert = new List<ItemsAddressings>();
            List<ItemsAddressings> itemsAddressingsForUpdate = new List<ItemsAddressings>();

            // Listas para duplicados
            List<string> duplicateIds = GetDuplicateIds(itemBaseImport);
            List<string> firstOccurrence = new List<string>();

            // Criar dicionário para mapear a chave composta de nome do addressing e nome do warehouse ao ID do addressing
            Dictionary<(string, string), int> addressingIds = new Dictionary<(string, string), int>();

            // Iterar sobre os itens importados
            foreach (var item in itemBaseImport)
            {
                List<ItemsAddressings> itemsAddressings = new List<ItemsAddressings>();

                foreach (var itemAddressing in itemsAddressings)
                {
                }

                if (duplicateIds.Contains(item.Id) || idsExist.Contains(item.Id))
                {
                    if (firstOccurrence.Contains(item.Id) || idsExist.Contains(item.Id))
                    {
                        // Caso seja duplicado e não seja a primeira ocorrência, cria apenas o ItemsAddressings
                        if (addressingNames.Contains(item.AddressingName + item.WarehouseName))
                        {
                            itemsAddressingsInsert.Add(await InsertItemAddressingsForUpdateAsync(item, addressingsIds[item.AddressingName + item.WarehouseName]));
                            continue;
                        }

                        itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null, addressingIds));
                        continue;
                    }

                    // Primeira ocorrência de um item duplicado, cria o Item e o ItemsAddressings
                    itemInsert.Add(await InsertImportItemAsync(item));
                    itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(item, null, addressingIds));
                    firstOccurrence.Add(item.Id);
                    continue;
                }

                // Item único, cria o Item e o ItemsAddressings
                itemInsert.Add(await InsertImportItemAsync(item));
                itemsAddressingsInsert.Add(await InsertOnlyItemAddressingImportItemAsync(null, item, addressingIds));
            }

            await _context.Item.AddRangeAsync(itemInsert);
            //await _context.ItemsAddressing.AddRangeAsync(itemsAddressingsInsert);
            _context.ItemsAddressing.UpdateRange(itemsAddressingsInsert);
            await _context.SaveChangesAsync();

            return true;
        }

        private List<string> GetDuplicateIds(List<ItemBaseImport> items)
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

        private async Task<Item> InsertImportItemAsync(ItemBaseImport item)
        {
            Item itemReturn = new Item();
            itemReturn.Id = item.Id;
            itemReturn.Name = item.ItemName;
            itemReturn.UnitOfMeasurement = item.UnitOfMeasurement;

            return itemReturn;
        }

        private async Task<ItemsAddressings> InsertOnlyItemAddressingImportItemAsync(ItemBaseImport item, ItemBaseImport itemReturn, Dictionary<(string, string), int> addressingIds)
        {
            ItemsAddressings itemsAddressings = new ItemsAddressings();
            if (itemReturn != null)
            {
                var key = (itemReturn.AddressingName, itemReturn.WarehouseName);
                itemsAddressings.ItemId = itemReturn.Id;
                itemsAddressings.AddressingId = addressingIds[key];
                itemsAddressings.Quantity = itemReturn.Quantity;
            }
            else
            {
                var key = (item.AddressingName, item.WarehouseName);
                itemsAddressings.ItemId = item.Id;
                itemsAddressings.AddressingId = addressingIds[key];
                itemsAddressings.Quantity = item.Quantity;
            }

            return itemsAddressings;
        }

        private async Task<ItemsAddressings> InsertItemAddressingsForUpdateAsync(ItemBaseImport itemAddressing, int addressingId)
        {
            ItemsAddressings itemsAddressings = await _itemAddressingService.GetItemAddressingByIdsAsync(itemAddressing.Id, addressingId);

            itemsAddressings.Quantity = itemAddressing.Quantity;

            return itemsAddressings;
        }

        //private async Task<ItemsAddressings> InsertOnlyItemAddressingImportItemAsync(ItemBaseImport item, ItemBaseImport itemReturn)
        //{
        //    ItemsAddressings itemsAddressings = new ItemsAddressings();
        //    if (itemReturn != null)
        //    {
        //        itemsAddressings.ItemId = itemReturn.Id;
        //        itemsAddressings.AddressingId = itemReturn.AddressingId;
        //        itemsAddressings.Quantity = itemReturn.Quantity;
        //    }
        //    else
        //    {
        //        itemsAddressings.ItemId = item.Id;
        //        itemsAddressings.AddressingId = item.AddressingId;
        //        itemsAddressings.Quantity = item.Quantity;
        //    }

        //    return itemsAddressings;
        //}
    }
}
