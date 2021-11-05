using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationUsuario
    {
        Task<bool> AdicionaUsuario(string email, string senha, int idade, string celular);
    }
}
