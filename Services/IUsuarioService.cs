using FolhaDePonto.Models;

namespace FolhaDePonto.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioModel>> GetUsuariosAsync();
        Task<UsuarioModel> GetUsuarioByIdAsync(int id);
        Task<UsuarioModel> CriarUsuarioAsync(UsuarioModel usuario);
        Task AtualizarUsuarioAsync(UsuarioModel usuario);
        Task ExcluirUsuarioAsync(UsuarioModel usuario);
        Task<UsuarioModel> AutenticarUsuarioAsync(string email, string senha);
        Task<bool> VerificarEmailExistenteAsync(string email);
        UsuarioModel ObterUsuarioLogado();
    }

}
