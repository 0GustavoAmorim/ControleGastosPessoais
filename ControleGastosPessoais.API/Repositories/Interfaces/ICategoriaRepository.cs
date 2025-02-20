using ControleGastosPessoais.Shared.DTOs.Categoria;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponseDTO>> GetCategorias();
        Task<CategoriaResponseDTO> GetCategoriaById(int id);
        Task AddCategoria(CategoriaRequestDTO categoria);
        Task<bool> UpdateCategoria(int id, CategoriaRequestDTO categoria);
        Task<bool> UpdateGastosCategoria(int categoriaId, int novaCategoriaId);
        Task<bool> DeleteCategoria(int id);
        Task<bool> VerificarGastosCategoria(int id);
    }
}

