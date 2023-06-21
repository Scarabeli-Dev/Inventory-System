using Inventory.Models.Enums;

namespace Inventory.ViewModels.Imports
{
    public class ItemImport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UnitOfMeasurement UnitOfMeasurement { get; set; }
        public int AddressingId { get; set; }
        public decimal Quantity { get; set; }
    }
}
