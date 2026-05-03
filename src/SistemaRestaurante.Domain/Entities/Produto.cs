namespace SistemaRestaurante.Domain.Entities
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public double EstoqueAtual { get; private set; }
        public string UnidadeMedida { get; private set; }
        public double EstoqueMinimo { get; private set; }

        public Produto(int id, string nome, double estoqueAtual, string unidadeMedida, double estoqueMinimo)
        {
            Id = id;
            Nome = nome;
            EstoqueAtual = estoqueAtual;
            UnidadeMedida = unidadeMedida;
            EstoqueMinimo = estoqueMinimo;
        }

        public void AtualizarEstoque(double novaQuantidade)
        {
            EstoqueAtual = novaQuantidade;
        }
    }
}