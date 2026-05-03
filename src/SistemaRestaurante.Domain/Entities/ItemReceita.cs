namespace SistemaRestaurante.Domain.Entities
{
    public class ItemReceita
    {
        public int Id { get; private set; }
        public int PratoId { get; private set; }
        public Prato Prato { get; private set; } = null!;
        public int ProdutoId { get; private set; }
        public double QuantidadeNecessaria { get; private set; }
        public Produto Produto { get; private set; } = null!;

        public ItemReceita(int id, int pratoId, int produtoId, double quantidadeNecessaria)
        {
            Id = id;
            PratoId = pratoId;
            ProdutoId = produtoId;
            QuantidadeNecessaria = quantidadeNecessaria;
        }
    }
}