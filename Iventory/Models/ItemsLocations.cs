using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class ItemsLocations
    {
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}
