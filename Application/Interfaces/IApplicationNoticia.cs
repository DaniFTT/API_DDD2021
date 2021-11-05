using Application.Interfaces.Generics;
using Entidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationNoticia : IApplicationGeneric<Noticia>
    {
        Task AdicionaNoticia(Noticia noticia);
        Task AtualizaNoticia(Noticia noticia);
        Task<List<Noticia>> ListarNoticiasAtivas();
        Task<List<Noticia>> ListarNoticiasInativas();
    }
}
