using EstagiariosAPI.Database;
using EstagiariosAPI.Entities;
using EstagiariosAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstagiariosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public WatchlistController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize]
        [HttpPost("Adicionar/{userId}")]
        public async Task<IActionResult> AddMovieInWatchList(int userId, AddMovie body)
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

            user.FilmesWatchList.Add(movie);
            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Filme adicionado com sucesso!"));
        }

        [Authorize]
        [HttpDelete("Deletar/{userId}/{movieId}")]
        public async Task<IActionResult> RemoveMovieInWatchList(int userId, int movieId)
        {
            var user = await _dbContext.Usuarios.FindAsync(userId);

            Filme movieWatchlist = user.FilmesWatchList.FirstOrDefault(f => f.Id == movieId);
            Filme movie = await _dbContext.Filmes.FindAsync(movieId);

            user.FilmesWatchList.Remove(movieWatchlist);
            _dbContext.Filmes.Remove(movie);

            await _dbContext.SaveChangesAsync();

            return Ok(new Message("Filme removido com sucesso!"));
        }
    }
}
