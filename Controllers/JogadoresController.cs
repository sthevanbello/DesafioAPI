﻿using ForumGames.Interfaces;
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
        public IJogadorRepository JogadorRepository { get; set; } = new JogadorRepository();   
        /*        
        public ICollection<Jogador> GetJogadores();
        public Jogador GetJogadorPorId(int id);
        public Jogador GetJogadorPorIdComGrupos(int id);
        public ICollection<Jogador> GetJogadoresComGrupos();
        public Jogador InsertJogador(Jogador jogador);
        public Jogador InsertJogadorComImagem(Jogador jogador);
        public bool UpdateJogador(int id, Jogador jogador);
        public bool DeleteJogador(int id);
        */
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
                var listaJogadores = JogadorRepository.GetJogadores();
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
    }
}
