using FolhaDePonto.Data;
using FolhaDePonto.Data.Repositories;
using FolhaDePonto.Models;
using FolhaDePonto.Models.ViewModels;
using FolhaDePonto.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FolhaDePonto.Controllers
{
    [Authorize]
    public class PontoController : Controller
    {
        private readonly IRegistroDePontoService _pontoService;
        private readonly IUsuarioService _usuarioService;
        private readonly ApplicationDbContext _context;
        private readonly IRegistroDePontoRepository _registroDePontoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public PontoController(IRegistroDePontoService pontoService, IUsuarioService usuarioService, ApplicationDbContext context, 
            IRegistroDePontoRepository registroDePontoRepository, IUsuarioRepository usuarioRepository )
        {
            _pontoService = pontoService;
            _usuarioService = usuarioService;
            _context = context;
            _registroDePontoRepository = registroDePontoRepository;
            _usuarioRepository = usuarioRepository; 
        }

        // GET: Ponto
        public async Task<IActionResult> Index()
        {
            var registrosDePontoList = await _pontoService.GetAllRegistrosDePontoAsync();
            var horasExtras = await _pontoService.CalcularHorasExtrasDoDiaAsync(registrosDePontoList);
            var horasRealizadas = await _pontoService.CalcularHorasExtrasDoMesAsync(registrosDePontoList);
            var viewModel = new PontoViewModel()
            {
                Id = registrosDePontoList,
                HorasExtras = horasExtras,
                TotalHorasTrabalhadas = horasRealizadas
            };
            return View(viewModel);
        }

        // GET: Ponto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ponto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegistroDePontoModel registroDePontoModel)
        {
            if (ModelState.IsValid)
            {
                // Obtém o usuário logado
                var usuario = _usuarioService.ObterUsuarioLogado();

                // Define o usuário do registro de ponto
                registroDePontoModel.Usuario = usuario;

                // Cadastra o registro de ponto
                _pontoService.AdicionarRegistroDePontoAsync(registroDePontoModel);

                return RedirectToAction("Index");
            }

            return View(registroDePontoModel);
        }

        // GET: Ponto/Edit/5
        public ActionResult Edit(int id)
        {
            // Obtém o registro de ponto pelo ID
            var registroDePonto = _pontoService.GetRegistroDePontoByIdAsync(id);

            if (registroDePonto == null)
            {
                return NotFound();
            }

            return View(registroDePonto);
        }

        // POST: Ponto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegistroDePontoModel registroDePontoModel)
        {
            if (ModelState.IsValid)
            {
                // Atualiza o registro de ponto
                _pontoService.AtualizarRegistroDePontoAsync(registroDePontoModel);

                return RedirectToAction("Index");
            }

            return View(registroDePontoModel);
        }

        // GET: Ponto/Delete/5
        public ActionResult Delete(int id)
        {
            // Obtém o registro de ponto pelo ID
            var registroDePonto = _pontoService.GetRegistroDePontoByIdAsync(id);

            if (registroDePonto == null)
            {
                return NotFound();
            }

            return View(registroDePonto);
        }

        // POST: Ponto/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroDePonto = await _context.RegistrosDePonto.FindAsync(id);
            if (registroDePonto == null)
            {
                return NotFound();
            }

            _context.RegistrosDePonto.Remove(registroDePonto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RegistroDePontoModel registroDePonto)
        {
            if (ModelState.IsValid)
            {
                // Verifica se o usuário está registrado no sistema
                var usuario = _usuarioRepository.GetUsuarioByIdAsync(registroDePonto.UserId);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Verifica se o registro de ponto existe
                var registroExistente = await _registroDePontoRepository.GetRegistroDePontoByIdAsync(registroDePonto.UserId);
                if (registroExistente == null)
                {
                    return NotFound();
                }

                // Atualiza as informações do registro de ponto
                registroExistente.Data = registroDePonto.Data;
                registroExistente.HoraEntrada = registroDePonto.HoraEntrada;
                registroExistente.HoraAlmoco = registroDePonto.HoraAlmoco;
                registroExistente.HoraRetornoAlmoco = registroDePonto.HoraRetornoAlmoco;
                registroExistente.HoraSaida = registroDePonto.HoraSaida;
                registroExistente.Observacao = registroDePonto.Observacao;
                registroExistente.TotalHorasTrabalhadas = registroDePonto.TotalHorasTrabalhadas;
                registroExistente.HorasExtras = registroDePonto.HorasExtras;

                // Salva as alterações no banco de dados
                await _registroDePontoRepository.AtualizarRegistroDePontoAsync(registroExistente);

                return RedirectToAction("Index");
            }

            return View(registroDePonto);
        }











    }


}


