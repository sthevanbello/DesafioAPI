using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ForumGames.Models
{
    public class Jogador
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        [Required(ErrorMessage = "Informe a sua senha")]
        [MinLength(6, ErrorMessage = "A senha deverá ter no mínimo 6 caracteres")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "informe o seu nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "informe o seu e-mail")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Insira um e-mail válido")]
        public string Email { get; set; }
        public string Imagem { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] // Não exibe o campo no Json se o valor for null
        public IList<Grupo> Grupos { get; set; } = new List<Grupo>(); // Utilizado para retornar os grupos dos quais o jogador participa
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<Postagem> Postagens { get; set; } = new List<Postagem>(); // Utilizado para retornar as postagens do jogador 
    }
}
