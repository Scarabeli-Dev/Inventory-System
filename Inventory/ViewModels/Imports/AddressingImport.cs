using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels.Imports
{
    public class AddressingImport
    {
        public string Name { get; set; }
        public int WarehouseId { get; set; }
    }
}
