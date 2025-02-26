using ControleGastosPessoais.Shared.DTOs.Gasto;
using System.Net.Http.Json;

namespace ControleGastosPessoais.Client.Services;

public class GastosServices
{
    private readonly HttpClient _http;

    public GastosServices(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<GastoResponseDTO>> GetGastos()
    {
        return await _http.GetFromJsonAsync<List<GastoResponseDTO>>("api/gastos");
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
