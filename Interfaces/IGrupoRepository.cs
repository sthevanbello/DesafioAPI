using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IGrupoRepository
    {
        public Grupo InsertGrupo(Grupo grupo);
        public ICollection<Grupo> GetAllGrupos();
        public Grupo GetGrupoPorId(int id);
        public Grupo GetGrupoPorIdComJogadores(int id);
        public ICollection<Grupo> GetAllGruposComJogadores();
        public ICollection<Grupo> GetAllGruposComPostagens();
        public Grupo GetGrupoPorIdComJogadoresComPostagens(int id);
        public ICollection<Grupo> GetAllGruposComJogadoresComPostagens();
        public bool UpdateGrupo(int id, Grupo grupo);
        public bool DeleteGrupo(int id);
    }
}
