using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class MesAnoAtual
    {
        public class JsonMesAnoAtual
        {
            public string AnoAtual { get; set; }
            public string Mes { get; set; }
            public List<Ano> Anos { get; set; }

            public JsonMesAnoAtual(string mes, string AnoCorrente, List<Ano> ListaAno)
            {
                AnoAtual = AnoCorrente;
                Mes = mes;
                Anos = ListaAno;
            }
        }

        public class Ano
        {
            public string ano { get; set; }

            public Ano(string ListAno) { ano = ListAno; }
        }
    }
}