using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FolhaDePonto.Models;

namespace FolhaDePonto.Data.Repositories
{
    public interface IRegistroDePontoRepository
    {
        Task<IEnumerable<RegistroDePontoModel>> GetAllRegistrosDePontoAsync();
        Task<RegistroDePontoModel> GetRegistroDePontoByIdAsync(int id);
        Task<IEnumerable<RegistroDePontoModel>> GetRegistrosDePontoByMonthAsync(int month);
        Task AdicionarRegistroDePontoAsync(RegistroDePontoModel registroDePonto);
        Task AtualizarRegistroDePontoAsync(RegistroDePontoModel registroDePonto);
        Task ExcluirRegistroDePontoAsync(int UserId);
        Task<int> SaveChangesAsync();
    }
}
