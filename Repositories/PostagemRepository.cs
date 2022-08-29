using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Linq;

namespace ForumGames.Repositories
{
    public class PostagemRepository : IPostagemRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";
        

        /*        
        OK -  public ICollection<Postagem> GetPostagens();
        Ok - public Postagem GetPostagemPorId(int id);
        Ok - public ICollection<Postagem> GetPostagemsComJogadores();

        public Postagem InsertPostagem(Postagem postagem); 

            - Ao inserir uma postagem, vincular o jogador ao grupo da postagem 
	            - Inserir o Jogador e o Grupo na tabela RL_Jogadores_Grupos, caso NÃO esteja inserido
		        para não ficar repetido

        public bool UpdatePostagem(int id, Postagem postagem);
        public bool DeletePostagem(int id);*/

        /// <summary>
        /// Exibir todas as postagens
        /// </summary>
        /// <returns>Retorna uma <b>List</b> de <b>Postagem</b></returns>
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
        /// <summary>
        /// Exibir uma postagem a partir do seu Id
        /// </summary>
        /// <param name="id">Id da Postagem</param>
        /// <returns>Retorna uma <b>Postagem</b></returns>
        public Postagem GetPostagemPorId(int id)
        {
            Postagem postagem = null;
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
                                FROM TB_Postagens AS P
                                WHERE P.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            postagem = new Postagem
                            {
                                Id = (int)result["Id_Postagem"],
                                Titulo = (string)result["Titulo_Postagem"],
                                Texto = (string)result["Texto_Postagem"],
                                Imagem = (string)result["Imagem_Postagem"],
                                DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                Grupo = null,
                                Jogador = null,
                                CategoriaPostagem = null
                            };
                        }
                    }

                }
            }
            return postagem;
        }

        /// <summary>
        /// Exibe uma lista com as postagens de cada jogador
        /// </summary>
        /// <returns>Retorna uma <b>List</b> de Postagem</returns>
        public ICollection<Postagem> GetPostagensComJogador()
        {
            IList<Postagem> listaPostagem = new List<Postagem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"SELECT 
	                                J.Id AS 'Id_Jogador',
	                                J.Usuario AS 'Usuario_Do_Jogador',
	                                J.Senha AS 'Senha_Usuario',
	                                J.Nome AS 'Nome_Do_Jogador',
	                                J.Email AS 'Email_Do_Jogador',
	                                RL.GrupoId AS 'Id_Grupo',
                                    G.Id AS 'Id_Grupo_Postagem',
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Id_Categoria_Grupo',
                                    CG.Id AS 'Id_Categoria_Grupo',
	                                CG.Categoria AS 'Nome_Categoria_Grupo',
	                                P.Id AS 'Id_Postagem',
	                                P.Titulo AS 'Titulo_Postagem',
	                                P.Texto AS 'Texto_Postagem',
	                                P.Imagem AS 'Imagem_Postagem',
	                                P.DataHora AS 'DataHora_Postagem',
	                                P.CategoriaPostagemId AS 'Id_Categoria_Postagem',
	                                CP.Id AS 'Id_Categoria_Postagem',
	                                CP.Categoria AS 'Categoria_Postagem'
                                FROM TB_Jogadores AS J
                                INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
                                INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
                                INNER JOIN TB_Categorias_Grupos AS CG ON G.CategoriaId = CG.Id
                                INNER JOIN TB_Postagens AS P ON P.JogadorId = J.Id AND P.GrupoId = G.Id
                                INNER JOIN TB_Categorias_Postagens AS CP ON CP.Id = P.CategoriaPostagemId
                                ORDER BY P.Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            if (!string.IsNullOrEmpty(result["Id_Postagem"].ToString()))
                            {
                                var postagem = new Postagem
                                {
                                    Id = (int)result["Id_Postagem"],
                                    Titulo = (string)result["Titulo_Postagem"],
                                    Texto = (string)result["Texto_Postagem"],
                                    Imagem = (string)result["Imagem_Postagem"],
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    Grupo = new Grupo
                                    {
                                        Id = (int)result["Id_Grupo_Postagem"],
                                        Descricao = result["Descricao_Grupo"].ToString(),
                                        Categoria = new CategoriaGrupo
                                        {
                                            Id = (int)result["Id_Categoria_Grupo"],
                                            NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString(),
                                            Grupos = null
                                        },
                                        CategoriaId = null,
                                        Jogadores = null,
                                        Postagens = null
                                    },
                                    Jogador = new Jogador
                                    {
                                        Id = (int)result["Id_Jogador"],
                                        Nome = result["Nome_Do_Jogador"].ToString(),
                                        Email = result["Email_Do_Jogador"].ToString(),
                                        Usuario = result["Usuario_Do_Jogador"].ToString(),
                                        Senha = result["Senha_Usuario"].ToString(),
                                        Grupos = null,
                                        Postagens = null
                                    },
                                    CategoriaPostagem = new CategoriaPostagem
                                    {
                                        Id = (int)result["Id_Categoria_Postagem"],
                                        NomeCategoriaPostagem = result["Categoria_Postagem"].ToString(),
                                        Postagens = null
                                    }
                                };
                                listaPostagem.Add(postagem);
                            }
                        }
                    }
                }
            }
            return listaPostagem;
        }
        /* Pendente */
        public Postagem InsertPostagem(Postagem postagem)
        {
            throw new System.NotImplementedException();
        }
        /* Pendente */
        public bool UpdatePostagem(int id, Postagem postagem)
        {
            throw new System.NotImplementedException();
        }
        /* Pendente */
        public bool DeletePostagem(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
