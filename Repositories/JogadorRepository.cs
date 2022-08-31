using ForumGames.Interfaces;
using ForumGames.Models;
using ForumGames.Utils;
using ForumGames.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace ForumGames.Repositories
{
    public class JogadorRepository : IJogadorRepository
    {
        public JogadorRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("ForumGames"); // Connection String recuperada do arquivo appsettings.json
        }
        public IConfiguration Configuration { get; set; }
        public string connectionString { get; set; }
        /// <summary>
        /// Inserir um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
        public Jogador InsertJogador(Jogador jogador)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_Jogadores 
                                        (Nome, Email, Usuario, Senha)
                                    VALUES
                                        (@Nome, @Email, @Usuario, @Senha)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = jogador.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = jogador.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = jogador.Senha;
                    cmd.Parameters.Add("Usuario", SqlDbType.NVarChar).Value = jogador.Usuario;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return jogador;
        }
        /// <summary>
        /// Inserir um jogador com imagem no banco de dados. Extensões permitidas: jpg, jpeg, png, svg
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <param name="arquivo">Imagem a ser inserida</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
        public Jogador InsertJogadorComImagem(Jogador jogador, IFormFile arquivo)
        {
            
            string imagem = Upload.UploadFile(arquivo);
            jogador.Imagem = imagem;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_Jogadores 
                                        (Nome, Email, Usuario, Senha, Imagem)
                                    VALUES
                                        (@Nome, @Email, @Usuario, @Senha, @Imagem)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = jogador.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = jogador.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = jogador.Senha;
                    cmd.Parameters.Add("Usuario", SqlDbType.NVarChar).Value = jogador.Usuario;
                    cmd.Parameters.Add("Imagem", SqlDbType.NVarChar).Value = imagem;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                
            }
            return jogador;
        }
        /// <summary>
        /// Exibir uma lista de todos os jogadores cadastrados. 
        /// Para visualizar a imagem inserida é necessário utilizar o endereço: https://localhost:5001/StaticFiles/Images/nome_da_imagem.extensão
        /// </summary>
        /// <returns>Retorna todos os jogadores</returns>
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
                                    J.Email AS 'Email_do_Usuario',
                                    J.Imagem AS 'Imagem_Jogador'
                                FROM TB_Jogadores AS J";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
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
                                Postagens = null,
                                Imagem  = result["Imagem_Jogador"].ToString()
                            });
                        }
                    }

                }
            }
            return listaJogadores;
        }
        /// <summary>
        /// Exibir um jogador passando o seu Id
        /// Para visualizar a imagem inserida é necessário utilizar o endereço: https://localhost:5001/StaticFiles/Images/nome_da_imagem.extensão
        /// </summary>
        /// <param name="id">Id do jogador a ser buscado</param>
        /// <returns>Retorna um único jogador</returns>
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
                                    J.Email AS 'Email_do_Usuario',
                                    J.Imagem AS 'Imagem_Jogador'
                                FROM TB_Jogadores AS J WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
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
                                Postagens = null,
                                Imagem = result["Imagem_Jogador"].ToString()
                            };
                        }
                    }

                }
            }
            return jogador;
        }
        /// <summary>
        /// Exibir uma lista de todos os jogadores que participam de algum grupo
        /// </summary>
        /// <returns>Retorna os jogadores e seus respectivos grupos</returns>
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
                                    J.Imagem AS 'Imagem_Jogador',
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
                                    Postagens = null,
                                    Imagem = result["Imagem_Jogador"].ToString()
                                };

                                if ((jogador?.Id ?? 0) > 0)
                                {
                                    jogador.Grupos.Add(grupo);
                                }

                                listaJogadores.Add(jogador);
                            }
                            else if ((grupo?.Id ?? 0) > 0)  // grupo?.Id ?? 0 -> Garante que se o grupo for nulo, atribui o valor 0 ao Id e compara se é maior do que zero.
                                                            // Isso é utilizado para evitar objeto nulo
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
        /// Exibir um jogador e os grupos dos quais ele participa
        /// </summary>
        /// <param name="id">Id do jogador a ser buscado</param>
        /// <returns>Retorna o jogador e os grupos dos quais ele participa</returns>
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
        /// <summary>
        /// Exibir uma lista de todos os jogadores e as suas postagens feitas
        /// </summary>
        /// <returns>Retorna os jogadores e suas respectivas postagens</returns>
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
                                    J.Imagem AS 'Imagem_Jogador',
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
                                    Grupos = null,
                                    Imagem = result["Imagem_Jogador"].ToString()
                                };

                                if ((jogador?.Id ?? 0) > 0)
                                {
                                    jogador.Postagens.Add(postagem);
                                }

                                listaJogadores.Add(jogador);
                            }
                            else if ((postagem?.Id ?? 0) > 0) // postagem?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca o jogador e adiciona a postagem na lista de postagens do jogador.
                                listaJogadores.FirstOrDefault(x => x.Id == (int)result["Id_Jogador"]).Postagens.Add(postagem);
                            }
                        }
                    }

                }
            }
            return listaJogadores;
        }
        /// <summary>
        /// Mostra um jogador e todas as suas postagens a partir do Id fornecido
        /// <para>Se não houver postagens, mostra o jogador e o campo postagem vazio</para>
        /// </summary>
        /// <param name="id">Id do jogador</param>
        /// <returns>Retorna um <b>Jogador</b> com as suas postagens feitas</returns>
        public Jogador GetJogadorPorIdComPostagens(int id)
        {
            Jogador jogador = GetJogadorPorId(id); // Verifica se o jogador existe no banco de dados
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
        /// <summary>
        /// Atualizar um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <returns>Retorna uma mensagem sobre a operação de atualização a ser realizada</returns>
        public bool UpdateJogador(int id, Jogador jogador)
        {
            if (GetJogadorPorId(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_Jogadores 
                                    SET 
                                        Nome = @Nome, 
                                        Email = @Email,     
                                        Usuario = @Usuario, 
                                        Senha = @Senha
                                WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = jogador.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = jogador.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = jogador.Senha;
                    cmd.Parameters.Add("Usuario", SqlDbType.NVarChar).Value = jogador.Usuario;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        /// <summary>
        /// Atualizar um jogador com imagem no banco de dados. Extensões permitidas: jpg, jpeg, png, svg
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <param name="arquivo">Imagem a ser atualizada</param>
        /// <returns>Retorna uma mensagem sobre a operação de atualização a ser realizada</returns>
        public bool UpdateJogadorComImagem(int id, Jogador jogador, IFormFile arquivo)
        {
            if (GetJogadorPorId(id) is null)
            {
                return false;
            }
            string imagem = Upload.UploadFile(arquivo);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_Jogadores 
                                    SET 
                                        Nome = @Nome, 
                                        Email = @Email,     
                                        Senha = @Senha,
                                        Usuario = @Usuario, 
                                        Imagem = @Imagem
                                WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = jogador.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = jogador.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = jogador.Senha;
                    cmd.Parameters.Add("Usuario", SqlDbType.NVarChar).Value = jogador.Usuario;
                    cmd.Parameters.Add("Imagem", SqlDbType.Text).Value = imagem;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        /// <summary>
        /// Deletar um jogador existente no banco de dados se ele não houver feito qualquer postagem
        /// </summary>
        /// <param name="id">Id do jogador</param>
        /// <returns>Retorna o resultado booleano da operação</returns>
        public bool DeleteJogador(int id)
        {
            var jogador = GetJogadorPorIdComPostagens(id);
            var jogadorGrupo = GetJogadorPorIdComGrupos(id);
            if (jogador is null)
            {
                return false;
            }
            if (jogador.Postagens.Count > 0)
            {
                throw new NaoPodeDeletarException("O jogador não pôde ser deletado, pois possui postagens em seu nome. Apague as postagens primeiro");
            }
            if (jogadorGrupo.Grupos.Count > 0)
            {
                throw new NaoPodeDeletarException("O jogador não pôde ser deletado, pois participa de algum grupo. Apague o relacionamento com esse(s) grupo(s) primeiro");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string scriptDelete = @"DELETE FROM TB_Jogadores WHERE Id = @id";

                // Execução no banco
                using (var cmd = new SqlCommand(scriptDelete, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
