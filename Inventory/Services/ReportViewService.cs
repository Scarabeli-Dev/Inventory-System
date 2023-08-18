using CsvHelper.Configuration;
using CsvHelper;
using Inventory.Data;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services.Interfaces;
using Inventory.ViewModels;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using ReflectionIT.Mvc.Paging;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using Inventory.ViewModels.Exports;

namespace Inventory.Services
{
    public class ReportViewService : IReportViewService
    {
        private readonly InventoryContext _context;
        private readonly IInventoryMovementService _inventoryMovementService;
        private readonly IStockTakingService _stockTakingService;
        private readonly IItemService _itemService;
        private readonly IItemAddressingService _itemAddressingService;

        private readonly List<FinalReportViewModel> _finalReportViewModel;

        public ReportViewService(IInventoryMovementService inventoryMovementService,
                                 IStockTakingService stockTakingService,
                                 IItemService itemService,
                                 IItemAddressingService itemAddressingService,
                                 InventoryContext context)
        {
            _inventoryMovementService = inventoryMovementService;
            _stockTakingService = stockTakingService;
            _itemService = itemService;
            _itemAddressingService = itemAddressingService;
            _context = context;

            _finalReportViewModel = GetSampleData();
        }

        private List<FinalReportViewModel> GetSampleData()
        {
            List<FinalReportViewModel> sampleData = new List<FinalReportViewModel>();
            var allItems = _itemService.GetAllItemsAsync();
            foreach (var item in allItems)
            {
                FinalReportViewModel finalReport = new FinalReportViewModel();

                finalReport.ItemId = item.Id;
                finalReport.ItemName = item.Name;
                finalReport.SystemQuantity = 0;
                finalReport.QuantityStockTaking = 0;

                finalReport.StockTakings = item.StockTaking;
                if (item.StockTaking.Count() > 0)
                {
                    finalReport.QuantityStockTaking = item.StockTaking.Sum(q => q.StockTakingQuantity);
                }

                finalReport.Addressings = item.Addressings;
                if (item.Addressings.Count() > 0)
                {
                    finalReport.SystemQuantity = item.Addressings.Sum(q => q.Quantity);
                }

                //Verificar quantidade com movimentação
                if (item.InventoryMovement.Count() > 0)
                {
                    List<InventoryMovement> itemMovementList = new List<InventoryMovement>();
                    HashSet<int> movementIndex = new HashSet<int>();

                    decimal movementIn = 0;
                    decimal movementOut = 0;

                    foreach (var movement in item.InventoryMovement)
                    {
                        foreach (var stocktaking in item.StockTaking)
                        {
                            if (stocktaking.StockTakingDate > movement.MovementDate && !movementIndex.Contains(movement.Id))
                            {
                                movementIndex.Add(movement.Id);
                                itemMovementList.Add(movement);
                                if (movement.MovementeType == Models.Enums.MovementeType.E)
                                {
                                    movementIn = movementIn + movement.Amount;
                                }
                                if (movement.MovementeType == Models.Enums.MovementeType.S)
                                {
                                    movementOut = movementOut + movement.Amount;
                                }
                            }
                        }
                    }
                    finalReport.InventoryMovement = itemMovementList;
                    finalReport.SystemQuantity = item.Addressings.Sum(q => q.Quantity) + movementIn - movementOut;
                }

                finalReport.Divergence = finalReport.QuantityStockTaking - finalReport.SystemQuantity;


                if (finalReport.StockTakings.Count() > 0)
                {
                    if (finalReport.SystemQuantity == finalReport.QuantityStockTaking)
                    {
                        finalReport.StockSituation = StockSituation.Regular;
                    }
                    else if (finalReport.Divergence < 0)
                    {
                        finalReport.StockSituation = StockSituation.HigherThanRegistered;
                    }
                    else if (finalReport.Divergence > 0)
                    {
                        finalReport.StockSituation = StockSituation.LowerThanRegistered;
                    }
                    else
                    {
                        finalReport.StockSituation = StockSituation.ItemNoCount;
                        finalReport.AddressingSituation = AddressingSituation.ItemNoCount;
                        sampleData.Add(finalReport);
                        continue;
                    }
                }
                else
                {
                    finalReport.StockSituation = StockSituation.ItemNoCount;
                    finalReport.AddressingSituation = AddressingSituation.ItemNoCount;
                    sampleData.Add(finalReport);
                    continue;
                }

                List<int> stockTakingIdsConference = new List<int>();
                List<int> addressingsIdsConference = new List<int>();

                foreach (var stockTakingConference in finalReport.StockTakings)
                {
                    stockTakingIdsConference.Add(stockTakingConference.AddressingsInventoryStart.AddressingId);
                }

                foreach (var addressingsConference in finalReport.Addressings)
                {
                    addressingsIdsConference.Add(addressingsConference.AddressingId);
                }

                if (addressingsIdsConference.SequenceEqual(stockTakingIdsConference))
                {
                    finalReport.AddressingSituation = AddressingSituation.Regular;
                }
                else if (!finalReport.StockTakings.Any(st => finalReport.Addressings.Any(ad => st.AddressingsInventoryStart.AddressingId == ad.AddressingId)) && finalReport.StockTakings.Count() > 0)
                {
                    finalReport.AddressingSituation = AddressingSituation.ItemInDivergentAddress;
                }
                else if (finalReport.Addressings.Sum(q => q.Quantity) == 0)
                {
                    finalReport.AddressingSituation = AddressingSituation.ItemNoAddressRecord;
                }
                else if (!addressingsIdsConference.SequenceEqual(stockTakingIdsConference))
                {
                    finalReport.AddressingSituation = AddressingSituation.ItemStoredInMoreThanOneAddress;
                }

                sampleData.Add(finalReport);
            }

            return sampleData;
        }

        public PagingList<FinalReportViewModel> ReportWithMovementation(string filter, int pageSize = 10, int pageindex = 1, string sortExpression = "ItemName", int warehouseId = 0, int stockSituation = -2, int addressingSituation = -2)
        {
            var qry = from fr in _finalReportViewModel
                      where fr.ItemId != null
                      select fr;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                qry = qry.Where(p => (p.ItemName.ToLower().Contains(filter.ToLower())) ||
                                           (p.ItemId.ToLower().Contains(filter.ToLower())));
            }

            if (warehouseId != 0)
            {
                qry = qry.Where(aw => aw.Addressings.Any(w => w.Id == warehouseId) ||
                                aw.StockTakings.Any(w => w.AddressingsInventoryStart.Addressing.WarehouseId == warehouseId));
            }
            if (stockSituation != -2)
            {
                StockSituation stockSituationEnum = (StockSituation)stockSituation;
                if (stockSituation == -1)
                {
                    qry = qry.Where(ss => ss.StockSituation != StockSituation.ItemNoCount);
                }
                else
                {
                    qry = qry.Where(ss => ss.StockSituation == stockSituationEnum);
                }
            }
            if (addressingSituation != -2)
            {
                AddressingSituation addressingSituationEnum = (AddressingSituation)addressingSituation;
                qry = qry.Where(ss => ss.AddressingSituation == addressingSituationEnum);
            }


            var model = PagingList.Create(qry, pageSize, pageindex, sortExpression, "ItemName");
            model.RouteValue = new RouteValueDictionary { { "filter", filter }, { "pageSize", pageSize }, { "warehouseId", warehouseId }, { "stockSituation", stockSituation }, { "addressingSituation", addressingSituation } };
            return model;
        }

        public void ExportToCSV()
        {
            var sampleData = GetSampleData();

            var records = new List<CsvExportReport>();

            foreach (var item in sampleData)
            {
                var row = new CsvExportReport
                {
                    Codigo = item.ItemId,
                    Produto = item.ItemName,
                    UnidadeMedida = item.UnitOfMeasurement,
                    EstoqueSistema = item.SystemQuantity,
                    Contagem = item.QuantityStockTaking,
                    Divergencia = item.Divergence,
                    StatusEstoque = item.StockSituation.GetDisplayName(), // Utilize o método para obter o DisplayName
                    StatusEnderecamento = item.AddressingSituation.GetDisplayName(), // Utilize o método para obter o DisplayName
                    Observacoes = string.Join("; ", item.StockTakings.Select(st => st.StockTakingObservation)),
                    EnderecamentoSistema1 = GetAddressingSystemString(item.Addressings, 0), // Utilize uma função auxiliar para obter a string de endereçamento
                    Dep1 = GetDepSystemString(item.Addressings, 0), // Utilize uma função auxiliar para obter a string de depósito
                    Quantidade1 = GetQuantitySystem(item.Addressings, 0), // Utilize uma função auxiliar para obter a quantidade
                    EnderecamentoSistema2 = GetAddressingSystemString(item.Addressings, 1), // Utilize uma função auxiliar para obter a string de endereçamento
                    Dep2 = GetDepSystemString(item.Addressings, 1), // Utilize uma função auxiliar para obter a string de depósito
                    Quantidade2 = GetQuantitySystem(item.Addressings, 1), // Utilize uma função auxiliar para obter a quantidade
                    EnderecamentoContagem1 = GetAddressingCountString(item.StockTakings, 0), // Utilize uma função auxiliar para obter a string de endereçamento
                    Dep3 = GetDepCountString(item.StockTakings, 0), // Utilize uma função auxiliar para obter a string de depósito
                    Quantidade3 = GetQuantityCount(item.StockTakings, 0), // Utilize uma função auxiliar para obter a quantidade
                    EnderecamentoContagem2 = GetAddressingCountString(item.StockTakings, 1), // Utilize uma função auxiliar para obter a string de endereçamento
                    Dep4 = GetDepCountString(item.StockTakings, 1), // Utilize uma função auxiliar para obter a string de depósito
                    Quantidade4 = GetQuantityCount(item.StockTakings, 1) // Utilize uma função auxiliar para obter a quantidade
                };

                records.Add(row);
            }

            using var writer = new StreamWriter("report.csv");
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            csv.WriteRecords(records);
        }

        // Funções auxiliares para obter os valores de endereçamento, depósito e quantidade
        private string GetAddressingSystemString(IEnumerable<ItemsAddressings> addressings, int index)
        {
            if (index < addressings.Count())
            {
                return addressings.ElementAt(index).Addressing.Name;
            }
            return "";
        }

        private string GetDepSystemString(IEnumerable<ItemsAddressings> addressings, int index)
        {
            if (index < addressings.Count())
            {
                return addressings.ElementAt(index).Addressing.Warehouse.Name;
            }
            return "";
        }

        private decimal GetQuantitySystem(IEnumerable<ItemsAddressings> addressings, int index)
        {
            if (index < addressings.Count())
            {
                return addressings.ElementAt(index).Quantity;
            }
            return 0;
        }

        private string GetAddressingCountString(IEnumerable<StockTaking> stockTakings, int index)
        {
            if (index < stockTakings.Count())
            {
                return stockTakings.ElementAt(index).AddressingsInventoryStart.Addressing.Name;
            }
            return "";
        }

        private string GetDepCountString(IEnumerable<StockTaking> stockTakings, int index)
        {
            if (index < stockTakings.Count())
            {
                return stockTakings.ElementAt(index).AddressingsInventoryStart.Addressing.Warehouse.Name;
            }
            return "";
        }

        private decimal GetQuantityCount(IEnumerable<StockTaking> stockTakings, int index)
        {
            if (index < stockTakings.Count())
            {
                return stockTakings.ElementAt(index).StockTakingQuantity;
            }
            return 0;
        }
    }
}