using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Categoria;
using ControleGastosPessoais.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosPessoais.API.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaResponseDTO>> GetCategorias()
        {
            return await _context.Categorias
                .Select(c => new CategoriaResponseDTO
                {
                    Id = c.Id,
                    Nome = c.Nome
                })
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<CategoriaResponseDTO> GetCategoriaById(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            return categoria != null ? new CategoriaResponseDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome
            }
                : null;
        }

        public async Task AddCategoria(CategoriaRequestDTO categoria)
        {
            bool categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == categoria.Nome.ToLower());

            if (categoriaExiste)
            {
                throw new Exception("Já existe uma categoria com esse nome.");
            }

            var novaCategoria = new Categoria
            {
                Nome = categoria.Nome
            };
            _context.Categorias.Add(novaCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCategoria(int id, CategoriaRequestDTO categoria)
        {
            var categoriaExistente = await _context.Categorias.FindAsync(id);
            if (categoriaExistente == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            bool categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == categoria.Nome.ToLower() && c.Id != id);

            if (categoriaExiste)
            {
                throw new Exception("Já existe uma categoria com esse nome.");
            }

            categoriaExistente.Nome = categoria.Nome;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateGastosCategoria(int categoriaId, int novaCategoriaId)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaId);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            var novaCategoria = await _context.Categorias.FindAsync(novaCategoriaId);
            if (novaCategoria == null)
            {
                throw new KeyNotFoundException("Nova categoria não encontrada.");
            }

            var gastos = await _context.Gastos.Where(g => g.CategoriaId == categoriaId)
                .ToListAsync();

            foreach (var gasto in gastos)
            {
                gasto.CategoriaId = novaCategoriaId;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            bool temGasto = await _context.Gastos.AnyAsync(g => g.CategoriaId == id);
            if (temGasto)
            {
                throw new InvalidOperationException("Não é possível excluir a categoria, pois ela está associada a um gasto que possui gastos.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VerificarGastosCategoria(int id)
        {
            return await _context.Gastos.AnyAsync(g => g.CategoriaId == id);
        }   
    }
}
