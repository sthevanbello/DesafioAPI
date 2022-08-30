using ForumGames.Interfaces;
using ForumGames.Models;
using ForumGames.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using ForumGames.Repositories;

namespace ForumGames.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaPostagensController : ControllerBase
    {
        private readonly ICategoriaPostagemRepository _categoriaPostagemRepository;

        public CategoriaPostagensController(ICategoriaPostagemRepository categoriaPostagemRepository)
        {
            _categoriaPostagemRepository = categoriaPostagemRepository;
        }


        /// <summary>
        /// Inserir uma Categoria de postagem nova no banco de dados
        /// </summary>
        /// <param name="categoriaPostagem">Categoria de postagem nova</param>
        /// <returns>Retorna a categoria de grupo nova</returns>
        [HttpPost]
        public IActionResult InsertCategoriaPostagem(CategoriaPostagem categoriaPostagem)
        {
            try
            {
                _categoriaPostagemRepository.InsertCategoriaPostagem(categoriaPostagem);
                return Ok(new { msg = "Categoria de postagem criada com sucesso", categoriaPostagem });
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
        /// Exibir todas as categorias de postagens cadastradas
        /// </summary>
        /// <returns>Retorna todas as categorias de postagens cadastradas</returns>
        // Get
        [HttpGet]
        public IActionResult GetAllCategoriaPostagem()
        {
            try
            {
                var listaCategorias = _categoriaPostagemRepository.GetAllCategoriaPostagem();
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
        /// Exibir todas as categorias e todos as postagens referentes à categoria
        /// </summary>
        /// <returns>Retorna todas as categorias e todos as postagens referentes à categoria</returns>
        [HttpGet("Postagens")]
        public IActionResult GetAllCategoriaPostagemComPostagens()
        {
            try
            {
                var categoriaGrupo = _categoriaPostagemRepository.GetAllCategoriaPostagemComPostagens();
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
        /// Exibir uma CategoriaPostagem e suas respectivas postagens
        /// </summary>
        /// <param name="id">Id da Categoria de Postagem</param>
        /// <returns>Retorna uma CategoriaPostagem com suas respectivas postagens</returns>
        [HttpGet("Postagens/{id}")]
        public IActionResult GetCategoriaPostagemPorIdComPostagens(int id)
        {
            try
            {
                var categoriaPostagem = _categoriaPostagemRepository.GetCategoriaPostagemPorIdComPostagens(id);
                if (categoriaPostagem is null)
                {
                    return NotFound(new { msg = "Categoria não encontrada. Verifique se o Id está correto" });
                }
                return Ok(categoriaPostagem);
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
        /// <param name="id">Id da Categoria de Postagem</param>
        /// <returns>Retorna uma única <b>CategoriaPostagem</b> </returns>
        [HttpGet("{id}")]
        public IActionResult GetCategoriaPostagemPorId(int id)
        {
            try
            {
                var categoriaPostagem = _categoriaPostagemRepository.GetCategoriaPostagemPorId(id);
                if (categoriaPostagem is null)
                {
                    return NotFound(new { msg = "Categoria não encontrada. Verifique se o Id está correto" });
                }
                return Ok(categoriaPostagem);
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
        /// <param name="categoriaPostagem">Dados atualizados</param>
        /// <returns>Retorna se a categoria foi alterada ou não foi alterada</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateCategoriaPostagem(int id, CategoriaPostagem categoriaPostagem)
        {
            try
            {
                var categoriaPostagemAtualizada = _categoriaPostagemRepository.UpdateCategoriaPostagem(id, categoriaPostagem);
                if (!categoriaPostagemAtualizada)
                {
                    return NotFound(new { msg = "Categoria de postagem não encontrada. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Categoria de postagem atualizado com sucesso.", categoriaPostagem });
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
        /// Excluir uma categoria de Postagem no banco de dados
        /// </summary>
        /// <param name="id">Id da categoria de Postagem</param>
        /// <returns>Retorna uma mensagem sobre a operação de exclusão a ser realizada</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoriaGrupo(int id)
        {
            try
            {
                var categoriaDeletada = _categoriaPostagemRepository.DeleteCategoriaPostagem(id);
                if (!categoriaDeletada)
                {
                    return NotFound(new { msg = "Categoria de Postagem não encontrada. Verifique se o Id está correto" });
                }
                return Ok(new { msg = "Categoria de Postagem excluída com sucesso." });
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
                    msg = "Falha ao excluir a categoria de postagem",
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
