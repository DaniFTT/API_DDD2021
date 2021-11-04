using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Notifications
{
    public class Notifica
    {
        [NotMapped]
        public string NomePropriedade { get; set; }
        [NotMapped]
        public string Mensagem { get; set; }

        [NotMapped]
        public List<Notifica> Notificacoes;

        public Notifica()
        {
            Notificacoes = new List<Notifica>();
        }

        public bool ValidarPropriedadeString(string valor, string nomeDaPropriedade)
        {
            if(string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomeDaPropriedade))
            {
                Notificacoes.Add(new Notifica
                {
                    Mensagem = "Campo Obrigatório",
                    NomePropriedade = nomeDaPropriedade
                });

                return false;
            }

            return true;
        }


        public bool ValidarPropriedadeDecimal(decimal valor, string nomeDaPropriedade)
        {
            if (valor < 1 || string.IsNullOrWhiteSpace(nomeDaPropriedade))
            {
                Notificacoes.Add(new Notifica
                {
                    Mensagem = "Valor deve ser maior que 0",
                    NomePropriedade = nomeDaPropriedade
                });

                return false;
            }

            return true;
        }
    }
}
