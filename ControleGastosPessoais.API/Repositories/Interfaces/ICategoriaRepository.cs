using ControleGastosPessoais.Shared.DTOs.Categoria;

namespace ControleGastosPessoais.API.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponseDTO>> GetCategorias();
        Task<CategoriaResponseDTO> GetCategoriaById(int id);
        Task AddCategoria(CategoriaRequestDTO categoria);
    }
}

