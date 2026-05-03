using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaRestaurante.Domain.Entities;

namespace SistemaRestaurante.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> ObterTodosAsync();
        Task AtualizarEstoqueAsync(int produtoId, double novaQuantidade);
    }
}