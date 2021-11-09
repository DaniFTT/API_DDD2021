using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Noticia
{
    public class NoticiaModel
    {
        public int NoticiaID { get; set; }
        public string Titulo { get; set; }
        public string Informacao { get; set; }
        public bool Ativo { get; set; }
        public string UsuarioId { get; set; }
    }
}
