using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.Models;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("Infos/{userId}")]
        public async Task<IActionResult> GetUserInfoById(int userId)
        {
            var user = _dbContext.Usuarios.Where(u => u.Id == userId).Select(u => new UserInfo
            {
                Id = u.Id,
                Nome = u.Nome,
                Sobrenome = u.Sobrenome,
                Email = u.Email,
                FilmeFavorito = u.Filme != null ? new UserMovieList
                {
                    Id = u.Filme.Id,
                    Nome = u.Filme.Nome,
                    IdAPI = u.Filme.IdAPI,
                    BackdropPath = u.Filme.BackdropPath
                } : null
            });

            if (user.Count() == 0)
            {
                return NotFound(new Message($"Não existe usuário cadastrado com id {userId}."));
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("WatchList/{userId}")]
        public async Task<IActionResult> GetUserWatchList(int userId)
        {
            var user = _dbContext.Usuarios
                .Include(u => u.FilmesWatchList)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new Message("Ainda não existem filmes cadastrados na sua WatchList."));
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

        [Authorize]
        [HttpGet("Vistos/{userId}")]
        public async Task<IActionResult> GetUserVistos(int userId)
        {
            var user = _dbContext.Usuarios
                .Include(u => u.Filmes)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound(new Message("Ainda não existem filmes cadastrados na sua lista de Vistos."));
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

        [Authorize]
        [HttpDelete("Deletar/{userId}")]
        public async Task<IActionResult> DeleteUSer(int userId)
        {
            var user = await _dbContext.Usuarios.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new Message($"Não existe usuário cadastrado com id {userId}."));
            }

            _dbContext.Usuarios.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Usuário deletado com sucesso."));
        }

        [Authorize]
        [HttpPatch("Atualizar/{userId}")]
        public async Task<IActionResult> UpdateUSer(int userId, UpdateUSer bodyUsuario)
        {
            var user = await _dbContext.Usuarios.FindAsync(userId);

            if (user == null)
            {
                return NotFound(new Message($"Não existe usuário cadastrado com id {userId}."));
            }

            user.Nome = !string.IsNullOrEmpty(bodyUsuario.Nome) ? bodyUsuario.Nome : user.Nome;
            user.Sobrenome = !string.IsNullOrEmpty(bodyUsuario.Sobrenome) ? bodyUsuario.Sobrenome : user.Sobrenome;
            user.Email = !string.IsNullOrEmpty(bodyUsuario.Email) ? bodyUsuario.Email : user.Email;
            user.Senha = !string.IsNullOrEmpty(bodyUsuario.Senha) ? bodyUsuario.Senha : user.Senha;

            await _dbContext.SaveChangesAsync();
            return Ok(new Message("Usuário atualizado com sucesso!"));
        }
    }
}
