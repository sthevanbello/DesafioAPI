using System.ComponentModel.DataAnnotations;

namespace ForumGames.Models
{
    public class JogadorGrupo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o Id do grupo")]
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        [Required(ErrorMessage = "Informe o Id do jogador")]
        public int JogadorId { get; set; }
        public Jogador Jogador { get; set; }
    }
}
