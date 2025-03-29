using Blazored.LocalStorage;
using ControleGastosPessoais.Client;
using ControleGastosPessoais.Client.Authentication;
using ControleGastosPessoais.Client.Services;
using ControleGastosPessoais.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.Extensions.DependencyInjection;



var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// MudBlazor
builder.Services.AddMudServices();

// Autenticação
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Handler que injeta o token em cada requisição
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

// HttpClient com token incluso automaticamente
builder.Services.AddHttpClient("API", client =>
{
    client.BaseAddress = new Uri("https://localhost:7085/");
}).AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

// Usa o HttpClient nomeado como padrão para injeção direta
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));

// Serviços que usam o HttpClient
builder.Services.AddScoped<GastosServices>();
builder.Services.AddScoped<CategoriasService>();

await builder.Build().RunAsync();
