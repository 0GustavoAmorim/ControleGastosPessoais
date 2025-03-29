using Blazored.LocalStorage;
using ControleGastosPessoais.Client.Authentication;
using ControleGastosPessoais.Client.Services.Interfaces;
using ControleGastosPessoais.Shared.Models.AuthenticationModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ControleGastosPessoais.Client.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _nav;

    public AuthService(HttpClient httpClient
                      , AuthenticationStateProvider authenticationStateProvider
                      , ILocalStorageService localStorage
                      , NavigationManager nav)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _nav = nav;
    }
    public async Task<RegisterResult> Register(RegisterModel registerModel)
    {
        var messageResult = await _httpClient.PostAsJsonAsync("api/account", registerModel);
        var result = await messageResult.Content.ReadFromJsonAsync<RegisterResult>();
        return result;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);

        var response = await _httpClient.PostAsync("api/login", // Verifique se a rota está certa
            new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            // Se a resposta não for JSON válido, retorna como mensagem de erro
            return new LoginResult
            {
                Successful = false,
                Error = responseContent
            };
        }

        LoginResult loginResult;

        try
        {
            loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new LoginResult { Successful = false, Error = "Erro ao processar resposta do servidor." };
        }
        catch (JsonException)
        {
            return new LoginResult
            {
                Successful = false,
                Error = "Erro ao interpretar a resposta da API. Verifique se o servidor está respondendo corretamente."
            };
        }

        await _localStorage.SetItemAsync("authToken", loginResult.Token);

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult.Token);

        return loginResult;
    }


    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;

        _nav.NavigateTo("/", forceLoad: true);
    }
}