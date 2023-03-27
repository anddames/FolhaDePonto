using FolhaDePonto.Models;
using FolhaDePonto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FolhaDePonto.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<UsuarioModel> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<UsuarioModel> signInManager, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Usuário logado com sucesso.");
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Usuário bloqueado.");
                    return View("Lockout");
                }

                ModelState.AddModelError(string.Empty, "Login inválido.");
            }

            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário desconectado.");
            return RedirectToAction("Index", "Home");
        }
    }

}
