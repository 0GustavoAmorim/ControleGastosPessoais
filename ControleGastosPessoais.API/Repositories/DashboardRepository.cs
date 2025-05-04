using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Dashboard;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControleGastosPessoais.API.Repositories

{
    public class DashboardRepository : IDashboardRepository  
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Gastos

        public async Task<GastoDTO> GetGastoTotal(string userId)
        {
            var totalGastos = await _context.Gastos
                .Where(g => g.UserId == userId)
                .SumAsync(g => g.Valor);
            
            return new GastoDTO
            {
                Total = totalGastos
            };
        }

        public async Task<GastoDTO> GetGastoMensal(string userId)
        {
            var mesAtual = DateTime.Now.Month;
            var anoAtual = DateTime.Now.Year;
            
            var totalGastosMensais = await _context.Gastos
                .Where(g => g.UserId == userId && g.Data.HasValue && g.Data.Value.Month == mesAtual && g.Data.Value.Year == anoAtual)
                .SumAsync(g => g.Valor);
           
            return new GastoDTO
            {
                Total = totalGastosMensais
            };
        }

        public async Task<GastoDTO> GetGastoPerso(string userId, DateTime dataInicio, DateTime dataFim)
        {
            // Ajustar a data final para incluir todos os registros do dia
            var dataFimAjustada = dataFim.Date.AddDays(1).AddSeconds(-1);

            // Garantir que a data inicial comece do início do dia
            var dataInicioAjustada = dataInicio.Date;

            var totalGastosPeriodo = await _context.Gastos
                .Where(g => g.UserId == userId &&
                           g.Data.HasValue &&
                           g.Data.Value >= dataInicioAjustada &&
                           g.Data.Value <= dataFimAjustada)
                .SumAsync(g => g.Valor);

            return new GastoDTO
            {
                Total = totalGastosPeriodo
            };
        }


        #endregion

        #region Gasto por categoria

        public async Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoria(string userId)
        {

            var gastosPorCategoria = await _context.Gastos
                .Where(g => g.UserId == userId)
                .GroupBy(g => g.Categoria.Nome)
                .Select(g => new GastoCategoriaDTO
                {
                    Categoria = g.Key,
                    Total = g.Sum(x => x.Valor)
                })
                .ToListAsync();

            return gastosPorCategoria;
        }

        public async Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoriaMensal(string userId, int ano = 0)
        {
            if (ano == 0)
                ano = DateTime.Now.Year;

            var gastosPorCategoriaMensal = await _context.Gastos
                .Where(g => g.UserId == userId && g.Data.HasValue && g.Data.Value.Year == ano)
                .GroupBy(g => new { g.Data.Value.Month, g.Categoria.Nome })
                .Select(g => new GastoCategoriaDTO
                {
                    Mes = g.Key.Month,
                    Categoria = g.Key.Nome,
                    Total = g.Sum(x => x.Valor)
                })
                .OrderBy(g => g.Mes)
                .ToListAsync();

            return gastosPorCategoriaMensal;
        }

        public async Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoriaPerso(string userId, DateTime dataInicio, DateTime dataFim)
        {
            // Ajustar a data final para incluir todos os registros do dia
            var dataFimAjustada = dataFim.Date.AddDays(1).AddSeconds(-1);

            // Garantir que a data inicial comece do início do dia
            var dataInicioAjustada = dataInicio.Date;

            var gastos = await _context.Gastos
                .Where(g => g.UserId == userId &&
                           g.Data.HasValue &&
                           g.Data.Value >= dataInicioAjustada &&
                           g.Data.Value <= dataFimAjustada)
                .GroupBy(g => g.Categoria.Nome)
                .Select(g => new GastoCategoriaDTO
                {
                    Categoria = g.Key,
                    Total = g.Sum(x => x.Valor)
                })
                .ToListAsync();

            return gastos;
        }
        #endregion
    }
}
