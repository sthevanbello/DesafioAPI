using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForumGames.Models
{
    public class Grupo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe a descrição do grupo")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Informe o Id da categoria")]
        public int CategoriaId { get; set; }
        public CategoriaGrupo Categoria { get; set; } // Utilizado para retornar o nome da categoria no Get do Controller
        public IList<Jogador> Jogadores { get; set; } = new List<Jogador>(); // Utilizado para retornar os jogadores do grupo
    }
}
