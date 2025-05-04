using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosPessoais.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        #region Dash Gastos

        [Authorize]
        [HttpGet("gasto_total")]
        public async Task<ActionResult<GastoDTO>> GetGastoTotal()
        {
            var gastoTotal = await _dashboardRepository.GetGastoTotal(UserId);

            if (gastoTotal == null)
            {
                return NotFound("Nenhum gasto encontrado.");
            }
            return Ok(gastoTotal);
        }

        [HttpGet("gasto-mensal")]
        public async Task<ActionResult<GastoDTO>> GetGastoMensal()
        {
            var gastoMensal = await _dashboardRepository.GetGastoMensal(UserId);
            if (gastoMensal == null)
            {
                return NotFound("Nenhum gasto encontrado no mês atual.");
            }
            return Ok(gastoMensal);
        }

        [HttpGet("gasto_perso")]
        public async Task<ActionResult<GastoDTO>> GetGastoPerso(DateTime dataInicio, DateTime dataFim)
        {
            var gastoPerso = await _dashboardRepository.GetGastoPerso(UserId, dataInicio, dataFim);
            if (gastoPerso == null)
            {
                return NotFound("Nenhum gasto encontrado no período especificado.");
            }

            return Ok(gastoPerso);
        }

        #endregion

        #region Dash Gasto por categoria

        [HttpGet("gasto-por-categoria")]
        public async Task<ActionResult<IEnumerable<GastoCategoriaDTO>>> GetGastoPorCategoria()
        {
            var gastosPorCategoria = await _dashboardRepository.GetGastoPorCategoria(UserId);

            if (gastosPorCategoria == null || !gastosPorCategoria.Any())
            {
                return NotFound("Nenhum gasto encontrado por categoria.");
            }

            return Ok(gastosPorCategoria);
        }

        [HttpGet("gasto_por_categoria_mensal")]
        public async Task<ActionResult<IEnumerable<GastoCategoriaDTO>>> GetGastoPorCategoriaMensal(int ano = 0)
        {
            var gastos = await _dashboardRepository.GetGastoPorCategoriaMensal(UserId, ano);

            if (!gastos.Any())
            {
                return NotFound("Nenhum gasto encontrado no periodo");
            }

            return Ok(gastos);
        }

        [HttpGet("gasto_por_categoria_perso")]
        public async Task<ActionResult<IEnumerable<GastoCategoriaDTO>>> GetGastoPorCategoriaPerso(DateTime dataInicio, DateTime dataFim)
        {
            var gastos = await _dashboardRepository.GetGastoPorCategoriaPerso(UserId, dataInicio, dataFim);
            if (!gastos.Any())
            {
                return NotFound("Nenhum gasto por categoria encontrado no período especificado.");
            }
            return Ok(gastos);
        }

        #endregion
    }
}