using System.Text;
using System.Text.Json;

namespace ControleGastosPessoais.API.Configurations
{
    public class HttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private HttpClient CreateCliente()
        {
            return _httpClientFactory.CreateClient();
        }

        public async Task<T?> GetTAsync<T>(string url)
        {
            var client = CreateCliente();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao acessar {url}: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            using var client = CreateCliente();
            var json = JsonSerializer.Serialize(data);
            var context = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, context);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Erro ao enviar dados para {url}: {response.StatusCode}");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseData, _jsonSerializerOptions);
        }

        public async Task<bool> PutAsync<TRequest>(string url, TRequest data)
        {
            using var client = CreateCliente();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(url, content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string url)
        {
            using var client = CreateCliente();
            var response = await client.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }   
    }
}
