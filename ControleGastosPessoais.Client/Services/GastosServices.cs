using ControleGastosPessoais.Shared.DTOs.Gasto;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace ControleGastosPessoais.Client.Services;

public class GastosServices
{
    private readonly HttpClient _http;
    private readonly NavigationManager _nav;

    public GastosServices(HttpClient http
                         ,NavigationManager nav)
    {
        _http = http;
        _nav = nav;
    }

    public async Task<List<GastoResponseDTO>> GetGastos()
    {
        var response = await _http.GetAsync("api/gastos");

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _nav.NavigateTo("/notauthorized");
            return new(); // evita erro após redirecionamento
        }

        if (!response.IsSuccessStatusCode)
        {
            // Aqui você pode logar, exibir mensagem, redirecionar ou só retornar lista vazia
            return new();
        }

        var result = await response.Content.ReadFromJsonAsync<List<GastoResponseDTO>>();
        return result ?? new();
    }

    public async Task AddGasto(GastoRequestDTO gastoDTO)
    {
        var response = await _http.PostAsJsonAsync("api/gastos", gastoDTO);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao adicionar gasto: {error}");
        }
    }

    public async Task UpdateGasto(int id, GastoRequestDTO gastoDTO)
    {
        var response = await _http.PutAsJsonAsync($"api/gastos/{id}", gastoDTO);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao atualizar gasto: {error}");
        }
    }

    public async Task DeleteGasto(int id)
    {
        var response = await _http.DeleteAsync($"api/gastos/{id}");
        if (response.IsSuccessStatusCode)
        {
            await GetGastos();
        }
    }

}
