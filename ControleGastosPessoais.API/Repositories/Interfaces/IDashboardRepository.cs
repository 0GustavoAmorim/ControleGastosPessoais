using ControleGastosPessoais.Shared.DTOs.Dashboard;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<GastoDTO> GetGastoTotal(string userId);
        Task<GastoDTO> GetGastoMensal(string userId);
        Task<GastoDTO> GetGastoPerso(string userId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoria(string userId);
        Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoriaMensal(string userId, int ano = 0);
        Task<IEnumerable<GastoCategoriaDTO>> GetGastoPorCategoriaPerso(string userId, DateTime dataInicio, DateTime dataFim);
    }
}
