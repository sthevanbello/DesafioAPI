using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumGames.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Informe a descrição do grupo")]
        public string Descricao { get; set; }
        
        public int CategoriaId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CategoriaGrupo Categoria { get; set; } // Utilizado para retornar o nome da categoria no Get do Controller

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<Jogador> Jogadores { get; set; } = new List<Jogador>(); // Utilizado para retornar os jogadores do grupo

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ICollection<Postagem> Postagens { get; set; } = new List<Postagem>();
    }
}
