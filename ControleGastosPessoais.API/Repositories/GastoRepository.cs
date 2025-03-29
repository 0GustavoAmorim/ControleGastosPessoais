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

        public async Task<List<GastoResponseDTO>> GetGastos(string userId)
        {
            var gastos = await _context.Gastos
                .Include(g => g.Categoria)
                .Where(g => g.UserId == userId)
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

        public async Task<GastoResponseDTO> GetGastoById(int id, string userId)
        {
            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

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

        public async Task AddGasto(GastoRequestDTO gasto, string userId)
        {
            var novoGasto = new Gasto
            {
                Descricao = gasto.Descricao,
                Valor = gasto.Valor,
                Data = gasto.Data,
                CategoriaId = gasto.CategoriaId,
                UserId = userId
            };

            _context.Gastos.Add(novoGasto);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> UpdateGasto(int id, GastoRequestDTO gasto, string userId)
        {
            var gastoAtual = await _context.Gastos.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

            if (gastoAtual == null)
            {
                return false;
            }

            gastoAtual.Descricao = gasto.Descricao;
            gastoAtual.Valor = gasto.Valor;
            gastoAtual.Data = gasto.Data;
            gastoAtual.CategoriaId = gasto.CategoriaId;
            gastoAtual.UserId = userId;

            await _context.SaveChangesAsync();
            return true;
        }   

        public async Task DeleteGasto(int id, string userId)
        {
            var gasto = await _context.Gastos.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
