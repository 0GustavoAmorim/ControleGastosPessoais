using ControleGastosPessoais.Shared.DTOs.Categoria;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponseDTO>> GetCategorias(string userId);
        Task<CategoriaResponseDTO> GetCategoriaById(int id, string userId);
        Task AddCategoria(CategoriaRequestDTO categoria, string userId);
        Task<bool> UpdateCategoria(int id, CategoriaRequestDTO categoria, string userId);
        Task<bool> UpdateGastosCategoria(int categoriaId, int novaCategoriaId, string userId);
        Task<bool> DeleteCategoria(int id, string userId);
        Task<bool> VerificarGastosCategoria(int id, string userId);
    }
}

