using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Gasto;
using ControleGastosPessoais.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosPessoais.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GastosController : BaseController
{
    private readonly IGastoRepository _gastoRepository;

    public GastosController(IGastoRepository gastoRepository)
    {
        _gastoRepository = gastoRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GastoResponseDTO>>> GetGastos()
    {
        var gastos = await _gastoRepository.GetGastos(UserId);
        if (gastos == null)
        {
            return NotFound();
        }

        return Ok(gastos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GastoResponseDTO>> GetGastoById(int id)
    {
        var gasto = await _gastoRepository.GetGastoById(id, UserId);
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

        await _gastoRepository.AddGasto(gasto, UserId);
        return Ok("Gasto cadastrado com sucesso!");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGasto(int id, GastoRequestDTO gasto)
    {
        if (gasto == null)
        {
            return BadRequest("Os dados do gasto são obrigatórios.");
        }

        if (gasto.CategoriaId <= 0)
        {
            return BadRequest("O ID da categoria é inválido.");
        }

        bool gastoAtualizado = await _gastoRepository.UpdateGasto(id, gasto, UserId);
        if (!gastoAtualizado)
        {
            return NotFound($"Gasto com ID {id} não encontrado.");
        }
        return Ok("Gasto atualizado com sucesso!");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGasto(int id)
    {
        await _gastoRepository.DeleteGasto(id, UserId);
        return Ok("Gasto excluído com sucesso!");
    }

}
