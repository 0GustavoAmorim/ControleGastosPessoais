using ControleGastosPessoais.Shared.DTOs.Categoria;
using System.Net.Http.Json;

namespace ControleGastosPessoais.Client.Services;

public class CategoriasService
{
    private readonly HttpClient _http;

    public CategoriasService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<CategoriaResponseDTO>> GetCategorias()
    {
        var categorias = await _http.GetFromJsonAsync<List<CategoriaResponseDTO>>("api/categorias");
        return categorias ?? new List<CategoriaResponseDTO>();
    }

    public async Task AddCategoria(CategoriaRequestDTO novaCategoria)
    {
        var response = await _http.PostAsJsonAsync("api/categorias", novaCategoria);
        if (response.IsSuccessStatusCode)
        {
            await GetCategorias();
        }
    }
}
