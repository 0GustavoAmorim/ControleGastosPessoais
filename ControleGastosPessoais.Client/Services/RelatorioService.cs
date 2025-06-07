using Microsoft.JSInterop;

public class RelatorioService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;

    public RelatorioService(HttpClient http, IJSRuntime js)
    {
        _http = http;
        _js = js;
    }

    public async Task ExportarExcelPorPeriodo(DateTime dataInicio, DateTime dataFim)
    {
        var url = $"api/reportes/exportar-periodo?dataInicio={dataInicio:yyyy-MM-dd}&dataFim={dataFim:yyyy-MM-dd}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            var erro = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao gerar relatório: {erro}");
        }

        var bytes = await response.Content.ReadAsByteArrayAsync();
        var fileName = $"Gastos_{dataInicio:yyyyMMdd}_{dataFim:yyyyMMdd}.xlsx";

        using var streamRef = new DotNetStreamReference(new MemoryStream(bytes));
        await _js.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }


}
