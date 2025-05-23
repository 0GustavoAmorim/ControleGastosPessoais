﻿using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Categoria;
using ControleGastosPessoais.Shared.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ControleGastosPessoais.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : BaseController
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IValidator<CategoriaRequestDTO> _validator;


    public CategoriasController(ICategoriaRepository categoriaRepository,
                                IValidator<CategoriaRequestDTO> validator)
    {
        _categoriaRepository = categoriaRepository;
        _validator = validator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetCategorias()
    {
        var categoria = await _categoriaRepository.GetCategorias(UserId);

        if (categoria == null)
        {
            return NotFound();
        }
        return Ok(categoria);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<CategoriaResponseDTO>>> GetCategoriasById(int id)
    {
        var categoria = await _categoriaRepository.GetCategoriaById(id, UserId);
        if (categoria == null)
            return NotFound($"Categoria com ID {id} não encontrada.");

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult> PostCategoria(CategoriaRequestDTO categoria)
    {
        var result = _validator.Validate(categoria);


        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        if (string.IsNullOrWhiteSpace(UserId))
        {
            return Unauthorized();
        }

        await _categoriaRepository.AddCategoria(categoria, UserId);
        return Ok("Categoria cadastrada com sucesso!");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategoria(int id, CategoriaRequestDTO categoria)
    {
        bool categoriaAtualizada = await _categoriaRepository.UpdateCategoria(id, categoria, UserId);
        if (!categoriaAtualizada)
            return NotFound($"Categoria com ID {id} não encontrada.");

        return Ok("Categoria atualizada com sucesso!");
    }

    [HttpPut("{id}/update-gastos/{novaCategoriaId}")]
    public async Task<ActionResult> UpdateGastosCategoria(int id, int novaCategoriaId)
    {
        try
        {
            bool atualizado = await _categoriaRepository.UpdateGastosCategoria(id, novaCategoriaId, UserId);
            if (atualizado)
            {
                return Ok("Gastos atualizados com sucesso.");
            }

            return BadRequest("Erro ao atualizar gastos.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategoria(int id)
    {
        try
        {
            var categoria = await _categoriaRepository.DeleteCategoria(id, UserId);
            if (!categoria)
                return BadRequest("Erro ao excluir categoria");

            return Ok("Categoria excluída com sucesso!");

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("{id}/gastos")]
    public async Task<bool> VerificarGastoCategoria(int id)
    {

        bool temGastos = await _categoriaRepository.VerificarGastosCategoria(id, UserId);
        if (temGastos)
            return temGastos;

        return temGastos;
    }
}
