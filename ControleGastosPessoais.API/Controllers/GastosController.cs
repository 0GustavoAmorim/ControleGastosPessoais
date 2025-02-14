using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Gasto;
using ControleGastosPessoais.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosPessoais.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GastosController : ControllerBase
{
    private readonly IGastoRepository _gastoRepository;

    public GastosController(IGastoRepository gastoRepository)
    {
        _gastoRepository = gastoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GastoResponseDTO>>> GetGastos()
    {
        var gastos = await _gastoRepository.GetGastos();
        return Ok(gastos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GastoResponseDTO>> GetGastoById(int id)
    {
        var gasto = await _gastoRepository.GetGastoById(id);
        if (gasto == null)
        {
            return NotFound($"Gasto com ID {id} não encontrado.");
        }
        return Ok(gasto);
    }

    [HttpPost]
    public async Task<ActionResult> PostGasto(GastoRequestDTO gasto)
    {
        if (gasto == null)
        {
            return BadRequest("Os dados do gasto são obrigatórios.");
        }

        if (gasto.CategoriaId <= 0)
        {
            return BadRequest("O ID da categoria é inválido.");
        }

        await _gastoRepository.AddGasto(gasto);
        return Ok("Gasto cadastrado com sucesso!");
    }

}
