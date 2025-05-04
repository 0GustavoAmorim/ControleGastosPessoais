using ControleGastosPessoais.Shared.DTOs.Dashboard;
using Microsoft.AspNetCore.Components;
using Nextended.Core.Extensions;
using System.Net.Http.Json;

namespace ControleGastosPessoais.Client.Services
{
    public class DashboardService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _nav;

        public DashboardService(HttpClient httpClient
                                , NavigationManager navigationManager)
        {
            _http = httpClient;
            _nav = navigationManager;
        }

        #region Dash Gastos

        public async Task<GastoDTO> GetGastoTotal()
        {
            var response = await _http.GetAsync("api/dashboard/gasto_total");

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }

            var result = await response.Content.ReadFromJsonAsync<GastoDTO>();
            return result ?? new();
        }

        public async Task<GastoDTO> GetGastoMensal()
        {
            var response = await _http.GetAsync("api/dashboard/gasto-mensal");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }
            var result = await response.Content.ReadFromJsonAsync<GastoDTO>();
            return result ?? new();
        }   

        public async Task<GastoDTO> GetGastoPerso(DateTime dataInicio, DateTime dataFim)
        {
            var response = await _http.GetAsync($"api/dashboard/gasto_perso?dataInicio={dataInicio}&dataFim={dataFim}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }
            var result = await response.Content.ReadFromJsonAsync<GastoDTO>();
            return result ?? new();
        }

        #endregion

        #region Dash Gasto por categoria
        public async Task<List<GastoCategoriaDTO>> GetGastosPorCategoriaAsync()
        {
            var response = await _http.GetAsync("api/dashboard/gasto-por-categoria");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }

            if (!response.IsSuccessStatusCode)
            {

                return new();
            }

            var result = await response.Content.ReadFromJsonAsync<List<GastoCategoriaDTO>>();
            return result ?? new();
        }

        public async Task<List<GastoCategoriaDTO>> GetGastoCategoriaMensal(int ano = 0)
        {
            var response = await _http.GetAsync($"api/dashboard/gasto_por_categoria_mensal?ano={ano}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }
            
            var result = await response.Content.ReadFromJsonAsync<List<GastoCategoriaDTO>>();
            return result ?? new();
        }

        public async Task<List<GastoCategoriaDTO>> GetGastoCategoriaPerso(DateTime dataInicio, DateTime dataFim)
        {
            var response = await _http.GetAsync($"api/dashboard/gasto_por_categoria_perso?dataInicio={dataInicio}&dataFim={dataFim}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _nav.NavigateTo("/notauthorized");
                return new();
            }
            var result = await response.Content.ReadFromJsonAsync<List<GastoCategoriaDTO>>();
            return result ?? new();
        }

        #endregion

    }
}
