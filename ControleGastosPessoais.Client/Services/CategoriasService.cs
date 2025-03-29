using ControleGastosPessoais.Shared.DTOs.Categoria;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ControleGastosPessoais.Client.Services;

public class CategoriasService
{
    private readonly HttpClient _http;
    private readonly NavigationManager _nav;

    public CategoriasService(HttpClient http
                            ,NavigationManager nav)
    {
        _http = http;
        _nav = nav;
    }

    public async Task<List<CategoriaResponseDTO>> GetCategorias()
    {
        //var categorias = await _http.GetFromJsonAsync<List<CategoriaResponseDTO>>("api/categorias");
        var response = await _http.GetAsync("api/categorias");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _nav.NavigateTo("/notauthorized");
            return new();
        }

        if (!response.IsSuccessStatusCode)
        {
            // opcional: você pode logar o erro ou redirecionar para uma página de erro
            return new();
        }

        var result = await response.Content.ReadFromJsonAsync<List<CategoriaResponseDTO>>();
        return result ?? new();

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
