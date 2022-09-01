using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumGames.Models
{
    // Model criada para ser utilizada se houver a necessidade de fazer o relacionamento através de um controller
    // Isso não foi necessário, pois só há duas colunas na tabela de relacionamento RL_Jogadorees_Grupos.
    // Com isso o relacionamento de N:M é feito atrás de Listas
    public class JogadorGrupo
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o Id do grupo")]
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        [Required(ErrorMessage = "Informe o Id do jogador")]
        public int JogadorId { get; set; }
        public Jogador Jogador { get; set; }
    }
}
