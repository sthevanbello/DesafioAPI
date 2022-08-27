using ForumGames.Interfaces;
using ForumGames.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
                                Email = (string)result["Email_do_Usuario"]
                            };
                        }
                    }

                }
            }
            return jogador;
        }

        public Jogador GetJogadorPorIdComGrupos(int id)
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
                                Email = (string)result["Email_do_Usuario"]
                                //Imagem = result[4].ToString()
                            });
                        }
                    }

                }
            }
            return listaJogadores;
        }
        public ICollection<Jogador> GetJogadoresComGrupos()
        {
            throw new System.NotImplementedException();
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
