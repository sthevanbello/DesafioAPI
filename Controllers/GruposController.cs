using ForumGames.Interfaces;
using ForumGames.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GruposController : ControllerBase
    {
        private readonly IGrupoRepository _grupoRepository;

        public GruposController(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }
        /// <summary>
        /// Exibir todos os Grupos contidos no banco de dados
        /// </summary>
        /// <returns>Retorna uma <b>Lista</b> de Grupos</returns>
        [HttpGet]
        public IActionResult GetAllGrupos()
        {
            try
            {
                var listaGrupos = _grupoRepository.GetAllGrupos();
                return Ok(listaGrupos);
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
        /// Exibir um grupo passando o seu Id
        /// </summary>
        /// <param name="id">Id do grupo a ser buscado</param>
        /// <returns>Retorna um único grupo</returns>
        [HttpGet("{id}")]
        public IActionResult GetGrupoPorId(int id)
        {
            try
            {
                var grupo = _grupoRepository.GetGrupoPorId(id);
                if (grupo is null)
                {
                    return NotFound(new { msg = "Grupo não encontrado. Verifique se o Id está correto" });
                }
                return Ok(grupo);
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
        /// Exibir uma lista de todos os grupos e os jogadores desses grupos
        /// </summary>
        /// <returns>Retorna uma lista de todos os grupos e os jogadores desses grupos</returns>
        [HttpGet("Jogadores")]
        public IActionResult GetAllGruposComJogadores()
        {
            try
            {
                var grupoJogadores = _grupoRepository.GetAllGruposComJogadores();
                return Ok(grupoJogadores);
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
        /// Exibir um grupo e os jogadores que o integram
        /// </summary>
        /// <param name="id">Id do grupo a ser buscado</param>
        /// <returns>Retorna um grupo e os jogadores que o integram</returns>
        [HttpGet("Jogadores/{id}")]
        public IActionResult GetGrupoPorIdComJogadores(int id)
        {
            try
            {
                var grupo = _grupoRepository.GetGrupoPorIdComJogadores(id);
                if (grupo is null)
                {
                    return NotFound(new { msg = "Grupo não encontrado. Verifique se o Id está correto" });
                }
                return Ok(grupo);
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
