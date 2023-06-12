using Inventory.Models;
using ReflectionIT.Mvc.Paging;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Inventory.ViewModels
{
    public class StockWithLocationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PagingList<Location> Locations { get; set; }
    }
}
