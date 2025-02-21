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

    public async Task UpdateCategoria(int id, CategoriaRequestDTO categoria)
    {
        var response = await _http.PutAsJsonAsync($"api/categorias/{id}", categoria);
        if (response.IsSuccessStatusCode)
        {
            await GetCategorias();
        }
    }

    public async Task UpdateGastosCategoria(int categoriaParaExcluir, int novaCategoriaId)
    {
        var response = await _http.PutAsJsonAsync($"api/categorias/{categoriaParaExcluir}/update-gastos/{novaCategoriaId}", new {});
        if (response.IsSuccessStatusCode)
        {
            await GetCategorias();
        }
    }

    public async Task DeleteCategoria(int id)
    {
        var response = await _http.DeleteAsync($"api/categorias/{id}");
        if (response.IsSuccessStatusCode)
        {
            await GetCategorias();
        }
    }

    public async Task<bool> CategoriaTemGastos(int id)
    {
        var response = await _http.GetAsync($"api/categorias/{id}/gastos");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<bool>();
    }
}
