using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface ICategoriaPostagemRepository
    {
        public ICollection<CategoriaPostagem> GetAllCategoriaPostagem();
        public CategoriaPostagem GetCategoriaPostagemPorId(int id);
        public IList<CategoriaPostagem> GetAllCategoriaPostagemComPostagens();
        public CategoriaPostagem GetCategoriaPostagemPorIdComPostagens(int id);
        public CategoriaPostagem InsertCategoriaPostagem(CategoriaPostagem categoriaPostagem);
        public bool UpdateCategoriaPostagem(int id, CategoriaPostagem categoriaPostagem);
        public bool DeleteCategoriaPostagem(int id);
    }
}
