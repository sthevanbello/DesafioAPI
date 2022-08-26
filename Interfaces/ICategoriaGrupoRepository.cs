using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface ICategoriaGrupoRepository
    {
        public ICollection<CategoriaGrupo> GetCategoriaGrupo();
        public CategoriaGrupo GetCategoriaGrupoPorId(int id);
        public CategoriaGrupo InsertCategoriaGrupo(CategoriaGrupo categoriaGrupo);
        public bool UpdateCategoriaGrupo(int id, CategoriaGrupo categoriaGrupo);
        public bool DeleteCategoriaGrupo(int id);
    }
}
