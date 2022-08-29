using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ForumGames.Repositories
{
    public class GrupoRepository : IGrupoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Forum_Games";
        public Grupo InsertGrupo(Grupo grupo)
        {
            throw new System.NotImplementedException();
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
        public Grupo GetGrupoPorIdComJogadores(int id)
        {
            throw new System.NotImplementedException();
        }
        public ICollection<Grupo> GetAllGruposComJogadores()
        {
            throw new System.NotImplementedException();
        }
        public ICollection<Grupo> GetAllGruposComPostagens()
        {
            throw new System.NotImplementedException();
        }
        public Grupo GetGrupoPorIdComJogadoresComPostagens(int id)
        {
            throw new System.NotImplementedException();
        }
        public ICollection<Grupo> GetAllGruposComJogadoresComPostagens()
        {
            throw new System.NotImplementedException();
        }
        public bool UpdateGrupo(int id, Grupo grupo)
        {
            throw new System.NotImplementedException();
        }
        public bool DeleteGrupo(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
