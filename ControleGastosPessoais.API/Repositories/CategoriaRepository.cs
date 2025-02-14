using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Categoria;
using ControleGastosPessoais.Shared.Models;
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
    }
}
