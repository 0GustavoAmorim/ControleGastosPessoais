using ControleGastosPessoais.Shared.DTOs.Gasto;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface IGastoRepository
    {
        Task<List<GastoResponseDTO>> GetGastos();
        Task<GastoResponseDTO> GetGastoById(int id);
        Task AddGasto(GastoRequestDTO gasto);
        Task<bool> UpdateGasto(int id, GastoRequestDTO gasto);
        Task DeleteGasto(int id);
    }

}
