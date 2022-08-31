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
        // Propriedade criada para utilizar o repositório através de injeção de dependência pelo construtor
        // Isso é feito para não instanciar um objeto de uma classe externa e manter um baixo acoplamento entre classes.
        private readonly IJogadorRepository _jogadorRepository;
        public JogadoresController(IJogadorRepository jogadorRepository)
        {
            _jogadorRepository = jogadorRepository;
        }
        /// <summary>
        /// Inserir um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
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
        /// Inserir um jogador com imagem no banco de dados. Extensões permitidas: jpg, jpeg, png, svg
        /// </summary>
        /// <param name="jogador">Jogador a ser inserido</param>
        /// <param name="arquivo">Imagem a ser inserida</param>
        /// <returns>Retorna um jogador após inserí-lo no banco</returns>
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
        /// Exibir uma lista de todos os jogadores cadastrados. 
        /// Para visualizar a imagem inserida é necessário utilizar o endereço: https://localhost:5001/StaticFiles/Images/nome_da_imagem.extensão
        /// </summary>
        /// <returns>Retorna todos os jogadores</returns>
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
        /// Para visualizar a imagem inserida é necessário utilizar o endereço: https://localhost:5001/StaticFiles/Images/nome_da_imagem.extensão
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
        /// Mostra um jogador e todas as suas postagens a partir do Id fornecido - 
        /// Se não houver postagens, mostra o jogador e o campo postagem vazio
        /// </summary>
        /// <param name="id">Id do jogador</param>
        /// <returns>Retorna um <b>Jogador</b> com as suas postagens feitas</returns>
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
        /// Atualizar um jogador no banco de dados
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <returns>Retorna uma mensagem sobre a operação de atualização a ser realizada</returns>
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
        /// Atualizar um jogador com imagem no banco de dados. Extensões permitidas: jpg, jpeg, png, svg
        /// </summary>
        /// <param name="jogador">Jogador a ser atualizado</param>
        /// <param name="id">Id do jogador a ser atualizado</param>
        /// <param name="arquivo">Imagem a ser atualizada</param>
        /// <returns>Retorna uma mensagem sobre a operação de atualização a ser realizada</returns>
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
        /// Deletar um jogador existente no banco de dados se ele não houver feito qualquer postagem
        /// </summary>
        /// <param name="id">Id do jogador</param>
        /// <returns>Retorna o resultado booleano da operação</returns>
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
