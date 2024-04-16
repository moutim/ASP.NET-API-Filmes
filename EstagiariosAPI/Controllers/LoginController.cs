using EstagiariosAPI.Database;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstagiariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public LoginController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login body)
        {
            var user = await _dbContext.Usuarios.FirstOrDefaultAsync(u => u.Email == body.Email);

            if (user == null)
            {
                return NotFound(new Message("Email ou senha incorreto."));
            }

            if (user.Senha != body.Senha)
            {
                return Unauthorized(new Message("Email ou senha incorreto."));
            }

            return Ok(new LoginSuccess
            {
                Id = user.Id,
                Nome = user.Nome
            });
        }
    }
}
