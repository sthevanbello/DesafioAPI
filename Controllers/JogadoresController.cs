using ForumGames.Interfaces;
using ForumGames.Models;
using ForumGames.Repositories;
using ForumGames.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JogadoresController : ControllerBase
    {
        private readonly IJogadorRepository _jogadorRepository;


        public JogadoresController(IJogadorRepository jogadorRepository)
        {
            _jogadorRepository = jogadorRepository;
        }
        /// <summary>
        /// Exibir uma lista de todos os jogadores cadastrados
        /// </summary>
        /// <returns>Retorna todos os jogadores</returns>
        // Get - Consultar
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
        /// Exibir um jogador passando o seu Id
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
        /// Exibir uma lista de todos os jogadores que participam de algum grupo
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
        /// Exibir um jogador e os grupos dos quais ele participa
        /// </summary>
        /// <param name="id">Id do jogador a ser buscado</param>
        /// <returns>Retorna o jogador e os grupos dos quais ele participa</returns>
        [HttpGet("Grupos/{id}")]
        public IActionResult GetJogadorPorIdComGrupos(int id)
        {
            try
            {
                var jogadores = _jogadorRepository.GetJogadorPorIdComGrupos(id);
                if (jogadores is null)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
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
        /// Exibir uma lista de todos os jogadores e as suas postagens feitas
        /// </summary>
        /// <returns>Retorna os jogadores e suas respectivas postagens</returns>
        [HttpGet("Postagens")]
        public IActionResult GetJogadoresComPostagens()
        {
            try
            {
                var jogadores = _jogadorRepository.GetJogadoresComPostagens();
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
        /// Exibir Jogador com as postagens feitas
        /// </summary>
        /// <param name="id">Id do jogador a ser buscado</param>
        /// <returns>Retorna um <b>Jogador</b> com as postagens</returns>
        [HttpGet("Postagens/{id}")]
        public IActionResult GetJogadorPorIdComPostagens(int id)
        {
            try
            {
                var jogadores = _jogadorRepository.GetJogadorPorIdComPostagens(id);
                if (jogadores is null)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
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
        /// Inserir um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
        // POST - Cadastrar
        [HttpPost]
        public IActionResult Insertjogador(Jogador jogador)
        {
            try
            {
                var jogadorInserido = _jogadorRepository.InsertJogador(jogador);
                return Ok(jogadorInserido);
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
        /// Inserir um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
        // POST - Cadastrar
        [HttpPost("Imagem")]
        public IActionResult InsertjogadorComImagem([FromForm] Jogador jogador, IFormFile arquivo)
        {
            try
            {
                var jogadorInserido = _jogadorRepository.InsertJogadorComImagem(jogador, arquivo);
                return Ok(jogadorInserido);
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
        /// Atualizar um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        // PUT - Alterar
        [HttpPut("{id}")]
        public IActionResult UpdateJogador(int id, Jogador jogador)
        {
            try
            {
                var jogadoratualizado = _jogadorRepository.UpdateJogador(id, jogador);
                if (!jogadoratualizado)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Jogador atualizado com sucesso.", jogador });
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
        /// Atualizar um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <param name="arquivo">Imagem a ser atualizada</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        // PUT - Alterar
        [HttpPut("Imagem/{id}")]
        public IActionResult UpdateJogadorComImagem(int id, [FromForm] Jogador jogador, IFormFile arquivo)
        {
            try
            {
                var jogadoratualizado = _jogadorRepository.UpdateJogadorComImagem(id, jogador, arquivo);
                if (!jogadoratualizado)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Jogador atualizado com sucesso.", jogador });
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
        /// Excluir um jogador no banco de dados
        /// </summary>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        // DELETE - Deletar
        [HttpDelete("{id}")]
        public IActionResult DeleteJogador(int id)
        {
            try
            {
                var jogadorDeletado = _jogadorRepository.DeleteJogador(id);
                if (!jogadorDeletado)
                {
                    return NotFound(new { msg = "Jogador não encontrado. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Jogador excluído com sucesso." });
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
            catch (NaoPodeDeletarException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha ao excluir o jogador",
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
