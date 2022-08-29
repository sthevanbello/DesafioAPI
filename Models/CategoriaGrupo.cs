using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ForumGames.Models
{
    // Categoria dos grupos
    public class CategoriaGrupo
    {
        public int Id { get; set; }
        public string NomeCategoriaGrupo { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] // Não exibe o campo no Json se o valor for null
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}
