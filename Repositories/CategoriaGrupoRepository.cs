using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ForumGames.Repositories
{
    public class CategoriaGrupoRepository : ICategoriaGrupoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";

        public CategoriaGrupo InsertCategoriaGrupo(CategoriaGrupo categoriaGrupo)
        {
            throw new System.NotImplementedException();
        }
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
                            });
                        }
                    }

                }
            }
            return listaCategoriaGrupo;
        }
        public CategoriaGrupo GetCategoriaGrupoPorId(int id)
        {
            throw new System.NotImplementedException();
        }
        public bool UpdateCategoriaGrupo(int id, CategoriaGrupo categoriaGrupo)
        {
            throw new System.NotImplementedException();
        }
        public bool DeleteCategoriaGrupo(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
