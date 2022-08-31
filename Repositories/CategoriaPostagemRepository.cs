using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System;
using ForumGames.Utils.Exceptions;
using Microsoft.Extensions.Configuration;

namespace ForumGames.Repositories
{
    public class CategoriaPostagemRepository : ICategoriaPostagemRepository
    {
        public CategoriaPostagemRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("ForumGames"); // Connection String recuperada do arquivo appsettings.json
        }
        public IConfiguration Configuration { get; set; }
        public string connectionString { get; set; }

        /// <summary>
        /// Inserir uma Categoria de postagem nova no banco de dados
        /// </summary>
        /// <param name="categoriaPostagem">Categoria de postagem nova</param>
        /// <returns>Retorna a categoria de grupo nova</returns>
        public CategoriaPostagem InsertCategoriaPostagem(CategoriaPostagem categoriaPostagem)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_Categorias_Postagens (Categoria) 
                                        VALUES	(@NomeCategoria)";


                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoriaPostagem.NomeCategoriaPostagem;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return categoriaPostagem;
        }

        /// <summary>
        /// Exibir todas as categorias de postagens cadastradas
        /// </summary>
        /// <returns>Retorna todas as categorias de postagens cadastradas</returns>
        public ICollection<CategoriaPostagem> GetAllCategoriaPostagem()
        {
            var listaCategoriaPostagem = new List<CategoriaPostagem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CP.Id AS 'Id_Categoria',
                                    CP.Categoria AS 'Categoria_Nome'
                                FROM TB_Categorias_Postagens AS CP";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaCategoriaPostagem.Add(new CategoriaPostagem
                            {
                                Id = (int)result["Id_Categoria"],
                                NomeCategoriaPostagem = (string)result["Categoria_Nome"],
                                Postagens = null
                            });
                        }
                    }

                }
            }
            return listaCategoriaPostagem;
        }
        /// <summary>
        /// Exibir uma única categoria a partir do Id fornecido como parâmetro
        /// </summary>
        /// <param name="id">Id da Categoria de Postagem</param>
        /// <returns>Retorna uma única <b>CategoriaPostagem</b> </returns>
        public CategoriaPostagem GetCategoriaPostagemPorId(int id)
        {
            CategoriaPostagem categoriaPostagem = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CP.Id AS 'Id_Categoria',
                                    CP.Categoria AS 'Categoria_Nome'
                                FROM TB_Categorias_Postagens AS CP
                                WHERE CP.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (result != null && result.HasRows && result.Read())
                        {
                            categoriaPostagem = new CategoriaPostagem
                            {
                                Id = (int)result["Id_Categoria"],
                                NomeCategoriaPostagem = (string)result["Categoria_Nome"],
                                Postagens = null
                            };
                        }
                    }

                }
            }
            return categoriaPostagem;
        }
        /// <summary>
        /// Exibir todas as categorias e todos as postagens referentes à categoria
        /// </summary>
        /// <returns>Retorna todas as categorias e todos as postagens referentes à categoria</returns>
        public IList<CategoriaPostagem> GetAllCategoriaPostagemComPostagens()
        {
            IList<CategoriaPostagem> listaCategoriaPostagem = new List<CategoriaPostagem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CP.Id AS 'Id_Categoria',
                                    CP.Categoria AS 'Categoria_Nome',
	                                P.CategoriaPostagemId AS 'ID_Categoria_Postagem',
	                                P.Id AS 'Id_Postagem',
	                                P.Titulo AS 'Titulo_Postagem',
	                                P.Texto AS 'Texto_Postagem',
	                                P.Imagem AS 'LinkImagem_Postagem',
	                                P.DataHora AS 'DataHora_Postagem',
	                                P.GrupoId AS 'Id_Grupo_Postagem',
	                                P.JogadorId AS 'Id_Jogador_Postagem'
                                FROM TB_Categorias_Postagens AS CP
                                LEFT JOIN TB_Postagens AS P ON CP.Id = P.CategoriaPostagemId";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
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
                                    Imagem = result["LinkImagem_Postagem"].ToString(),
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    Grupo = null,
                                    Jogador = null,
                                    CategoriaPostagem = null
                                };

                            }
                            if (!listaCategoriaPostagem.Any(x => x.Id == (int)result["Id_Categoria"]))
                            {
                                var categoriaPostagem = new CategoriaPostagem
                                {
                                    Id = (int)result["Id_Categoria"],
                                    NomeCategoriaPostagem = (string)result["Categoria_Nome"]
                                };

                                if ((categoriaPostagem?.Id ?? 0) > 0)
                                {
                                    categoriaPostagem.Postagens.Add(postagem);
                                }

                                listaCategoriaPostagem.Add(categoriaPostagem);
                            }
                            else if ((postagem?.Id ?? 0) > 0) // grupo?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                                                              // Isso é para evitar falha de objeto nulo
                            {
                                // Busca a categoria e adiciona a postagem na lista de postagem nas quais a categoria é utilizada
                                listaCategoriaPostagem.FirstOrDefault(x => x.Id == (int)result["Id_Categoria"]).Postagens.Add(postagem);
                            }

                        }
                    }
                }
            }
            return listaCategoriaPostagem;
        }
        /// <summary>
        /// Exibir uma CategoriaPostagem e suas respectivas postagens
        /// </summary>
        /// <param name="id">Id da Categoria de Postagem</param>
        /// <returns>Retorna uma CategoriaPostagem com suas respectivas postagens</returns>
        public CategoriaPostagem GetCategoriaPostagemPorIdComPostagens(int id)
        {
            var categoriaPostagem = GetCategoriaPostagemPorId(id);
            if (categoriaPostagem is null)
            {
                return null;
            }
            categoriaPostagem.Postagens = new List<Postagem>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CP.Id AS 'Id_Categoria',
                                    CP.Categoria AS 'Categoria_Nome',
	                                P.CategoriaPostagemId AS 'ID_Categoria_Postagem',
	                                P.Id AS 'Id_Postagem',
	                                P.Titulo AS 'Titulo_Postagem',
	                                P.Texto AS 'Texto_Postagem',
	                                P.Imagem AS 'LinkImagem_Postagem',
	                                P.DataHora AS 'DataHora_Postagem',
	                                P.GrupoId AS 'Id_Grupo_Postagem',
	                                P.JogadorId AS 'Id_Jogador_Postagem'
                                FROM TB_Categorias_Postagens AS CP
                                LEFT JOIN TB_Postagens AS P ON CP.Id = P.CategoriaPostagemId
                                WHERE CP.Id = @id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
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
                                    Imagem = result["LinkImagem_Postagem"].ToString(),
                                    DataHora = Convert.ToDateTime(result["DataHora_Postagem"]),
                                    Grupo = null,
                                    Jogador = null,
                                    CategoriaPostagem = null
                                };
                                categoriaPostagem.Postagens.Add(postagem);
                            }
                        }
                    }
                }
            }
            return categoriaPostagem;
        }
        /// <summary>
        /// Atualizar uma categoria de acordo com o Id fornecido
        /// </summary>
        /// <param name="id">Id da categoriaa ser atualizada</param>
        /// <param name="categoriaPostagem">Dados atualizados</param>
        /// <returns>Retorna se a categoria foi alterada ou não foi alterada</returns>
        public bool UpdateCategoriaPostagem(int id, CategoriaPostagem categoriaPostagem)
        {
            if (GetCategoriaPostagemPorId(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_Categorias_Postagens 
                                    SET 
                                        Categoria = @Categoria
                                WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("Categoria", SqlDbType.NVarChar).Value = categoriaPostagem.NomeCategoriaPostagem;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        /// <summary>
        /// Excluir uma categoria de Postagem no banco de dados
        /// </summary>
        /// <param name="id">Id da categoria de Postagem</param>
        /// <returns>Retorna um bool sobre a operação de exclusão a ser realizada</returns>
        public bool DeleteCategoriaPostagem(int id)
        {
            var categoriaPostagem = GetCategoriaPostagemPorIdComPostagens(id);
            if (categoriaPostagem is null)
            {
                return false;
            }
            if (categoriaPostagem.Postagens.Count > 0)
            {
                throw new NaoPodeDeletarException("A categoria de postagens não pôde ser deletada, pois possui alguma postagem criada com essa categoria. Apague as postagens primeiro");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"DELETE FROM TB_Categorias_Postagens WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
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
