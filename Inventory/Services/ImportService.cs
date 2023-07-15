using CsvHelper;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels.Imports;
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
            foreach (var warehouse in await _warehouseService.GetAllAsync<Warehouse>())
            {
                warehouseNames.Add(warehouse.Name);
            }

            Dictionary<string, int> addressingsIds = new Dictionary<string, int>();
            var addressings = await _addressingService.GetAllAddressingsAsync();
            foreach (var addressing in addressings)
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
            await InsertAddressings(addressingsBaseImport);
            await InsertItems(itemBaseImport, addressingNames);

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

        public async Task<bool> InsertAddressings(List<AddressingBaseImport> addressingsBaseImport)
        {
            List<Addressing> addressingsInsert = new List<Addressing>();

            // Criar dicionário para mapear nome do armazém ao ID
            Dictionary<string, int> warehouseIds = new Dictionary<string, int>();

            // Obter todos os armazéns existentes
            var warehouses = await _warehouseService.GetAllAsync<Warehouse>();

            // Popula o dicionário com os IDs dos armazéns
            foreach (var warehouse in warehouses)
            {
                warehouseIds[warehouse.Name] = warehouse.Id;
            }

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

        public async Task<bool> InsertItems(List<ItemBaseImport> itemBaseImport, HashSet<string> addressingNames)
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
            Dictionary<string, int> addressingsIds = new Dictionary<string, int>();


            // Obter todos os addressings existentes
            var addressings = await _addressingService.GetAllAsync<Addressing>();

            // Popula o dicionário com os IDs dos addressings usando a chave composta de nome do addressing e nome do warehouse
            foreach (var addressing in addressings)
            {
                var key = (addressing.Name, addressing.Warehouse.Name);
                addressingIds[key] = addressing.Id;

                string name = addressing.Name + addressing.Warehouse.Name;

                addressingsIds[name] = addressing.Id;
            }

            // Iterar sobre os itens importados
            foreach (var item in itemBaseImport)
            {

                if (duplicateIds.Contains(item.Id) || idsExist.Contains(item.Id))
                {
                    if (firstOccurrence.Contains(item.Id) || idsExist.Contains(item.Id))
                    {
                        // Caso seja duplicado e não seja a primeira ocorrência, cria apenas o ItemsAddressings
                        if (addressingNames.Contains(item.AddressingName + item.WarehouseName))
                        {
                            ItemsAddressings itemAddressing = await _itemAddressingService.GetItemAddressingByIdsAsync(item.Id, addressingsIds[item.AddressingName + item.WarehouseName]);
                            if (itemAddressing != null)
                            {
                                itemsAddressingsInsert.Add(InsertItemAddressingsForUpdateAsync(item, itemAddressing));
                                continue;
                            }
                        }

                        itemsAddressingsInsert.Add(InsertOnlyItemAddressingImportItemAsync(item, null, addressingIds));
                        continue;
                    }

                    // Primeira ocorrência de um item duplicado, cria o Item e o ItemsAddressings
                    itemInsert.Add(InsertImportItemAsync(item));
                    itemsAddressingsInsert.Add(InsertOnlyItemAddressingImportItemAsync(item, null, addressingIds));
                    firstOccurrence.Add(item.Id);
                    continue;
                }

                // Item único, cria o Item e o ItemsAddressings
                itemInsert.Add(InsertImportItemAsync(item));
                itemsAddressingsInsert.Add(InsertOnlyItemAddressingImportItemAsync(null, item, addressingIds));
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

        private Item InsertImportItemAsync(ItemBaseImport item)
        {
            Item itemReturn = new Item();
            itemReturn.Id = item.Id;
            itemReturn.Name = item.ItemName;
            itemReturn.UnitOfMeasurement = item.UnitOfMeasurement;

            return itemReturn;
        }

        private ItemsAddressings InsertOnlyItemAddressingImportItemAsync(ItemBaseImport item, ItemBaseImport itemReturn, Dictionary<(string, string), int> addressingIds)
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

        private ItemsAddressings InsertItemAddressingsForUpdateAsync(ItemBaseImport itemAddressing, ItemsAddressings itemAddressingInsert)
        {
            itemAddressingInsert.Quantity = itemAddressing.Quantity;

            return itemAddressingInsert;
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
