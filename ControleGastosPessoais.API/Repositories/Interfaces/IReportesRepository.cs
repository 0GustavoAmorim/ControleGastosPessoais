using ControleGastosPessoais.Shared.DTOs.Dashboard;
using ControleGastosPessoais.Shared.DTOs.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface IReportesRepository
    {
        Task<List<ExportacaoGastoDTO>> GetReportesPorPeriodo(DateTime dataInicio, [FromQuery] DateTime dataFim, string userId);
    }
}
