using CsvHelper.Configuration.Attributes;
using Inventory.Models.Enums;
using Inventory.ViewModels.ViewModelEnums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.ViewModels.Exports
{
    public class CsvExportReport
    {
        [Name("CÓDIGO")]
        public string Codigo { get; set; }

        [Name("PRODUTO")]
        public string Produto { get; set; }

        [Name("U.M.")]
        public UnitOfMeasurement UnidadeMedida { get; set; }

        [Name("ESTOQUE SISTEMA")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstoqueSistema { get; set; }

        [Name("CONTAGEM")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Contagem { get; set; }

        [Name("DIVERGÊNCIA")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Divergencia { get; set; }

        [Name("STATUS DO ESTOQUE")]
        public string StatusEstoque { get; set; }

        [Name("STATUS DO ENDEREÇAMENTO")]
        public string StatusEnderecamento { get; set; }

        // Endereçamento Sistema
        [Name("EnderecamentoSistema1")]
        public string EnderecamentoSistema1 { get; set; }

        [Name("Dep1")]
        public string Dep1 { get; set; }

        [Name("Quantidade1")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade1 { get; set; }

        [Name("EnderecamentoSistema2")]
        public string EnderecamentoSistema2 { get; set; }

        [Name("Dep2")]
        public string Dep2 { get; set; }

        [Name("Quantidade2")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade2 { get; set; }

        // Endereçamento Contagem
        [Name("EnderecamentoContagem1")]
        public string EnderecamentoContagem1 { get; set; }

        [Name("Dep3")]
        public string Dep3 { get; set; }

        [Name("Quantidade3")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade3 { get; set; }

        [Name("EnderecamentoContagem2")]
        public string EnderecamentoContagem2 { get; set; }

        [Name("Dep4")]
        public string Dep4 { get; set; }

        [Name("Quantidade4")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade4 { get; set; }

        [Name("OBSERVAÇÕES")]
        public string Observacoes { get; set; }
    }
}
