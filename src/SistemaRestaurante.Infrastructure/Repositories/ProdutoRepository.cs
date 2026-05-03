using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaRestaurante.Domain.Entities;
using SistemaRestaurante.Domain.Interfaces;
using SistemaRestaurante.Infrastructure.Context;

namespace SistemaRestaurante.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly RestauranteDbContext _context;

        public ProdutoRepository(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.ToListAsync();
        }

        public async Task AtualizarEstoqueAsync(int produtoId, double novaQuantidade)
        {
            var produto = await _context.Produtos.FindAsync(produtoId);
            if (produto != null)
            {
                produto.AtualizarEstoque(novaQuantidade);
                await _context.SaveChangesAsync();
            }
        }
    }
}