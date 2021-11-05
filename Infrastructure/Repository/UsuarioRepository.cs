using Domain.Interfaces;
using Entidades.Entities;
using Infrastructure.Configurations;
using Infrastructure.Repository.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    class UsuarioRepository : GenericRepository<ApplicationUser>, IUsuario
    {
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
    }
}
