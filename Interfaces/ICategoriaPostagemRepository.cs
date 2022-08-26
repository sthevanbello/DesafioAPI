using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface ICategoriaPostagemRepository
    {
        public ICollection<CategoriaPostagem> GetCategoriaPostagem();
        public CategoriaPostagem GetCategoriaPostagemPorId(int id);
        public CategoriaPostagem InsertCategoriaPostagem(CategoriaPostagem categoriaPostagem);
        public bool UpdateCategoriaPostagem(int id, CategoriaPostagem categoriaPostagem);
        public bool DeleteCategoriaPostagem(int id);
    }
}
