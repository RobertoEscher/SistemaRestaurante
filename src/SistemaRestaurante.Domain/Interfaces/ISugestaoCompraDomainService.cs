namespace SistemaRestaurante.Domain.Interfaces
{
    using SistemaRestaurante.Domain.Entities;
    using System.Collections.Generic;

    public interface ISugestaoCompraDomainService
    {
        List<(Produto Produto, double ConsumoEstimado, double QuantidadeAComprar)> CalcularSugestoes(
            List<VendaPrato> vendas, 
            List<Produto> produtos);
    }
}