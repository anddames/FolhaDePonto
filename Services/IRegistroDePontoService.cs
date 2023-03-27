using FolhaDePonto.Models;

namespace FolhaDePonto.Services
{
    public interface IRegistroDePontoService
    {
        Task<List<RegistroDePontoModel>> GetAllRegistrosDePontoAsync();
        Task<RegistroDePontoModel> GetRegistroDePontoByIdAsync(int UserId);
        Task<List<RegistroDePontoModel>> GetRegistrosDePontoByMonthAsync(int month);
        Task<bool> AdicionarRegistroDePontoAsync(RegistroDePontoModel registroDePonto);
        Task<bool> AtualizarRegistroDePontoAsync(RegistroDePontoModel registroDePonto);
        Task<bool> ExcluirRegistroDePontoAsync(int UserId);
        Task<double> CalcularHorasTrabalhadasDoDiaAsync(RegistroDePontoModel registroDePonto);
        Task<double> CalcularHorasTrabalhadasDoMesAsync(int month);
        Task<double> CalcularHorasExtrasDoDiaAsync(RegistroDePontoModel registroDePonto);
        Task<double> CalcularHorasExtrasDoMesAsync(int month);
    }
}

