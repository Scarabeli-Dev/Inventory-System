using Inventory.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels.Imports
{
    public class ImportBase
    {
        public string Id { get; set; }
        public string ItemName { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        public string WarehouseName { get; set; }
        public string AddressingName { get; set; }
    }
}
