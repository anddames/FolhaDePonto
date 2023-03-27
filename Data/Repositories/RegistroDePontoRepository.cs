using FolhaDePonto.Models;
using Microsoft.EntityFrameworkCore;

namespace FolhaDePonto.Data.Repositories
{
    public class RegistroDePontoRepository : IRegistroDePontoRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistroDePontoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegistroDePontoModel>> GetAllRegistrosDePontoAsync()
        {
            return await _context.RegistrosDePonto.ToListAsync();
        }

        public async Task<RegistroDePontoModel> GetRegistroDePontoByIdAsync(int id)
        {
            return await _context.RegistrosDePonto.FindAsync(id);
        }

        public async Task<IEnumerable<RegistroDePontoModel>> GetRegistrosDePontoByMonthAsync(int month)
        {
            return await _context.RegistrosDePonto.Where(rp => rp.Data.Month == month).ToListAsync();
        }

        public async Task AdicionarRegistroDePontoAsync(RegistroDePontoModel registroDePonto)
        {
            await _context.RegistrosDePonto.AddAsync(registroDePonto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarRegistroDePontoAsync(RegistroDePontoModel registroDePonto)
        {
            _context.RegistrosDePonto.Update(registroDePonto);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirRegistroDePontoAsync(int UserId)
        {
            var registro = await _context.RegistrosDePonto.FindAsync(UserId);
            _context.RegistrosDePonto.Remove(registro);
            await _context.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }

}

