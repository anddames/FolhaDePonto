using FolhaDePonto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FolhaDePonto.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        // Construtor que injeta o UserManager na classe
        public UsuarioController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // Action que retorna a view de cadastro de usuário
        public IActionResult Cadastrar()
        {
            return View();
        }

        // Action que processa o cadastro de usuário enviado via POST
        [HttpPost]
        public async Task<IActionResult> Cadastrar(UsuarioModel model)
        {
            // Verifica se o modelo enviado é válido
            if (ModelState.IsValid)
            {
                // Cria um novo usuário
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                // Cria o usuário no banco de dados usando o UserManager
                var result = await _userManager.CreateAsync(user, model.Senha);

                // Verifica se a criação do usuário foi bem sucedida
                if (result.Succeeded)
                {
                    // Redireciona o usuário para a página de login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Adiciona erros de validação ao ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Se o modelo não for válido, retorna a view com os erros de validação
            return View(model);
        }

        // Action que retorna a view de perfil do usuário logado
        public async Task<IActionResult> Perfil()
        {
            // Obtém o usuário atual através do UserManager
            var user = await _userManager.GetUserAsync(User);

            // Verifica se o usuário existe
            if (user == null)
            {
                return NotFound();
            }

            // Cria um modelo com as informações do usuário
            var model = new UsuarioModel
            {
                Nome = user.UserName,
                Email = user.Email,

            };

            // Retorna a view com o modelo
            return View(model);
        }

        // Action que processa a exclusão de um usuário
        [HttpPost]
        public async Task<IActionResult> Excluir()
        {
            // Obtém o usuário atual através do UserManager
            var user = await _userManager.GetUserAsync(User);

            // Verifica se o usuário existe
            if (user == null)
            {
                return NotFound();
            }

            // Remove o usuário do banco de dados usando o UserManager
            var result = await _userManager.DeleteAsync(user);

            // Verifica se a remoção foi bem sucedida
            if (result.Succeeded)
            {
                // Desloga o usuário e redireciona para a página de login
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Adiciona erros de validação ao ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se a remoção não for bem sucedida, retorna a view com os erros de validação
            return View("Perfil", new UsuarioModel
            {
                Nome = user.UserName,
                Email = user.Email,
            });
        }
    }
}




