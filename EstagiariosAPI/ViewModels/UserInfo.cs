using EstagiariosAPI.Entities;

namespace EstagiariosAPI.ViewModels
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public UserMovieList? FilmeFavorito { get; set; }
    }
}
