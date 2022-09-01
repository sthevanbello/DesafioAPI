using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ForumGames.Models
{
    // Categoria das postagens
    public class CategoriaPostagem
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        public string NomeCategoriaPostagem { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] // Não exibe o campo no Json se o valor for null
        public ICollection<Postagem> Postagens { get; set; } = new List<Postagem>();
    }
}
