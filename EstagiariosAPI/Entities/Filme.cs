using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstagiariosAPI.Entities
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public int IdAPI { get; set; }
        public string? BackdropPath { get; set; }
        public ICollection<Usuario> Usuario { get; } = new List<Usuario>();
        public List<Usuario> Usuarios { get; } = [];
        public List<Usuario> UsuarioWatchList { get; } = [];
    }
}
