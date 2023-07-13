using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class InventoryStart
    {
        public int Id { get; set; }

        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime InventoryStartDate { get; set; }

        [Display(Name = "Estimativa de termino")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StockTakingFinishDate { get; set; }

        [Display(Name = "Depósito")]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [Display(Name = "Concluído")]
        public bool IsCompleted { get; set; }
        public IEnumerable<AddressingsInventoryStart> Addressings { get; set; }
    }
}
