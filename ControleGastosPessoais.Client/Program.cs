using ControleGastosPessoais.Client;
using ControleGastosPessoais.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7085/") });
builder.Services.AddScoped<GastosServices>();
builder.Services.AddScoped<CategoriasService>();


await builder.Build().RunAsync();
