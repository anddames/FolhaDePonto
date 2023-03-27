using System.Threading.Tasks;

namespace FolhaDePonto.Services
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(string password);
        Task LogoutAsync();
    }
}
