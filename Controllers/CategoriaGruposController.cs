using ForumGames.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using ForumGames.Models;
using ForumGames.Utils.Exceptions;

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
        /// Inserir uma Categoria de grupo nova no banco de dados
        /// </summary>
        /// <param name="categoriaGrupo">Categoria de grupo nova</param>
        /// <returns>Retorna a categoria de grupo nova</returns>
        [HttpPost]
        public IActionResult InsertCategoriaGrupo(CategoriaGrupo categoriaGrupo)
        {
            try
            {
                _categoriaGrupoRepository.InsertCategoriaGrupo(categoriaGrupo);
                return Ok(new {msg = "Categoria de grupo criada com sucesso", categoriaGrupo});
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
        /// Exibir todas as categorias de grupos cadastradas
        /// </summary>
        /// <returns>Retorna todas as categorias de grupos cadastradas</returns>
        // Get
        [HttpGet]
        public IActionResult GetCategoriaGrupo()
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
        /// <summary>
        /// Exibir todas as categorias e todos os grupos cadastrados
        /// </summary>
        /// <returns>Retorna todas as categorias e todos os grupos cadastrados</returns>
        [HttpGet("Grupos")]
        public IActionResult GetAllCategoriaGrupoComGrupos()
        {
            try
            {
                var categoriaGrupo = _categoriaGrupoRepository.GetAllCategoriaGrupoComGrupos();
                return Ok(categoriaGrupo);
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
        /// Exibir uma CategoriaGrupo e seus respectivos grupos
        /// </summary>
        /// <param name="id">Id da Categoria de Grupo</param>
        /// <returns>Retorna uma CategoriaGrupo com seus respectivos grupos</returns>
        [HttpGet("Grupos/{id}")]
        public IActionResult GetCategoriaGrupoPorIdComGrupos(int id)
        {
            try
            {
                var categoriaGrupo = _categoriaGrupoRepository.GetCategoriaGrupoPorIdComGrupos(id);
                if (categoriaGrupo is null)
                {
                    return NotFound(new { msg = "Categoria não encontrada. Verifique se o Id está correto" });
                }
                return Ok(categoriaGrupo);
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
        /// Exibir uma única categoria a partir do Id fornecido como parâmetro
        /// </summary>
        /// <param name="id">Id da Categoria de Grupo</param>
        /// <returns>Retorna uma única <b>CategoriaGrupo</b> </returns>
        [HttpGet("{id}")]
        public IActionResult GetCategoriaGrupoPorId(int id)
        {
            try
            {
                var categoriaGrupo = _categoriaGrupoRepository.GetCategoriaGrupoPorId(id);
                if (categoriaGrupo is null)
                {
                    return NotFound(new { msg = "Categoria não encontrada. Verifique se o Id está correto" });
                }
                return Ok(categoriaGrupo);
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
        /// Atualizar uma categoria de acordo com o Id fornecido
        /// </summary>
        /// <param name="id">Id da categoriaa ser atualizada</param>
        /// <param name="categoriaGrupo">Dados atualizados</param>
        /// <returns>Retorna se a categoria foi alterada ou não foi alterada</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateCategoriaGrupo(int id, CategoriaGrupo categoriaGrupo)
        {
            try
            {
                var categoriaGrupoAtualizada = _categoriaGrupoRepository.UpdateCategoriaGrupo(id, categoriaGrupo);
                if (!categoriaGrupoAtualizada)
                {
                    return NotFound(new { msg = "Categoria de Grupo não encontrada. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Categoria de Grupo atualizada com sucesso.", categoriaGrupo });
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
        /// Excluir uma categoria de Grupo no banco de dados
        /// </summary>
        /// <param name="id">Id da categoria de Grupo</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoriaGrupo(int id)
        {
            try
            {
                var categoriaDeletada = _categoriaGrupoRepository.DeleteCategoriaGrupo(id);
                if (!categoriaDeletada)
                {
                    return NotFound(new { msg = "Categoria de Grupo não encontrada. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Categoria de Grupo excluída com sucesso." });
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
                    msg = "Falha ao excluir a categoria de grupos",
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
