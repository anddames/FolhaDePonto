using Microsoft.EntityFrameworkCore;
using FolhaDePonto.Models;

namespace FolhaDePonto.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioModel>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> UsuarioExisteAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        public async Task<UsuarioModel> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UsuarioModel> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task AdicionarUsuarioAsync(UsuarioModel usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirUsuarioAsync(int id)
        {
            var usuario = await GetUsuarioByIdAsync(id);
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }

    }
}
