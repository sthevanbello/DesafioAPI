using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        //public IList<Grupos> Grupos { get; set; }
    }
}
