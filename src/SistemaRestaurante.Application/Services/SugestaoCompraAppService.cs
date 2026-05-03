using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaRestaurante.Application.Dtos;
using SistemaRestaurante.Application.Interfaces;
using SistemaRestaurante.Domain.Interfaces;
using SistemaRestaurante.Domain.Services;

namespace SistemaRestaurante.Application.Services
{
    public class SugestaoCompraAppService : ISugestaoCompraAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IVendaRepository _vendaRepository;
        private readonly ISugestaoCompraDomainService _sugestaoDomainService;

        public SugestaoCompraAppService(
            IProdutoRepository produtoRepository,
            IVendaRepository vendaRepository,
            ISugestaoCompraDomainService sugestaoDomainService)
        {
            _produtoRepository = produtoRepository;
            _vendaRepository = vendaRepository;
            _sugestaoDomainService = sugestaoDomainService;
        }

        public async Task<List<SugestaoCompraDto>> ObterSugestoesDaSemanaAsync()
        {
            var dataInicio = DateTime.Now.AddDays(-7);
            var dataFim = DateTime.Now;

            // Busca os dados da infraestrutura
            var vendas = await _vendaRepository.ObterVendasPorPeriodoAsync(dataInicio, dataFim);
            var produtos = await _produtoRepository.ObterTodosAsync();

            // Executa a regra de negócio do domínio
            var resultadoCalculado = _sugestaoDomainService.CalcularSugestoes(vendas, produtos);

            // Mapeia para o DTO de retorno
            return resultadoCalculado.Select(item => new SugestaoCompraDto
            {
                ProdutoId = item.Produto.Id,
                NomeProduto = item.Produto.Nome,
                EstoqueAtual = item.Produto.EstoqueAtual,
                ConsumoEstimado = item.ConsumoEstimado,
                QuantidadeParaComprar = item.QuantidadeAComprar,
                UnidadeMedida = item.Produto.UnidadeMedida
            }).ToList();
        }
    }
}