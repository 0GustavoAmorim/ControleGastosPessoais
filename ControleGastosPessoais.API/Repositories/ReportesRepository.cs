using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Dashboard;
using ControleGastosPessoais.Shared.DTOs.Reportes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosPessoais.API.Repositories
{
    public class ReportesRepository : IReportesRepository
    {
        private readonly AppDbContext _context;

        public ReportesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExportacaoGastoDTO>> GetReportesPorPeriodo(DateTime dataInicio, DateTime dataFim, string userId)
        {
            var gastos = await _context.Gastos
                .Include(g => g.Categoria)
                .Where(g => g.UserId == userId &&
                    g.Data >= dataInicio && g.Data <= dataFim)
                .ToListAsync();

            return gastos.Select(g => new ExportacaoGastoDTO
            {
                Descricao = g.Descricao,
                Valor = g.Valor,
                Categoria = g.Categoria.Nome,
                Data = g.Data.ToString()
            }).ToList();
        }
    }
}
