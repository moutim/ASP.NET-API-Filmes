using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstagiariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VistoController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public VistoController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("Adicionar/{userId}")]
        public async Task<IActionResult> AddMovieInVistos(int userId, AddMovie body)
        {
            Filme movie = new Filme
            {
                Nome = body.Nome,
                IdAPI = body.IdAPI,
                BackdropPath = body.BackdropPath,
            };
            _dbContext.Filmes.Add(movie);
            await _dbContext.SaveChangesAsync();

            Usuario user = await _dbContext.Usuarios.FindAsync(userId);

            user.Filmes.Add(movie);
            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Filme adicionado com sucesso!"));
        }

        [HttpDelete("Deletar/{userId}/{movieId}")]
        public async Task<IActionResult> RemoveMovieInWatchList(int userId, int movieId)
        {
            var user = await _dbContext.Usuarios.FindAsync(userId);

            Filme movieWatchlist = user.Filmes.FirstOrDefault(f => f.Id == movieId);
            Filme movie = await _dbContext.Filmes.FindAsync(movieId);

            user.Filmes.Remove(movieWatchlist);
            _dbContext.Filmes.Remove(movie);

            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Filme removido com sucesso!"));
        }
    }
}
