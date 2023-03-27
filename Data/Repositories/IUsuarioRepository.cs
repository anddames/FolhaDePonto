using FolhaDePonto.Models;

namespace FolhaDePonto.Data.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<UsuarioModel>> GetAllUsuariosAsync();
        Task<UsuarioModel> GetUsuarioByIdAsync(int id);
        Task<UsuarioModel> GetUsuarioByEmailAsync(string email);
        Task<bool> UsuarioExisteAsync(string email);
        Task AdicionarUsuarioAsync(UsuarioModel usuario);
        Task AtualizarUsuarioAsync(UsuarioModel usuario);
        Task ExcluirUsuarioAsync(int id);
    }
}
