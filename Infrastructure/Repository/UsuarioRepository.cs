using Domain.Interfaces;
using Entidades.Entities;
using Infrastructure.Configurations;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    class UsuarioRepository : GenericRepository<ApplicationUser>, IUsuario
    {
        private readonly DbContextOptions<Context> _optionsBuilder;
        public UsuarioRepository()
        {
            _optionsBuilder = new DbContextOptions<Context>();
        }
        public async Task<bool> AdicionaUsuario(string email, string senha, int idade, string celular)
        {
            try
            {
                using (var data = new Context(_optionsBuilder))
                {
                    await data.ApplicationUser.AddAsync(
                        new ApplicationUser 
                        { 
                            Email = email,
                            PasswordHash = senha,
                            Idade = idade,
                            Celular = celular
                        });

                    await data.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ExisteUsuario(string email, string senha)
        {
            try
            {
                using (var data = new Context(_optionsBuilder))
                {
                    return await data.ApplicationUser
                        .Where(u => u.Email.Equals(email) && u.PasswordHash.Equals(senha))
                        .AsNoTracking()
                        .AnyAsync();                                        
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
