using FolhaDePonto.Models;
using Microsoft.EntityFrameworkCore;
using FolhaDePonto.Data;
using FolhaDePonto.Data.Repositories;

namespace FolhaDePonto.Services
{
    public class RegistroDePontoService : IRegistroDePontoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioService _usuarioService;
        private readonly int HorasDiarias;
        private readonly IRegistroDePontoRepository _registroDePontoRepository;

    public RegistroDePontoService(ApplicationDbContext context, IUsuarioService usuarioService, IRegistroDePontoRepository registroDePontoRepository)
        {
            _context = context;
            _usuarioService = usuarioService;
            _registroDePontoRepository = registroDePontoRepository;
        }

        public async Task<List<RegistroDePontoModel>> GetAllRegistrosDePontoAsync()
        {
            var registrosDePonto = await _context.RegistrosDePonto.ToListAsync();
            return registrosDePonto;
        }
        public async Task<RegistroDePontoModel> GetRegistroDePontoByIdAsync(int id)
        {
            return await _context.RegistrosDePonto.FindAsync(id);
        }
        public async Task<List<RegistroDePontoModel>> GetRegistrosDePontoByMonthAsync(int month)
        {
            // Filtra todos os registros de ponto do mês especificado
            var registrosDoMes = await _registroDePontoRepository.GetRegistrosDePontoByMonthAsync(month);

            // Calcula o total de horas trabalhadas no mês
            var totalHorasTrabalhadas = 0.0;
            foreach (var registro in registrosDoMes)
            {
                totalHorasTrabalhadas += await CalcularHorasTrabalhadasDoDiaAsync(registro);
            }

            // Calcula o total de horas extras no mês
            _ = await CalcularHorasExtrasDoMesAsync(month);

            // Atualiza a propriedade TotalHorasTrabalhadas de cada registro de ponto
            foreach (var registro in registrosDoMes)
            {
                registro.TotalHorasTrabalhadas = await CalcularHorasTrabalhadasDoDiaAsync(registro);
            }

            // Retorna os registros de ponto do mês com as informações atualizadas
            return (List<RegistroDePontoModel>)registrosDoMes;
        }

        public async Task<bool> AdicionarRegistroDePontoAsync(RegistroDePontoModel registroDePonto)
        {
            // Verifica se já existe um registro de ponto para a data informada
            var registroExistente = await _context.RegistrosDePonto.FirstOrDefaultAsync(r => r.Data.Date == registroDePonto.Data.Date && r.UserId == registroDePonto.UserId);
            if (registroExistente != null)
            {
                throw new ArgumentException($"Já existe um registro de ponto para a data {registroExistente.Data.ToShortDateString()}.");
            }

            // Verifica se o usuário existe
            var usuario = await _usuarioService.GetUsuarioByIdAsync(registroDePonto.UserId);
            if (usuario != null)
            {
                // Define o horário de entrada
                registroDePonto.HoraEntrada = DateTime.Now;

                // Adiciona o registro de ponto e salva as mudanças no banco de dados
                _context.RegistrosDePonto.Add(registroDePonto);
                await _context.SaveChangesAsync();

                return true;
            }
            throw new ArgumentException($"Usuário com id {registroDePonto.UserId} não encontrado.");
        }

        public async Task<bool> AtualizarRegistroDePontoAsync(RegistroDePontoModel registroDePonto)
        {
            var registroExistente = await _registroDePontoRepository.GetRegistroDePontoByIdAsync(registroDePonto.UserId);

            if (registroExistente == null)
            {
                return false;
            }

            registroExistente.HoraEntrada = registroDePonto.HoraEntrada;
            registroExistente.HoraSaida = registroDePonto.HoraSaida;
            registroExistente.Observacao = registroDePonto.Observacao;

            await _registroDePontoRepository.AtualizarRegistroDePontoAsync(registroExistente);

            // Atualiza o total de horas trabalhadas do dia
            registroExistente.TotalHorasTrabalhadas = await CalcularHorasTrabalhadasDoDiaAsync(registroExistente);

            // Atualiza o total de horas trabalhadas do mês
            var registrosDoMes = await _registroDePontoRepository.GetRegistrosDePontoByMonthAsync(registroExistente.Data.Month);
            double totalHorasTrabalhadas = 0;
            foreach (var registro in registrosDoMes)
            {
                totalHorasTrabalhadas += await CalcularHorasTrabalhadasDoDiaAsync(registro);
            }
            registroExistente.TotalHorasTrabalhadas = totalHorasTrabalhadas;

            // Atualiza o total de horas extras do dia
            registroExistente.HorasExtras = await CalcularHorasExtrasDoDiaAsync(registroExistente);

            // Atualiza o total de horas extras do mês
            double totalHorasExtras = await CalcularHorasExtrasDoMesAsync(registroExistente.Data.Month);
            registroExistente.HorasExtras = totalHorasExtras;

            await _registroDePontoRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirRegistroDePontoAsync(int id)
        {
            var registroDePonto = await _context.RegistrosDePonto.FindAsync(id);
            if (registroDePonto != null)
            {
                _context.RegistrosDePonto.Remove(registroDePonto);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new ArgumentException("Registro de ponto não encontrado");
        }
        public async Task<double> CalcularHorasTrabalhadasDoDiaAsync(RegistroDePontoModel registroDePonto)
        {

            // Calcula a diferença entre o horário de entrada e o horário de saída
            var diferencaHoras = registroDePonto.HoraSaida.Subtract(registroDePonto.HoraEntrada);

            double horasTrabalhadas;
            // Se a diferença de horas for menor ou igual a 8 horas, as horas trabalhadas são iguais à diferença de horas
            if (diferencaHoras <= TimeSpan.FromHours(8))
            {
                horasTrabalhadas = diferencaHoras.TotalHours;
            }
            // Se a diferença de horas for maior que 8 horas, as horas trabalhadas são iguais a 8 horas mais as horas extras
            else
            {
                horasTrabalhadas = 8;
                var horasExtras = diferencaHoras.Subtract(TimeSpan.FromHours(8));
                horasTrabalhadas += horasExtras.TotalHours;
            }

            return horasTrabalhadas;
        }
        public async Task<double> CalcularHorasTrabalhadasDoMesAsync(int month)
        {
            var registrosDoMes = await _registroDePontoRepository.GetRegistrosDePontoByMonthAsync(month);
            var totalHorasTrabalhadas = 0.0;

            foreach (var registro in registrosDoMes)
            {
                totalHorasTrabalhadas += await CalcularHorasTrabalhadasDoDiaAsync(registro);
            }

            var totalHorasMes = DateTime.DaysInMonth(DateTime.Now.Year, month) * 8;
            var horasExtras = totalHorasTrabalhadas - totalHorasMes;

            return horasExtras;
        }
        public async Task<double> CalcularHorasExtrasDoDiaAsync(RegistroDePontoModel registroDePonto)
        {
            var cargaHorariaDiaria = TimeSpan.FromHours(8);
            var entrada = registroDePonto.HoraEntrada;
            var saida = registroDePonto.HoraSaida;

            if (entrada == null || saida == null)
            {
                return 0;
            }

            var horasTrabalhadas = (DateTimeOffset)saida - (DateTimeOffset)entrada;

            if (horasTrabalhadas <= cargaHorariaDiaria)
            {
                return 0;
            }

            var horasExtras = horasTrabalhadas - cargaHorariaDiaria;

            return (double)horasExtras.TotalHours;
        }
        public async Task<double> CalcularHorasExtrasDoMesAsync(int month)
        {
            var registros = await _registroDePontoRepository.GetRegistrosDePontoByMonthAsync(month);

            double totalHorasExtras = 0;

            foreach (var registro in registros)
            {
                var horasTrabalhadas = await CalcularHorasTrabalhadasDoDiaAsync(registro);

                if (horasTrabalhadas > HorasDiarias)
                {
                    totalHorasExtras += horasTrabalhadas - HorasDiarias;
                }
            }

            return totalHorasExtras;
        }
    }

}


