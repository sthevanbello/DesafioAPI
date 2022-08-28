using ForumGames.Interfaces;
using ForumGames.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace ForumGames.Repositories
{
    public class JogadorRepository : IJogadorRepository
    {
        //private IDbConnection connectionString;
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";
        /// <summary>
        /// Insere um jogador no banco
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Jogador InsertJogador(Jogador jogador)
        {
            throw new System.NotImplementedException();
        }
        public Jogador InsertJogadorComImagem(Jogador jogador)
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Lista todos os jogadores
        /// </summary>
        /// <returns>Retorna uma lista de jogadores</returns>
        public ICollection<Jogador> GetJogadores()
        {
            var listaJogadores = new List<Jogador>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
	                                J.Id AS 'Id_jogador', 
	                                J.Nome AS 'Nome_Jogador', 
	                                J.Usuario AS 'Nome_de_Usuario', 
	                                J.Senha AS 'Senha_de_Usuario', 
                                    J.Email AS 'Email_do_Usuario'
                                FROM TB_Jogadores AS J";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaJogadores.Add(new Jogador
                            {
                                Id = (int)result["Id_jogador"],
                                Nome = (string)result["Nome_Jogador"],
                                Usuario = (string)result["Nome_de_Usuario"],
                                Senha = (string)result["Senha_de_Usuario"],
                                Email = (string)result["Email_do_Usuario"],
                                Grupos = null,
                                Postagens = null
                            });
                        }
                    }

                }
            }
            return listaJogadores;
        }
        public Jogador GetJogadorPorId(int id)
        {
            Jogador jogador = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
	                                J.Id AS 'Id_jogador', 
	                                J.Nome AS 'Nome_Jogador', 
	                                J.Usuario AS 'Nome_de_Usuario', 
	                                J.Senha AS 'Senha_de_Usuario', 
                                    J.Email AS 'Email_do_Usuario'
                                FROM TB_Jogadores AS J WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (result != null && result.HasRows && result.Read())
                        {
                            jogador = new Jogador
                            {
                                Id = (int)result["Id_jogador"],
                                Nome = (string)result["Nome_Jogador"],
                                Usuario = (string)result["Nome_de_Usuario"],
                                Senha = (string)result["Senha_de_Usuario"],
                                Email = (string)result["Email_do_Usuario"],
                                Grupos = null,
                                Postagens = null
                            };
                        }
                    }

                }
            }
            return jogador;
        }
        /// <summary>
        /// Jogador por id com os grupos dos quais cada jogador participa
        /// </summary>
        /// <returns>Retorna o Jogador com os grupos dos quais participa</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ICollection<Jogador> GetJogadoresComGrupos()
        {
            IList<Jogador> listaJogadores = new List<Jogador>();
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
                                ORDER BY J.Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var grupo = new Grupo();
                            if (!string.IsNullOrEmpty(result["Id_Grupo"].ToString()))
                            {
                                grupo = new Grupo
                                {
                                    Id = (int)result["Id_Grupo"],
                                    Descricao = result["Descricao_Grupo"].ToString(),

                                    Categoria = new CategoriaGrupo
                                    {
                                        Id = (int)result["Id_Categoria_Grupo"],
                                        NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString()
                                    },
                                    Jogadores = null
                                };
                            }

                            if (!listaJogadores.Any(x => x.Id == (int)result["Id_Jogador"]))
                            {
                                var jogador = new Jogador
                                {
                                    Id = (int)result["Id_Jogador"],
                                    Nome = result["Nome_Do_Jogador"].ToString(),
                                    Email = result["Email_Do_Jogador"].ToString(),
                                    Usuario = result["Usuario_Do_Jogador"].ToString(),
                                    Senha = result["Senha_Usuario"].ToString(),
                                    Postagens = null
                                };

                                if ((jogador?.Id ?? 0) > 0)
                                {
                                    jogador.Grupos.Add(grupo);
                                }

                                listaJogadores.Add(jogador);
                            }
                            else if ((grupo?.Id ?? 0) > 0) // grupo?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca o jogador e adiciona o grupo na lista de grupos dos quais o jogador participa
                                listaJogadores.FirstOrDefault(x => x.Id == (int)result["Id_Jogador"]).Grupos.Add(grupo);
                            }
                        }
                    }

                }
            }
            return listaJogadores;
        }
        /// <summary>
        /// Jogador por Id com os grupos dos quais ele participa
        /// </summary>
        /// <param name="id">Id do jogador</param>
        /// <returns>Retorna um <b>Jogador</b></returns>
        public Jogador GetJogadorPorIdComGrupos(int id)
        {
            Jogador jogador = GetJogadorPorId(id);
            if (jogador is null)
            {
                return null;
            }
            jogador.Grupos = new List<Grupo>();
            jogador.Postagens = null;
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
                                WHERE J.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = jogador.Id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var grupo = new Grupo();
                            if (!string.IsNullOrEmpty(result["Id_Grupo"].ToString()))
                            {

                                grupo = new Grupo
                                {
                                    Id = (int)result["Id_Grupo"],
                                    Descricao = result["Descricao_Grupo"].ToString(),

                                    Categoria = new CategoriaGrupo
                                    {
                                        Id = (int)result["Id_Categoria_Grupo"],
                                        NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString()
                                    },
                                    Jogadores = null,
                                };
                                jogador.Grupos.Add(grupo);
                            }
                        }
                    }

                }
            }
            return jogador;
        }

        public ICollection<Jogador> GetJogadoresComPostagens()
        {
            IList<Jogador> listaJogadores = new List<Jogador>();
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
                                        NomeCategoriaPostagem = result["Categoria_Postagem"].ToString()
                                    },
                                    Grupo = new Grupo
                                    {
                                        Id = (int)result["Id_Grupo"],
                                        Descricao = result["Descricao_Grupo"].ToString(),

                                        Categoria = new CategoriaGrupo
                                        {
                                            Id = (int)result["Id_Categoria_Grupo"],
                                            NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString()
                                        },
                                        Jogadores = null
                                    }
                                };
                            }

                            if (!listaJogadores.Any(x => x.Id == (int)result["Id_Jogador"]))
                            {
                                var jogador = new Jogador
                                {
                                    Id = (int)result["Id_Jogador"],
                                    Nome = result["Nome_Do_Jogador"].ToString(),
                                    Email = result["Email_Do_Jogador"].ToString(),
                                    Usuario = result["Usuario_Do_Jogador"].ToString(),
                                    Senha = result["Senha_Usuario"].ToString(),
                                    Grupos = null
                                };

                                if ((jogador?.Id ?? 0) > 0)
                                {
                                    jogador.Postagens.Add(postagem);
                                }

                                listaJogadores.Add(jogador);
                            }
                            else if ((postagem?.Id ?? 0) > 0) // grupo?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca o jogador e adiciona o grupo na lista de grupos dos quais o jogador participa
                                listaJogadores.FirstOrDefault(x => x.Id == (int)result["Id_Jogador"]).Postagens.Add(postagem);
                            }
                        }
                    }

                }
            }
            return listaJogadores;
        }

        public Jogador GetJogadorPorIdComPostagens(int id)
        {
            Jogador jogador = GetJogadorPorId(id);
            if (jogador is null)
            {
                return null;
            }
            jogador.Postagens = new List<Postagem>();
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
                                WHERE J.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = jogador.Id;
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
                                    Titulo = result["Titulo_Postagem"].ToString(),
                                    Texto = result["Texto_Postagem"].ToString(),
                                    Imagem = result["Imagem_Postagem"].ToString(),
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    CategoriaPostagem = new CategoriaPostagem
                                    {
                                        Id = (int)result["Id_Categoria_Postagem"],
                                        NomeCategoriaPostagem = result["Categoria_Postagem"].ToString()
                                    },
                                    Grupo = new Grupo
                                    {
                                        Id = (int)result["Id_Grupo"],
                                        Descricao = result["Descricao_Grupo"].ToString(),

                                        Categoria = new CategoriaGrupo
                                        {
                                            Id = (int)result["Id_Categoria_Grupo"],
                                            NomeCategoriaGrupo = result["Nome_Categoria_Grupo"].ToString()
                                        },
                                        Jogadores = null
                                    }
                                };
                                jogador.Postagens.Add(postagem);
                            }
                        }
                    }
                }
            }
            return jogador;
        }
        public bool UpdateJogador(int id, Jogador jogador)
        {
            throw new System.NotImplementedException();
        }
        public bool DeleteJogador(int id)
        {
            throw new System.NotImplementedException();
        }


    }
}
