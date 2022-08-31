using ForumGames.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IJogadorRepository
    {
        public ICollection<Jogador> GetJogadores();
        public Jogador GetJogadorPorId(int id);
        public ICollection<Jogador> GetJogadoresComGrupos();
        public Jogador GetJogadorPorIdComGrupos(int id);
        public ICollection<Jogador> GetJogadoresComPostagens();
        public Jogador GetJogadorPorIdComPostagens(int id);
        public Jogador InsertJogador(Jogador jogador);
        public Jogador InsertJogadorComImagem(Jogador jogador, IFormFile arquivo);
        public bool UpdateJogador(int id, Jogador jogador);
        public bool UpdateJogadorComImagem(int id, Jogador jogador, IFormFile arquivo);
        public bool DeleteJogador(int id);
    }
}
