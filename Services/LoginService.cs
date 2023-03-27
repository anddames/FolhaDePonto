using FolhaDePonto.Data.Repositories;
using FolhaDePonto.Models;
using Microsoft.AspNetCore.Identity;

namespace FolhaDePonto.Services
{
    public class LoginService : ILoginService
    {
        public Task<bool> LoginAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}


