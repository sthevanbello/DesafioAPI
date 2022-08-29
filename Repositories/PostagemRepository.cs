using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ForumGames.Repositories
{
    public class PostagemRepository : IPostagemRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";
        public bool DeletePostagem(int id)
        {
            throw new System.NotImplementedException();
        }

        /*        
        OK -  public ICollection<Postagem> GetPostagens();
        public Postagem GetPostagemPorId(int id);
        public ICollection<Postagem> GetPostagemsComJogadores();
        public ICollection<Postagem> GetPostagemsComGrupos();
        public ICollection<Postagem> GetPostagemsComGruposEJogadores();

        public Postagem InsertPostagem(Postagem postagem); 

            - Ao inserir uma postagem, vincular o jogador ao grupo da postagem 
	            - Inserir o Jogador e o Grupo na tabela RL_Jogadores_Grupos, caso NÃO esteja inserido
		        para não ficar repetido

        public bool UpdatePostagem(int id, Postagem postagem);
        public bool DeletePostagem(int id);*/

        public ICollection<Postagem> GetPostagens()
        {
            var listaPostagens = new List<Postagem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
	                                P.Id AS 'Id_Postagem',
	                                P.Titulo AS 'Titulo_Postagem',
	                                P.Texto AS 'Texto_Postagem',
	                                P.Imagem AS 'Imagem_Postagem',
	                                P.DataHora AS 'DataHora_Postagem',
	                                P.GrupoId AS 'Id_Grupo_Postagem',
	                                P.CategoriaPostagemId AS 'Id_Categoria_Postagem',
	                                P.JogadorId AS 'Is_Jogador_Postagem'
                                FROM TB_Postagens AS P";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaPostagens.Add(new Postagem
                            {
                                Id = (int)result["Id_Postagem"],
                                Titulo = (string)result["Titulo_Postagem"],
                                Texto = (string)result["Texto_Postagem"],
                                Imagem = (string)result["Imagem_Postagem"],
                                DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                Grupo = null,
                                Jogador = null,
                                CategoriaPostagem = null
                            });
                        }
                    }

                }
            }
            return listaPostagens;
        }

        public Postagem GetPostagemPorId(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Postagem> GetPostagemsComJogadores()
        {
            throw new System.NotImplementedException();
        }

        public Postagem InsertPostagem(Postagem postagem)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdatePostagem(int id, Postagem postagem)
        {
            throw new System.NotImplementedException();
        }
    }
}
