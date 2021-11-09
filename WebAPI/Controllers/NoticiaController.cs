using Application.Interfaces;
using Entidades.Entities;
using Entidades.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.Noticia;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly IApplicationNoticia _applicationNoticia;

        public NoticiaController(IApplicationNoticia applicationNoticia)
        {
            _applicationNoticia = applicationNoticia;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarNoticia")]
        public async Task<List<Notifica>> AdicionarNoticia([FromBody]NoticiaModel noticiaModel)
        {
            var novaNoticia = new Noticia();
            novaNoticia.Titulo = noticiaModel.Titulo;
            novaNoticia.Informacao = noticiaModel.Informacao;
            novaNoticia.UserId = await RetornaIdUsuarioLogado();

            await _applicationNoticia.AdicionaNoticia(novaNoticia);

            return novaNoticia.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AtualizaNoticia")]
        public async Task<List<Notifica>> AtualizaNoticia([FromBody]NoticiaModel noticiaModel)
        {
            var noticiaAtual = await _applicationNoticia.GetById(noticiaModel.NoticiaID);
            noticiaAtual.Titulo = noticiaModel.Titulo;
            noticiaAtual.Informacao = noticiaModel.Informacao;
            noticiaAtual.Ativo = noticiaModel.Ativo;
            noticiaAtual.UserId = await RetornaIdUsuarioLogado();

            await _applicationNoticia.AtualizaNoticia(noticiaAtual);

            return noticiaAtual.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/BuscarPorId")]
        public async Task<Noticia> BuscarPorId([FromQuery]int Id)
        {
            var noticiaAtual = await _applicationNoticia.GetById(Id);

            return noticiaAtual;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ExcluirNoticia")]
        public async Task<List<Notifica>> ExcluirNoticia([FromQuery]int Id)
        {
            var noticiaAtual = await _applicationNoticia.GetById(Id);

            await _applicationNoticia.Delete(noticiaAtual);

            return noticiaAtual.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarTodasNoticias")]
        public async Task<List<Noticia>> ListarTodasNoticias()
        {
            return await _applicationNoticia.List();
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarNoticiasAtivas")]
        public async Task<List<Noticia>> ListarNoticiasAtivas()
        {
            return await _applicationNoticia.ListarNoticiasAtivas();
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarNoticiasInativas")]
        public async Task<List<Noticia>> ListarNoticiasInativas()
        {
            return await _applicationNoticia.ListarNoticiasInativas();
        }

        private async Task<string> RetornaIdUsuarioLogado()
        {
            if (User != null)
            {
                var userId = User.FindFirst("userId");
                return userId.Value;
            }

            return string.Empty;
        }
    }
}
