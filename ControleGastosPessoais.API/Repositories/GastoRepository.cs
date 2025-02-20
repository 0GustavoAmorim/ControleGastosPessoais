using ControleGastosPessoais.API.Data;
using ControleGastosPessoais.API.Repositories.Interfaces;
using ControleGastosPessoais.Shared.DTOs.Categoria;
using ControleGastosPessoais.Shared.DTOs.Gasto;
using ControleGastosPessoais.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastosPessoais.API.Repositories
{
    public class GastoRepository : IGastoRepository
    {
        private readonly AppDbContext _context;

        public GastoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GastoResponseDTO>> GetGastos()
        {
            var gastos = await _context.Gastos
                .Include(g => g.Categoria)
                .Select(g => new GastoResponseDTO
                {
                    Id = g.Id,
                    Descricao = g.Descricao,
                    Valor = g.Valor,
                    Data = g.Data,
                    CategoriaNome = g.Categoria.Nome
                })
                .OrderByDescending(g => g.Id)
                .ToListAsync();

            return gastos;
        }

        public async Task<GastoResponseDTO> GetGastoById(int id)
        {
            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .FirstOrDefaultAsync(g => g.Id == id);

            return gasto != null
                ? new GastoResponseDTO
                {
                    Id = gasto.Id,
                    Descricao = gasto.Descricao,
                    Valor = gasto.Valor,
                    Data = gasto.Data,
                    CategoriaNome = gasto.Categoria.Nome
                } : null;
        }

        public async Task AddGasto(GastoRequestDTO gasto)
        {
            var novoGasto = new Gasto
            {
                Descricao = gasto.Descricao,
                Valor = gasto.Valor,
                Data = gasto.Data,
                CategoriaId = gasto.CategoriaId
            };

            _context.Gastos.Add(novoGasto);
            await _context.SaveChangesAsync();
        }

      

    }
}
