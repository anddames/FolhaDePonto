using FolhaDePonto.Data;
using FolhaDePonto.Data.Repositories;
using FolhaDePonto.Models;
using FolhaDePonto.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FolhaPonto.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<UsuarioModel> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(ApplicationDbContext context, IUsuarioRepository usuarioRepository, 
            IPasswordHasher<UsuarioModel> passwordHasher, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor; 
        }

        public async Task<List<UsuarioModel>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<UsuarioModel> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<UsuarioModel> CriarUsuarioAsync(UsuarioModel usuario)
        {
            usuario.Senha = _passwordHasher.HashPassword(usuario, usuario.Senha);
            // Verifica se já existe um usuário com o mesmo email
            var usuarioExistente = await _usuarioRepository.GetUsuarioByEmailAsync(usuario.Email);
            if (usuarioExistente != null)
                throw new ArgumentException("Já existe um usuário cadastrado com esse email.");

            // Calcula a carga horária semanal do usuário
            usuario.CargaHorariaDiaria *= 5;

            // Cria o registro de ponto do usuário
            usuario.RegistroDePonto = new List<RegistroDePontoModel>();

            // Adiciona o usuário ao banco de dados
            await _usuarioRepository.AdicionarUsuarioAsync(usuario);

            return usuario;
        }

        public async Task AtualizarUsuarioAsync(UsuarioModel usuario)
        {
            await _usuarioRepository.AtualizarUsuarioAsync(usuario);
        }
        public async Task ExcluirUsuarioAsync(UsuarioModel usuario)
        {
            await _usuarioRepository.ExcluirUsuarioAsync(usuario.Id);
        }
        public string GerarTokenJwt(UsuarioModel usuario)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ChaveSecreta"]));
            var credenciais = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JWT:Emissor"],
                _configuration["JWT:Destinatario"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JWT:ValidadeEmMinutos"])),
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public UsuarioModel ObterUsuarioLogado()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;

            var usuario = new UsuarioModel  
            {
                Id = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value),
                Nome = claimsIdentity.FindFirst(ClaimTypes.Name).Value,
                Email = claimsIdentity.FindFirst(ClaimTypes.Email).Value
            };

            return usuario;
        }

        public Task<UsuarioModel> AutenticarUsuarioAsync(string email, string senha)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerificarEmailExistenteAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}


