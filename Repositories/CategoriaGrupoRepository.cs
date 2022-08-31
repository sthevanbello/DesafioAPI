using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ForumGames.Utils.Exceptions;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ForumGames.Repositories
{
    public class CategoriaGrupoRepository : ICategoriaGrupoRepository
    {
        public CategoriaGrupoRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            connectionString = Configuration.GetConnectionString("ForumGames"); // Connection String recuperada do arquivo appsettings.json
        }
        public IConfiguration Configuration { get; set; }
        public string connectionString { get; set; }

        /// <summary>
        /// Inserir uma Categoria de grupo nova no banco de dados
        /// </summary>
        /// <param name="categoriaGrupo">Categoria de grupo nova</param>
        /// <returns>Retorna a categoria de grupo nova</returns>
        public CategoriaGrupo InsertCategoriaGrupo(CategoriaGrupo categoriaGrupo)
        {
            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_Categorias_Grupos (Categoria) 
                                        VALUES	(@NomeCategoria)";


                // Execução no banco
                using (SqlCommand cmd = new(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoriaGrupo.NomeCategoriaGrupo;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return categoriaGrupo;
        }

        /// <summary>
        /// Exibir todas as categorias de grupos cadastradas
        /// </summary>
        /// <returns>Retorna todas as categorias de grupos cadastradas</returns>
        public ICollection<CategoriaGrupo> GetAllCategoriaGrupo()
        {
            var listaCategoriaGrupo = new List<CategoriaGrupo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CG.Id AS 'Id_Categoria',
                                    CG.Categoria AS 'Categoria_Nome'
                                FROM TB_Categorias_Grupos AS CG";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaCategoriaGrupo.Add(new CategoriaGrupo
                            {
                                Id = (int)result["Id_Categoria"],
                                NomeCategoriaGrupo = (string)result["Categoria_Nome"],
                                Grupos = null
                            });
                        }
                    }

                }
            }
            return listaCategoriaGrupo;
        }
        /// <summary>
        /// Exibir uma única categoria a partir do Id fornecido como parâmetro
        /// </summary>
        /// <param name="id">Id da Categoria de Grupo</param>
        /// <returns>Retorna uma única <b>CategoriaGrupo</b> </returns>
        public CategoriaGrupo GetCategoriaGrupoPorId(int id)
        {
            CategoriaGrupo categoriaGrupo = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CG.Id AS 'Id_Categoria',
                                    CG.Categoria AS 'Categoria_Nome'
                                FROM TB_Categorias_Grupos AS CG
                                WHERE CG.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (result != null && result.HasRows && result.Read())
                        {
                            categoriaGrupo = new CategoriaGrupo
                            {
                                Id = (int)result["Id_Categoria"],
                                NomeCategoriaGrupo = (string)result["Categoria_Nome"],
                                Grupos = null
                            };
                        }
                    }

                }
            }
            return categoriaGrupo;
        }
        /// <summary>
        /// Exibir todas as categorias e todos os grupos cadastrados
        /// </summary>
        /// <returns>Retorna todas as categorias e todos os grupos cadastrados</returns>
        public IList<CategoriaGrupo> GetAllCategoriaGrupoComGrupos()
        {
            IList<CategoriaGrupo> listaCategoriaGrupo = new List<CategoriaGrupo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CG.Id AS 'Id_Categoria',
                                    CG.Categoria AS 'Categoria_Nome',
	                                G.CategoriaId AS 'ID_Categoria_Grupo',
	                                G.Id AS 'Id_Grupo',
	                                G.Descricao AS 'Grupo_Descricao'
                                FROM TB_Categorias_Grupos AS CG
                                LEFT JOIN TB_Grupos AS G ON CG.Id = G.CategoriaId";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
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
                                    Categoria = null,
                                    Descricao = result["Grupo_Descricao"].ToString(),
                                    Jogadores = null,
                                    Postagens = null
                                };

                            }
                            if (!listaCategoriaGrupo.Any(x => x.Id == (int)result["Id_Categoria"]))
                            {
                                var categoriaGrupo = new CategoriaGrupo
                                {
                                    Id = (int)result["Id_Categoria"],
                                    NomeCategoriaGrupo = (string)result["Categoria_Nome"],
                                };

                                if ((categoriaGrupo?.Id ?? 0) > 0)
                                {
                                    categoriaGrupo.Grupos.Add(grupo);
                                }

                                listaCategoriaGrupo.Add(categoriaGrupo);
                            }
                            else if ((grupo?.Id ?? 0) > 0) // grupo?.Id ?? 0 -> Garante que se for nulo, atribui o valor 0 e compara se é maior do que zero.
                            {
                                // Busca a categoria e adiciona o grupo na lista de grupos nos quais a categoria é utilizada
                                listaCategoriaGrupo.FirstOrDefault(x => x.Id == (int)result["Id_Categoria"]).Grupos.Add(grupo);
                            }

                        }
                    }
                }
            }
            return listaCategoriaGrupo;
        }
        /// <summary>
        /// Exibir uma única categoria a partir do Id fornecido como parâmetro e exibe de quais grupos ela faz parte
        /// </summary>
        /// <param name="id">Id da Categoria de Grupo</param>
        /// <returns>Retorna uma única <b>CategoriaGrupo</b> com seus respectivos grupos</returns>
        public CategoriaGrupo GetCategoriaGrupoPorIdComGrupos(int id)
        {
            var categoriaGrupo = GetCategoriaGrupoPorId(id);
            if (categoriaGrupo is null)
            {
                return null;
            }
            categoriaGrupo.Grupos = new List<Grupo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    CG.Id AS 'Id_Categoria',
                                    CG.Categoria AS 'Categoria_Nome',
	                                G.CategoriaId AS 'ID_Categoria_Grupo',
	                                G.Id AS 'Id_Grupo',
	                                G.Descricao AS 'Grupo_Descricao'
                                FROM TB_Categorias_Grupos AS CG
                                LEFT JOIN TB_Grupos AS G ON CG.Id = G.CategoriaId
                                WHERE CG.Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            if (!string.IsNullOrEmpty(result["Id_Grupo"].ToString()))
                            {
                                var grupo = new Grupo
                                {
                                    Id = (int)result["Id_Grupo"],
                                    Categoria = null,
                                    Descricao = result["Grupo_Descricao"].ToString(),
                                    Jogadores = null
                                };
                                categoriaGrupo.Grupos.Add(grupo);
                            }
                        }
                    }
                }
            }
            return categoriaGrupo;
        }
        /// <summary>
        /// Atualizar uma categoria de acordo com o Id fornecido
        /// </summary>
        /// <param name="id">Id da categoriaa ser atualizada</param>
        /// <param name="categoriaGrupo">Dados atualizados</param>
        /// <returns>Retorna se a categoria foi alterada ou não foi alterada</returns>
        public bool UpdateCategoriaGrupo(int id, CategoriaGrupo categoriaGrupo)
        {
            if (GetCategoriaGrupoPorId(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_Categorias_Grupos 
                                    SET 
                                        Categoria = @Categoria
                                WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("Categoria", SqlDbType.NVarChar).Value = categoriaGrupo.NomeCategoriaGrupo;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        /// <summary>
        /// Excluir uma categoria de Grupo no banco de dados
        /// </summary>
        /// <param name="id">Id da categoria de Grupo</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        public bool DeleteCategoriaGrupo(int id)
        {
            var categoriaGrupo = GetCategoriaGrupoPorIdComGrupos(id);
            if (categoriaGrupo is null)
            {
                return false;
            }
            if (categoriaGrupo.Grupos.Count > 0)
            {
                throw new NaoPodeDeletarException("A categoria de grupos não pôde ser deletada, pois possui algum grupo criado que está usando a categoria. Apague o grupo primeiro");
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"DELETE FROM TB_Categorias_Grupos WHERE Id = @Id";

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
