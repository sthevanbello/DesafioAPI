using ForumGames.Models;
using System.Collections.Generic;

namespace ForumGames.Interfaces
{
    public interface IPostagemRepository
    {
        public ICollection<Postagem> GetPostagens();
        public Postagem GetPostagemPorId(int id);
        public ICollection<Postagem> GetPostagemsComJogadores();
        public Postagem InsertPostagem(Postagem postagem);
        public bool UpdatePostagem(int id, Postagem postagem);
        public bool DeletePostagem(int id);
    }
}
