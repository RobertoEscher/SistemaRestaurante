using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaRestaurante.Domain.Entities;
using SistemaRestaurante.Domain.Interfaces;
using SistemaRestaurante.Infrastructure.Context;

namespace SistemaRestaurante.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly RestauranteDbContext _context;

        public VendaRepository(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<List<VendaPrato>> ObterVendasPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _context.Vendas
                .Include(v => v.Prato)
                    .ThenInclude(p => p.Receita)
                        .ThenInclude(i => i.Produto)
                .Where(v => v.DataVenda >= inicio && v.DataVenda <= fim)
                .ToListAsync();
        }
    }
}