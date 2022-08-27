using ForumGames.Interfaces;
using ForumGames.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadoresController : ControllerBase
    {
        private readonly IJogadorRepository _jogadorRepository;
        //public IJogadorRepository JogadorRepository { get; set; } = new JogadorRepository();
        /*        
        public ICollection<Jogador> GetJogadores();
        public Jogador GetJogadorPorId(int id);
        public ICollection<Jogador> GetJogadoresComGrupos();
        public Jogador GetJogadorPorIdComGrupos(int id);
        public Jogador InsertJogador(Jogador jogador);
        public Jogador InsertJogadorComImagem(Jogador jogador);
        public bool UpdateJogador(int id, Jogador jogador);
        public bool DeleteJogador(int id);
        */

        public JogadoresController(IJogadorRepository jogadorRepository)
        {
            _jogadorRepository = jogadorRepository;
        }
        /// <summary>
        /// Lista todos os jogadores cadastrados
        /// </summary>
        /// <returns>Retorna todos os jogadores</returns>
        // Get
        [HttpGet]
        public IActionResult GetJogadores()
        {
            try
            {
                var listaJogadores = _jogadorRepository.GetJogadores();
                return Ok(listaJogadores);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
            
        }
        /// <summary>
        /// Mostra um jogador passando o seu Id
        /// </summary>
        /// <param name="id">Id do jogador a ser buscado</param>
        /// <returns>Retorna um único jogador</returns>
        [HttpGet("{id}")]
        public IActionResult GetJogadorPorId(int id)
        {
            try
            {
                var jogador = _jogadorRepository.GetJogadorPorId(id);
                if (jogador is null)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
                return Ok(jogador);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }

        }
        /// <summary>
        /// Lista todos os jogadores e os grupos dos quais ele participa
        /// </summary>
        /// <returns>Retorna os jogadores e seus respectivos grupos</returns>
        [HttpGet("Grupos")]
        public IActionResult GetJogadoresComGrupos()
        {
            try
            {
                var jogadores = _jogadorRepository.GetJogadoresComGrupos();
                return Ok(jogadores);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// Lista todos os jogadores e os grupos dos quais ele participa
        /// </summary>
        /// <returns>Retorna os jogadores e seus respectivos grupos</returns>
        [HttpGet("Grupos/{id}")]
        public IActionResult GetJogadoresPorIdComGrupos(int id)
        {
            try
            {
                var jogadores = _jogadorRepository.GetJogadorPorIdComGrupos(id);
                return Ok(jogadores);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }
    }
}
