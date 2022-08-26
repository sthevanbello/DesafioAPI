using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IGrupoRepository
    {
        public ICollection<Grupo> GetGrupos();
        public Grupo GetGrupoPorId(int id);
        public Grupo GetGrupoComJogadoresPorId(int id);
        public ICollection<Grupo> GetGruposComJogadores();
        public Grupo InsertGrupo(Grupo grupo);
        public bool UpdateGrupo(int id, Grupo grupo);
        public bool DeleteGrupo(int id);
    }
}
