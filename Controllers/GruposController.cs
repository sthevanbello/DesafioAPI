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
    public class GruposController : ControllerBase
    {
        private readonly IGrupoRepository _grupoRepository;

        public GruposController(IGrupoRepository grupoRepository)
        {
            _grupoRepository = grupoRepository;
        }

        /// <summary>
        /// Inserir um grupo no banco de dados com uma categoria existente
        /// </summary>
        /// <param name="grupo">Grupo informado</param>
        /// <returns>Retorna o grupo que foi inserido</returns>
        [HttpPost]
        public IActionResult InsertGrupo(Grupo grupo)
        {
            try
            {
                _grupoRepository.InsertGrupo(grupo);
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
            catch (ThereIsntCategoryException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha ao inserir grupo",
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
        /// <summary>
        /// Exibir uma lista com todos os grupos, postagens e os jogadores que integram o grupo.
        /// </summary>
        /// <returns>Retorna uma <b>List</b> com todos os grupos, postagens e os jogadores que integram o grupo</returns>
        [HttpGet("Postagens")]
        public IActionResult GetAllGruposComPostagensEJogadores()
        {
            try
            {
                var grupos = _grupoRepository.GetAllGruposComPostagensEJogadores();
                return Ok(grupos);
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
        /// Exibe um Grupo com as Postagens e os jogadores que fizeram as postagens
        /// </summary>
        /// <param name="id">Id do grupo</param>
        /// <returns>Retorna um <b>Grupo</b> com as Postagens e os jogadores que fizeram as postagens</returns>
        [HttpGet("Postagens/{id}")]
        public IActionResult GetGrupoPorIdComPostagensEJogadores(int id)
        {
            try
            {
                var grupo = _grupoRepository.GetGrupoPorIdComPostagensEJogadores(id);
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
        /// Atualizar um grupo existente no banco de dados com uma categoria existente
        /// </summary>
        /// <param name="id">Id do grupo a ser atualizado</param>
        /// <param name="grupo">Dados atualizados do grupo</param>
        /// <returns>Retorna o grupo que foi inserido</returns>
        [HttpPut]
        public IActionResult UpdateGrupo(int id, Grupo grupo)
        {
            try
            {
                _grupoRepository.UpdateGrupo(id, grupo);
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
            catch (ThereIsntCategoryException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha ao inserir grupo",
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
