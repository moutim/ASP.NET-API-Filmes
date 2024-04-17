using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.Models;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstagiariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public RegistroController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUsuario bodyUsuario)
        {
            Usuario usuario = new()
            {
                Nome = bodyUsuario.Nome,
                Sobrenome = bodyUsuario.Sobrenome,
                Email = bodyUsuario.Email,
                Senha = bodyUsuario.Senha
            };

            var checkUsuario = _dbContext.Usuarios.Where(u => u.Email == usuario.Email);

            if (checkUsuario.Any())
            {
                return Conflict(new Message("Já existe usário cadastrado com esse email!"));
            }

            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Usuário cadastrado com sucesso!"));
        }
    }
}
