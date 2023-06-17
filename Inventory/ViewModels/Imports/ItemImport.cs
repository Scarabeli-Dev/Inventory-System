using Inventory.Models.Enums;
using Inventory.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels.Imports
{
    public class ItemImport
    {
        public string Name { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int AddressingId { get; set; }
        public decimal Quantity { get; set; }
    }
}
