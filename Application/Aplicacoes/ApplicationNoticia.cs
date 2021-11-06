using Application.Interfaces;
using Domain.Interfaces;
using Domain.Interfaces.ServiceInterfaces;
using Entidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Aplicacoes
{
    public class ApplicationNoticia : IApplicationNoticia
    {
        INoticia _INoticia;
        INoticiaService _INoticiaService;
        public ApplicationNoticia(INoticia Inoticia, INoticiaService INoticiaService)
        {
            _INoticia = Inoticia;
            _INoticiaService = INoticiaService;
        }

        public async Task AdicionaNoticia(Noticia noticia)
        {
            await _INoticiaService.AdicionaNoticia(noticia);
        }

        public async Task AtualizaNoticia(Noticia noticia)
        {
            await _INoticiaService.AtualizaNoticia(noticia);
        }

        public async Task<List<Noticia>> ListarNoticiasAtivas()
        {
            return await _INoticiaService.ListarNoticiasAtivas();
        }

        public async Task<List<Noticia>> ListarNoticiasInativas()
        {
            return await _INoticiaService.ListarNoticiasInativas();
        }

        public async Task Add(Noticia Object)
        {
            await _INoticia.Add(Object);
        }

        public async Task Update(Noticia Object)
        {
            await _INoticia.Update(Object);
        }

        public async Task Delete(Noticia Object)
        {
            await _INoticia.Delete(Object);
        }

        public async Task<Noticia> GetById(int Id)
        {
            return await _INoticia.GetById(Id);
        }

        public async Task<List<Noticia>> List()
        {
            return await _INoticia.List();
        }
    }
}
