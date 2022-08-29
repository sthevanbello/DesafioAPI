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
    }
}
