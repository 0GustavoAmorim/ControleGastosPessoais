using ClosedXML.Excel;
using ControleGastosPessoais.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastosPessoais.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : BaseController
    {
        private readonly IReportesRepository _reportesRepository;

        public ReportesController(IReportesRepository reportesRepository)
        {
            _reportesRepository = reportesRepository;
        }

        [HttpGet("exportar-periodo")]
        public async Task<IActionResult> ExportarPorPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        {
            var gastos = await _reportesRepository.GetReportesPorPeriodo(dataInicio, dataFim, UserId);

            if (!gastos.Any())
                return NotFound("Nenhum gasto encontrado nesse período.");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório Por Período");

            // 🎨 Cores e estilo do cabeçalho
            worksheet.Cell(1, 1).Value = "Descrição";
            worksheet.Cell(1, 2).Value = "Valor (R$)";
            worksheet.Cell(1, 3).Value = "Categoria";
            worksheet.Cell(1, 4).Value = "Data";

            var headerRange = worksheet.Range(1, 1, 1, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // 📄 Dados
            int linha = 2;
            foreach (var gasto in gastos)
            {
                worksheet.Cell(linha, 1).Value = gasto.Descricao;
                worksheet.Cell(linha, 2).Value = gasto.Valor;
                worksheet.Cell(linha, 3).Value = gasto.Categoria;
                worksheet.Cell(linha, 4).Value = gasto.Data;

                linha++;
            }

            // 💡 Estilizar o conteúdo
            var dataRange = worksheet.Range(2, 1, linha - 1, 4);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            dataRange.Style.Font.FontSize = 11;

            // 💰 Formatação numérica
            worksheet.Column(2).Style.NumberFormat.Format = "R$ #,##0.00";

            // 📐 Ajustar largura
            worksheet.Columns().AdjustToContents();

            // 🧊 Rodapé com total (opcional)
            worksheet.Cell(linha + 1, 1).Value = "Total:";
            worksheet.Cell(linha + 1, 2).FormulaA1 = $"=SUM(B2:B{linha - 1})";
            worksheet.Cell(linha + 1, 2).Style.Font.Bold = true;
            worksheet.Cell(linha + 1, 2).Style.NumberFormat.Format = "R$ #,##0.00";
            worksheet.Cell(linha + 1, 1).Style.Font.Bold = true;
            worksheet.Range(linha + 1, 1, linha + 1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;


            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var nomeArquivo = $"Gastos_{dataInicio:yyyyMMdd}_{dataFim:yyyyMMdd}.xlsx";

            return File(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                nomeArquivo);
        }
    }
}
