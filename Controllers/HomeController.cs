using FolhaDePonto.Data;
using FolhaDePonto.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FolhaDePonto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HomeController(ApplicationDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var registrosDePonto = _context.RegistrosDePonto
            .Where(rp => rp.UserId == userId && rp.Data.Month == DateTime.Now.Month)
            .ToList();

            ViewData["RegistrosDoMes"] = registrosDePonto;

            return View(registrosDePonto);
        }

        public IActionResult Perfil()
        {
            return View();
        }

        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        public IActionResult RegistrarPonto()
        {
            return View();
        }
       
    }


}
