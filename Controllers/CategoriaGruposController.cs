using ForumGames.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaGruposController : ControllerBase
    {
        private readonly ICategoriaGrupoRepository _categoriaGrupoRepository;

        public CategoriaGruposController(ICategoriaGrupoRepository categoriaGrupoRepository)
        {
            _categoriaGrupoRepository = categoriaGrupoRepository;
        }

        /// <summary>
        /// Lista todas as categorias de grupos cadastradas
        /// </summary>
        /// <returns>Retorna todas as categorias de grupos cadastradas</returns>
        // Get
        [HttpGet]
        public IActionResult GetCategoriasGrupos()
        {
            try
            {
                var listaCategorias = _categoriaGrupoRepository.GetAllCategoriaGrupo();
                return Ok(listaCategorias);
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
