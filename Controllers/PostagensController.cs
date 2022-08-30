using ForumGames.Interfaces;
using ForumGames.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using ForumGames.Models;
using ForumGames.Utils.Exceptions;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostagensController : ControllerBase
    {
        private readonly IPostagemRepository _postagemRepository;

        public PostagensController(IPostagemRepository postagemRepository)
        {
            _postagemRepository = postagemRepository;
        }
        /// <summary>
        /// Exibir uma lista de postagens
        /// </summary>
        /// <returns>Retorna uma <b>List</b> de <b>Postagem</b></returns>
        [HttpGet]
        public IActionResult GetPostagens()
        {
            try
            {
                var listaPostagens = _postagemRepository.GetPostagens();
                return Ok(listaPostagens);
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
        /// Exibir uma postagem a partir do seu Id
        /// </summary>
        /// <param name="id">Id da Postagem</param>
        /// <returns>Retorna uma <b>Postagem</b></returns>
        [HttpGet("{id}")]
        public IActionResult GetPostagemPorId(int id)
        {
            try
            {
                var postagem = _postagemRepository.GetPostagemPorId(id);
                if (postagem is null)
                {
                    return NotFound(new { msg = "Postagem não encontrada. Verifique se o Id está correto" });
                }
                return Ok(postagem);
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
        /// Exibe uma lista com as postagens de cada jogador
        /// </summary>
        /// <returns>Retorna uma <b>List</b> de Postagem</returns>
        [HttpGet("Jogadores")]
        public IActionResult GetPostagensComJogador()
        {
            try
            {
                var listaPostagens = _postagemRepository.GetPostagensComJogador();
                return Ok(listaPostagens);
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

        /* Pendente */
        /// <summary>
        /// Inserir uma postagem com Categoria, Jogador e Grupo, incluindo o relacionamento da RL_Jogadores_Grupos 
        /// </summary>
        /// <param name="postagem"></param>
        /// <returns></returns>
        // POST - Cadastrar
        [HttpPost]
        public IActionResult Insertjogador(Postagem postagem)
        {
            try
            {
                var postagemInserida = _postagemRepository.InsertPostagem(postagem);
                return Ok(postagemInserida);
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
            catch (NaoHaJogadorException ex)
            {
                return StatusCode(404, new
                {
                    msg = "Falha ao inserir a postagem",
                    erro = ex.Message
                });
            }
            catch (NaoHaCategoriaException ex)
            {
                return StatusCode(404, new
                {
                    msg = "Falha ao inserir a postagem",
                    erro = ex.Message
                });
            }
            catch (NaoHaGrupoException ex)
            {
                return StatusCode(404, new
                {
                    msg = "Falha ao inserir a postagem",
                    erro = ex.Message
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
