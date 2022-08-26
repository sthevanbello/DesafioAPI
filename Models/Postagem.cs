﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ForumGames.Models
{
    public class Postagem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o título da postagem")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Informe o texto da postagem")]
        public string Texto { get; set; }
        public string Imagem { get; set; }
        public DateTime DataHora { get; set; }
        [Required(ErrorMessage = "Informe o Id do grupo da postagem")]
        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; }
        [Required(ErrorMessage = "Informe o Id da categoria da postagem")]
        public int CategoriaPostagemId { get; set; }
        public CategoriaPostagem CategoriaPostagem { get; set; }
        [Required(ErrorMessage = "Informe o Id do jogador da postagem")]
        public int JogadorId { get; set; }
        public Jogador Jogador { get; set; }
    }
}