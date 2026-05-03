namespace SistemaRestaurante.Application.Dtos
{
    public class SugestaoCompraDto
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public double EstoqueAtual { get; set; }
        public double ConsumoEstimado { get; set; }
        public double QuantidadeParaComprar { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }
}