using Domain.Interfaces;
using Domain.Interfaces.ServiceInterfaces;
using Entidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class NoticiaServices : INoticiaService
    {
        private readonly INoticia _INoticia;

        public NoticiaServices(INoticia iNoticia)
        {
            _INoticia = iNoticia;
        }

        public async Task AdicionaNoticia(Noticia noticia)
        {
            var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
            var validarInformacoes = noticia.ValidarPropriedadeString(noticia.Informacao, "Informacao");

            if(validarTitulo && validarInformacoes)
            {
                noticia.DataAlteracao = DateTime.Now;
                noticia.DataCadastro = DateTime.Now;
                noticia.Ativo = true;

                await _INoticia.Add(noticia);
            }
        }

        public async Task AtualizaNoticia(Noticia noticia)
        {
            var validarTitulo = noticia.ValidarPropriedadeString(noticia.Titulo, "Titulo");
            var validarInformacoes = noticia.ValidarPropriedadeString(noticia.Informacao, "Informacao");

            if (validarTitulo && validarInformacoes)
            {
                noticia.DataAlteracao = DateTime.Now;
                noticia.Ativo = true;

                await _INoticia.Update(noticia);
            }
        }

        public async Task<List<Noticia>> ListarNoticiasAtivas()
        {
            return await _INoticia.ListarNoticias(n => n.Ativo);
        }
        public async Task<List<Noticia>> ListarNoticiasInativas()
        {
            return await _INoticia.ListarNoticias(n => !n.Ativo);
        }
    }
}
