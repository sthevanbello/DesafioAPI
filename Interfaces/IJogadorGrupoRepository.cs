using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IJogadorGrupoRepository
    {
        public ICollection<JogadorGrupo> GetJogadorGrupo();
        public JogadorGrupo GetJogadorGrupoPorId(int id);
        public JogadorGrupo InsertJogadorGrupo(JogadorGrupo jogadorGrupo);
        public bool UpdateJogadorGrupo(int id, JogadorGrupo jogadorGrupo);
        public bool DeleteJogadorGrupo(int id);
    }
}
