using Domain.Interfaces;
using Entidades.Entities;
using Infrastructure.Configurations;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class NoticiaRepository : GenericRepository<Noticia>, INoticia
    {
        //protected readonly DbContextOptions<Context> _optionsBuilder;
        //public NoticiaRepository()
        //{
        //    _optionsBuilder = new DbContextOptions<DbContext>();
        //}

        public async Task<List<Noticia>> ListarNoticias(Expression<Func<Noticia, bool>> exNoticia)
        {
            using (var data = new Context(_optionsBuilder))
            {
                return await data.Noticia.Where(exNoticia).AsNoTracking().ToListAsync();
            }
        }
    }
}
