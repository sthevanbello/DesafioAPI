using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IJogadorRepository
    {
        public ICollection<Jogador> GetJogadores();
        public Jogador GetJogadorPorId(int id);
        public Jogador GetJogadorPorIdComGrupos(int id);
        public ICollection<Jogador> GetJogadoresComGrupos();
        public Jogador InsertJogador(Jogador jogador);
        public Jogador InsertJogadorComImagem(Jogador jogador);
        public bool UpdateJogador(int id, Jogador jogador);
        public bool DeleteJogador(int id);
    }
}
