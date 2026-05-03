using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaRestaurante.Domain.Entities;

namespace SistemaRestaurante.Domain.Interfaces
{
    public interface IVendaRepository
    {
        Task<List<VendaPrato>> ObterVendasPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}