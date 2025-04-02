using ControleGastosPessoais.Shared.DTOs.Gasto;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface IGastoRepository
    {
        Task<List<GastoResponseDTO>> GetGastos(string userId);
        Task<GastoResponseDTO> GetGastoById(int id, string userId);
        Task AddGasto(GastoRequestDTO gasto, string userId);
        Task<bool> UpdateGasto(int id, GastoRequestDTO gasto, string userId);
        Task DeleteGasto(int id, string userId);
    }

}
