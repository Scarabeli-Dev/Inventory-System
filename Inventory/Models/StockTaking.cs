using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class StockTaking
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data da contagem")]
        public DateTime? StockTakingDate { get; set; }

        [Display(Name = "Item")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public string ItemId { get; set; }
        public Item Item { get; set; }

        [Display(Name = "Endereçamento")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        public int AddressingsInventoryStartId { get; set; }
        public AddressingsInventoryStart AddressingsInventoryStart { get; set; }

        [Display(Name = "Quantidade na Contagem")]
        [Required(ErrorMessage = "{0} é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public decimal StockTakingQuantity { get; set; }

        [Display(Name = "Data de Fabricação")]
        public DateTime? FabricationDate { get; set; }

        [Display(Name = "Data de Validade")]
        public DateTime? ExpirationDate { get; set; }

        [Display(Name = "Lote")]
        [StringLength(30, ErrorMessage = "Máximo de {1} caracteres!")]
        public string? ItemBatch { get; set; }

        [Display(Name = "Observação")]
        [StringLength(250, ErrorMessage = "Máximo de {1} caracteres!")]
        public string StockTakingObservation { get; set; }

        //[Display(Name = "Contagem para o inventário")]
        //[Required(ErrorMessage = "{0} é obrigatório")]
        //public int InventoryStartId { get; set; }
        //public InventoryStart InventoryStart { get; set; }

        public int NumberOfCount { get; set; }
    }
}
