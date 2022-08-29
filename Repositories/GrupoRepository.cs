using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;
using ForumGames.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ForumGames.Repositories
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";

        /// <summary>
        /// Inserir um grupo no banco de dados
        /// </summary>
        /// <param name="grupo">Grupo informado</param>
        /// <returns>Retorna o grupo que foi inserido</returns>
        /// <exception cref="ThereIsntCategoryException">Captura uma exception informando que a categoria informada não existe no banco de dados</exception>
        public Grupo InsertGrupo(Grupo grupo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string scriptInsert = @"INSERT INTO TB_Grupos 
                                            (Descricao, CategoriaId) 
                                        VALUES 
                                            (@Descricao, @CategoriaId)";

                // Verifica se a categoria informada existe no banco de dados
                string scriptCategoria = @"SELECT 
                                            CG.Id AS 'Id_Categoria',
                                            CG.Categoria AS 'Categoria_Nome'
                                        FROM TB_Categorias_Grupos AS CG
                                        WHERE CG.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(scriptCategoria, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = grupo.CategoriaId;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            using (SqlCommand cmdInsert = new SqlCommand(scriptInsert, connection))
                            {
                                // Declarar as variáveis por parâmetros
                                cmdInsert.Parameters.Add("Descricao", SqlDbType.NVarChar).Value = grupo.Descricao;
                                cmdInsert.Parameters.Add("CategoriaId", SqlDbType.NVarChar).Value = grupo.CategoriaId;
                                cmdInsert.CommandType = CommandType.Text;
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                        if (!result.HasRows)
                        {
                            throw new ThereIsntCategoryException("Não há categoria com o id informado");
                        }
                    }
                }

            }
            return grupo;
        }
        /// <summary>
        /// Exibir todos os Grupos contidos no banco de dados
        /// </summary>
        /// <returns>Retorna uma <b>List</b> de Grupos</returns>
        public ICollection<Grupo> GetAllGrupos()
        {
            var listaGrupos = new List<Grupo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
	                                G.Id AS 'Id_Grupo',
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Categoria_Grupo'
                                FROM TB_Grupos AS G";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaGrupos.Add(new Grupo
                            {
                                Id = (int)result["Id_Grupo"],
                                Descricao = result["Descricao_Grupo"].ToString(),
                                Categoria = null,
                                Jogadores = null
                            });
                        }
                    }

                }
            }
            return listaGrupos;
        }
        /// <summary>
        /// Exibir um grupo a partir do Id
        /// </summary>
        /// <param name="id">Id do grupo</param>
        /// <returns>Retorna um <b>Grupo</b> a partir do id</returns>
        public Grupo GetGrupoPorId(int id)
        {
            Grupo grupo = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
	                                G.Id AS 'Id_Grupo',
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Categoria_Grupo'
                                FROM TB_Grupos AS G
                                WHERE G.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (result != null && result.HasRows && result.Read())
                        {
                            grupo = new Grupo
                            {
                                Id = (int)result["Id_Grupo"],
                                Descricao = result["Descricao_Grupo"].ToString(),
                                Categoria = null,
                                Jogadores = null
                            };
                        }
                    }

                }
            }
            return grupo;
        }
        /// <summary>
        /// Exibir uma lista de grupos com os jogadores que integram o grupo
        /// </summary>
        /// <returns>Retorna uma lista de grupos com os jogadores integram o grupo</returns>
        public ICollection<Grupo> GetAllGruposComJogadores()
        {
            IList<Grupo> listaGrupos = new List<Grupo>();
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
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Id_Categoria_Grupo',
	                                CG.Categoria AS 'Nome_Categoria_Grupo'
                                FROM TB_Jogadores AS J
                                INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
                                INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
                                INNER JOIN TB_Categorias_Grupos AS CG ON G.CategoriaId = CG.Id
                                ORDER BY G.Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var jogador = new Jogador();
                            if (!string.IsNullOrEmpty(result["Id_Jogador"].ToString()))
                            {
                                jogador = new Jogador
                                {

                                    Id = (int)result["Id_Jogador"],
                                    Nome = result["Nome_Do_Jogador"].ToString(),
                                    Email = result["Email_Do_Jogador"].ToString(),
                                    Usuario = result["Usuario_Do_Jogador"].ToString(),
                                    Senha = result["Senha_Usuario"].ToString(),
                                    Postagens = null,
                                    Grupos = null
                                };
                            }

                            if (!listaGrupos.Any(x => x.Id == (int)result["Id_Grupo"]))
                            {
                                var grupo = new Grupo
                                {
                                    Id = (int)result["Id_Grupo"],
                                    Descricao = result["Descricao_Grupo"].ToString(),

                                    Categoria = new CategoriaGrupo
                                    {
                                        Id = (int)result["Id_Categoria_Grupo"],
                                        NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString(),
                                        Grupos = null
                                    },
                                };

                                if ((grupo?.Id ?? 0) > 0)
                                {
                                    grupo.Jogadores.Add(jogador);
                                }

                                listaGrupos.Add(grupo);
                            }
                            else if ((jogador?.Id ?? 0) > 0) // grupo?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca o jogador e adiciona o grupo na lista de grupos dos quais o jogador participa
                                listaGrupos.FirstOrDefault(x => x.Id == (int)result["Id_Grupo"]).Jogadores.Add(jogador);
                            }
                        }
                    }

                }
            }
            return listaGrupos;
        }
        /// <summary>
        /// Exibir um grupo e os jogadores que o integram
        /// </summary>
        /// <param name="id">Id do grupo a ser buscado</param>
        /// <returns>Retorna um grupo e os jogadores que o integram</returns>
        public Grupo GetGrupoPorIdComJogadores(int id)
        {
            Grupo grupo = GetGrupoPorId(id);
            if (grupo is null)
            {
                return null;
            }
            grupo.Jogadores = new List<Jogador>();
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
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Id_Categoria_Grupo',
	                                CG.Categoria AS 'Nome_Categoria_Grupo'
                                FROM TB_Jogadores AS J
                                INNER JOIN	RL_Jogadores_Grupos AS RL ON J.Id = RL.JogadorId
                                INNER JOIN TB_Grupos AS G ON G.Id = RL.GrupoId
                                INNER JOIN TB_Categorias_Grupos AS CG ON G.CategoriaId = CG.Id
                                WHERE G.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = grupo.Id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var jogador = new Jogador();
                            if (!string.IsNullOrEmpty(result["Id_Jogador"].ToString()))
                            {

                                jogador = new Jogador
                                {

                                    Id = (int)result["Id_Jogador"],
                                    Nome = result["Nome_Do_Jogador"].ToString(),
                                    Email = result["Email_Do_Jogador"].ToString(),
                                    Usuario = result["Usuario_Do_Jogador"].ToString(),
                                    Senha = result["Senha_Usuario"].ToString(),
                                    Postagens = null,
                                    Grupos = null
                                };
                                grupo.Jogadores.Add(jogador);
                            }
                        }
                    }

                }
            }
            return grupo;
        }
        /// <summary>
        /// Exibir uma lista com todos os grupos, postagens e os jogadores que integram o grupo.
        /// </summary>
        /// <returns>Retorna uma <b>List</b> com todos os grupos, postagens e os jogadores que integram o grupo</returns>
        public ICollection<Grupo> GetAllGruposComPostagensEJogadores()
        {
            IList<Grupo> listaGrupos = new List<Grupo>();
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
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Id_Categoria_Grupo',
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
                                INNER JOIN TB_Categorias_Postagens AS CP ON CP.Id = P.CategoriaPostagemId";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var postagem = new Postagem();
                            if (!string.IsNullOrEmpty(result["Id_Postagem"].ToString()))
                            {
                                postagem = new Postagem
                                {
                                    Id = (int)result["Id_Postagem"],
                                    Titulo = result["Titulo_Postagem"].ToString(),
                                    Texto = result["Texto_Postagem"].ToString(),
                                    Imagem = result["Imagem_Postagem"].ToString(),
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    CategoriaPostagem = new CategoriaPostagem
                                    {
                                        Id = (int)result["Id_Categoria_Postagem"],
                                        NomeCategoriaPostagem = result["Categoria_Postagem"].ToString(),
                                        Postagens = null
                                    },
                                    Grupo = null,
                                    Jogador = new Jogador
                                    {
                                        Id = (int)result["Id_Jogador"],
                                        Nome = result["Nome_Do_Jogador"].ToString(),
                                        Email = result["Email_Do_Jogador"].ToString(),
                                        Usuario = result["Usuario_Do_Jogador"].ToString(),
                                        Senha = result["Senha_Usuario"].ToString(),
                                        Grupos = null,
                                        Postagens = null
                                    }
                                };
                            }

                            if (!listaGrupos.Any(x => x.Id == (int)result["Id_Grupo"]))
                            {

                                var grupo = new Grupo
                                {
                                    Id = (int)result["Id_Grupo"],
                                    Descricao = result["Descricao_Grupo"].ToString(),

                                    Categoria = new CategoriaGrupo
                                    {
                                        Id = (int)result["Id_Categoria_Grupo"],
                                        NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString(),
                                        Grupos = null
                                    },
                                    Jogadores = null
                                };

                                if ((grupo?.Id ?? 0) > 0)
                                {
                                    grupo.Postagens.Add(postagem);
                                }

                                listaGrupos.Add(grupo);
                            }
                            else if ((postagem?.Id ?? 0) > 0) // postagem?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca o grupo e adiciona a postagem na lista de postagens do grupo.
                                listaGrupos.FirstOrDefault(x => x.Id == (int)result["Id_Grupo"]).Postagens.Add(postagem);
                            }
                        }
                    }

                }
            }
            return listaGrupos;
        }
        /// <summary>
        /// Exibe um Grupo com as Postagens e os jogadores que fizeram as postagens
        /// </summary>
        /// <param name="id">Id do grupo</param>
        /// <returns>Retorna um <b>Grupo</b> com as Postagens e os jogadores que fizeram as postagens</returns>
        public Grupo GetGrupoPorIdComPostagensEJogadores(int id)
        {
            Grupo grupo = GetGrupoPorId(id);
            if (grupo is null)
            {
                return null;
            }
            grupo.Postagens = new List<Postagem>();
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
	                                G.Descricao AS 'Descricao_Grupo',
	                                G.CategoriaId AS 'Id_Categoria_Grupo',
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
                                WHERE G.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = grupo.Id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var postagem = new Postagem();
                            if (!string.IsNullOrEmpty(result["Id_Postagem"].ToString()))
                            {
                                postagem = new Postagem
                                {
                                    Id = (int)result["Id_Postagem"],
                                    Titulo = result["Titulo_Postagem"].ToString(),
                                    Texto = result["Texto_Postagem"].ToString(),
                                    Imagem = result["Imagem_Postagem"].ToString(),
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    CategoriaPostagem = new CategoriaPostagem
                                    {
                                        Id = (int)result["Id_Categoria_Postagem"],
                                        NomeCategoriaPostagem = result["Categoria_Postagem"].ToString(),
                                        Postagens = null
                                    },
                                    Grupo = null,
                                    Jogador = new Jogador
                                    {
                                        Id = (int)result["Id_Jogador"],
                                        Nome = result["Nome_Do_Jogador"].ToString(),
                                        Email = result["Email_Do_Jogador"].ToString(),
                                        Usuario = result["Usuario_Do_Jogador"].ToString(),
                                        Senha = result["Senha_Usuario"].ToString(),
                                        Grupos = null,
                                        Postagens = null
                                    }
                                };
                            }
                            grupo.Postagens.Add(postagem);
                        }
                    }
                }
            }
            return grupo;
        }
        /// <summary>
        /// Atualiza um Grupo existente no banco de dados
        /// </summary>
        /// <param name="id">Id do Grupo a ser atualizado</param>
        /// <param name="grupo">Dados atualizados do Grupo</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        public bool UpdateGrupo(int id, Grupo grupo)
        {
            if (GetGrupoPorId(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Execução no banco
                string scriptCategoria = @"SELECT 
                                            CG.Id AS 'Id_Categoria',
                                            CG.Categoria AS 'Categoria_Nome'
                                        FROM TB_Categorias_Grupos AS CG
                                        WHERE CG.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(scriptCategoria, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = grupo.CategoriaId;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (!result.HasRows)
                        {
                            throw new ThereIsntCategoryException("Não há categoria com o id informado");
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string scriptUpdate = @"UPDATE TB_Grupos 
					                    SET 
						                    Descricao = @Descricao,
						                    CategoriaId = @CategoriaId
				                        WHERE Id = @Id";
                using (SqlCommand cmdUpdate = new SqlCommand(scriptUpdate, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmdUpdate.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmdUpdate.Parameters.Add("Descricao", SqlDbType.NVarChar).Value = grupo.Descricao;
                    cmdUpdate.Parameters.Add("CategoriaId", SqlDbType.Int).Value = grupo.CategoriaId;
                    cmdUpdate.CommandType = CommandType.Text;
                    cmdUpdate.ExecuteNonQuery();
                }
            }
            return true;
        }
        /**/
        public bool DeleteGrupo(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
