using System;
using System.Collections.Generic;
using SistemaRestaurante.Domain.Entities;
using SistemaRestaurante.Domain.Interfaces;

namespace SistemaRestaurante.Domain.Services
{
    public class SugestaoCompraDomainService : ISugestaoCompraDomainService
    {
        private const double MargemSeguranca = 1.20; // 20% de margem

        public List<(Produto Produto, double ConsumoEstimado, double QuantidadeAComprar)> CalcularSugestoes(
            List<VendaPrato> vendas, 
            List<Produto> produtos)
        {
            var demandaIngredientes = new Dictionary<int, double>();

            // 1. Calcula o consumo total com base no histórico de vendas e receitas
            foreach (var venda in vendas)
            {
                if (venda.Prato?.Receita == null) continue;

                foreach (var item in venda.Prato.Receita)
                {
                    if (demandaIngredientes.ContainsKey(item.ProdutoId))
                        demandaIngredientes[item.ProdutoId] += item.QuantidadeNecessaria * venda.QuantidadeVendida;
                    else
                        demandaIngredientes[item.ProdutoId] = item.QuantidadeNecessaria * venda.QuantidadeVendida;
                }
            }

            var sugestoes = new List<(Produto Produto, double ConsumoEstimado, double QuantidadeAComprar)>();

            // 2. Compara o consumo projetado com o estoque atual
            foreach (var produto in produtos)
            {
                demandaIngredientes.TryGetValue(produto.Id, out double consumoEstimado);
                double necessidadeTotal = consumoEstimado * MargemSeguranca;

                if (produto.EstoqueAtual < necessidadeTotal || produto.EstoqueAtual < produto.EstoqueMinimo)
                {
                    double quantidadeComprar = necessidadeTotal - produto.EstoqueAtual;

                    if (produto.EstoqueAtual + quantidadeComprar < produto.EstoqueMinimo)
                    {
                        quantidadeComprar = produto.EstoqueMinimo - produto.EstoqueAtual;
                    }

                    if (quantidadeComprar > 0)
                    {
                        sugestoes.Add((produto, consumoEstimado, Math.Round(quantidadeComprar, 2)));
                    }
                }
            }

            return sugestoes;
        }
    }
}