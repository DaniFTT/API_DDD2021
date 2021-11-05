using Domain.Interfaces.Generics;
using Entidades.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INoticia : IGenerics<Noticia>
    {
        Task<List<Noticia>> ListarNoticias(Expression<Func<Noticia, bool>> exNoticia);
    }
}
