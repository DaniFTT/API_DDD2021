using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public int Idade { get; set; }
        public string Celular { get; set; }
    }
}
