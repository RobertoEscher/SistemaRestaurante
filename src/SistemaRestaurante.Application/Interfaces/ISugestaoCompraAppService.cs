using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaRestaurante.Application.Dtos;

namespace SistemaRestaurante.Application.Interfaces
{
    public interface ISugestaoCompraAppService
    {
        Task<List<SugestaoCompraDto>> ObterSugestoesDaSemanaAsync();
    }
}