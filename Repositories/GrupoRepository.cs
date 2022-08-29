using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

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
                                ORDER BY J.Id";

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
        public Grupo GetGrupoPorIdComJogadores(int id)
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
