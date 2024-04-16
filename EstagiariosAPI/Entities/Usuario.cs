using Azure;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EstagiariosAPI.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int? FilmeId { get; set; }

        [JsonProperty("filmeFavorito")]
        public Filme? Filme { get; set; }

        [JsonProperty("Vistos")]
        public List<Filme> Filmes { get; } = [];

        [JsonProperty("WatchList")]
        public List<Filme> FilmesWatchList { get; } = [];
    }
}
