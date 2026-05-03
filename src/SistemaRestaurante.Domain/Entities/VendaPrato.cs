using System;

namespace SistemaRestaurante.Domain.Entities
{
    public class VendaPrato
    {
        public int Id { get; private set; }
        public int PratoId { get; private set; }
        public Prato Prato { get; private set; } = null!;
        public int QuantidadeVendida { get; private set; }
        public DateTime DataVenda { get; private set; }

        public VendaPrato(int id, int pratoId, int quantidadeVendida, DateTime dataVenda)
        {
            Id = id;
            PratoId = pratoId;
            QuantidadeVendida = quantidadeVendida;
            DataVenda = dataVenda;
        }
    }
}