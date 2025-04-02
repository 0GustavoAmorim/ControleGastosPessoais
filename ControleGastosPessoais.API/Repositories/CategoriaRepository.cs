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

        public async Task<List<CategoriaResponseDTO>> GetCategorias(string userId)
        {
            return await _context.Categorias
                .Where(c => c.UserId == userId && c.Nome != "Sem Categoria")
                .Select(c => new CategoriaResponseDTO
                {
                    Id = c.Id,
                    Nome = c.Nome
                })
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<CategoriaResponseDTO> GetCategoriaById(int id, string userId)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            return categoria != null ? new CategoriaResponseDTO
            {
                Id = categoria.Id,
                Nome = categoria.Nome
            }
                : null;
        }

        public async Task AddCategoria(CategoriaRequestDTO categoria, string userId)
        {
            bool categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == categoria.Nome.ToLower() && c.UserId == userId);

            if (categoriaExiste)
            {
                throw new Exception("Já existe uma categoria com esse nome.");
            }

            var novaCategoria = new Categoria
            {
                Nome = categoria.Nome,
                UserId = userId
            };

            _context.Categorias.Add(novaCategoria);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCategoria(int id, CategoriaRequestDTO categoria, string userId)
        {
            var categoriaExistente = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (categoriaExistente == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            bool categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == categoria.Nome.ToLower() && c.Id != id && c.UserId == userId);

            if (categoriaExiste)
            {
                throw new Exception("Já existe uma categoria com esse nome.");
            }

            categoriaExistente.Nome = categoria.Nome;
            await _context.SaveChangesAsync();

            return true;
        }

        //Atualiza categoria do gasto com categoria existente
        public async Task<bool> UpdateGastosCategoria(int categoriaId, int novaCategoriaId, string userId)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == categoriaId && c.UserId == userId);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            var novaCategoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == novaCategoriaId && c.UserId == userId);
            if (novaCategoria == null)
            {
                throw new KeyNotFoundException("Nova categoria não encontrada.");
            }

            var gastos = await _context.Gastos.Where(g => g.CategoriaId == categoriaId && g.UserId == userId)
                .ToListAsync();

            foreach (var gasto in gastos)
            {
                gasto.CategoriaId = novaCategoriaId;
            }
             
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoria(int id, string userId)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (categoria == null)
            {
                throw new KeyNotFoundException("Categoria não encontrada.");
            }

            bool temGasto = await _context.Gastos.AnyAsync(g => g.CategoriaId == id && g.UserId == userId);
            if (temGasto)
            {
                throw new InvalidOperationException("Não é possível excluir a categoria, pois ela está associada a um gasto que possui gastos.");
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> VerificarGastosCategoria(int id, string userId)
        {
            return await _context.Gastos.AnyAsync(g => g.CategoriaId == id && g.UserId == userId);
        }   
    }
}
