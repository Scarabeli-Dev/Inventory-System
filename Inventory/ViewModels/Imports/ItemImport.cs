using Inventory.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels.Imports
{
    public class ItemImport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int AddressingId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
    }
}
