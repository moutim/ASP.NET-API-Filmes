using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.Models;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstagiariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public UsuarioController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("Infos/{id}")]

        public async Task<IActionResult> GetUserInfoById(int id)
        {
            var user = _dbContext.Usuarios.Where(u => u.Id == id).Select(u => new UserInfo
            {
                Id = u.Id,
                Nome = u.Nome,
                Sobrenome = u.Sobrenome,
                Email = u.Email,
                FilmeFavorito = new UserMovieList
                {
                    Id = u.Filme.Id,
                    Nome = u.Filme.Nome,
                    IdAPI = u.Filme.IdAPI,
                    BackdropPath = u.Filme.BackdropPath
                }
            });

            if (user.Count() == 0)
            {
                return NotFound(new ErrorMessage($"Não existe usuário cadastrado com id {id}."));
            }

            return Ok(user);
        }

        [HttpPost("Criar")]
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
                return Conflict(new ErrorMessage("Já existe usário cadastrado com esse email!"));
            }

            _dbContext.Usuarios.Add(usuario);
            await _dbContext.SaveChangesAsync();

            return Ok("Usuário cadastrado com sucesso!");
        }

        [HttpGet("WatchList/{id}")]
        public async Task<IActionResult> GetUserWatchList(int id)
        {
            var user = _dbContext.Usuarios
                .Include(u => u.FilmesWatchList)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new ErrorMessage("Ainda não existem filmes cadastrados na sua WatchList."));
            }

            var userModel = user.FilmesWatchList.Select(item => new UserMovieList
            {
                Id = item.Id,
                IdAPI = item.IdAPI,
                Nome = item.Nome,
                BackdropPath = item.BackdropPath,
            });

            return Ok(userModel);
        }

        [HttpGet("Vistos/{id}")]
        public async Task<IActionResult> GetUserVistos(int id)
        {
            var user = _dbContext.Usuarios
                .Include(u => u.Filmes)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound(new ErrorMessage("Ainda não existem filmes cadastrados na sua lista de Vistos."));
            }

            var userModel = user.Filmes.Select(item => new UserMovieList
            {
                Id = item.Id,
                IdAPI = item.IdAPI,
                Nome = item.Nome,
                BackdropPath = item.BackdropPath,
            });

            return Ok(userModel);
        }
    }
}
