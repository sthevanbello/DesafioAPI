using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ForumGames.Utils.Exceptions;

namespace ForumGames.Repositories
{
    public class CategoriaGrupoRepository : ICategoriaGrupoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";

        /// <summary>
        /// Inserir uma Categoria de grupo nova no banco de dados
        /// </summary>
        /// <param name="categoriaGrupo">Categoria de grupo nova</param>
        /// <returns>Retorna a categoria de grupo nova</returns>
        public CategoriaGrupo InsertCategoriaGrupo(CategoriaGrupo categoriaGrupo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_Categorias_Grupos (Categoria) 
                                        VALUES	(@NomeCategoria)";


                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
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
        /// Lista todas as categorias de grupos cadastradas
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
                    // Ler todos os itens da consulta com foreach e while
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
        /// Exibe uma única categoria a partir do Id fornecido como parâmetro
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
                    // Ler todos os itens da consulta com foreach e while
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
        /// Exibe uma única categoria a partir do Id fornecido como parâmetro e exibe de quais grupos ela faz parte
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
        /// Atualiza uma categoria de acordo com o Id fornecido
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
        public bool DeleteCategoriaGrupo(int id)
        {
            var categoriaGrupo = GetCategoriaGrupoPorIdComGrupos(id);
            if (categoriaGrupo is null)
            {
                return false;
            }
            if (categoriaGrupo.Grupos.Count > 0)
            {
                throw new CannotDeleteException("A categoria de grupos não pôde ser deletada, pois possui grupo criado com a categoria. Apague o grupo primeiro");
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
