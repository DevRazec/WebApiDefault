using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class DBConnectionVirtual
    {
        public List<Periodo> periodo = new List<Periodo>()
        {
            new Periodo() { IdPeriodo = 1, NomePeriodo = "de 1 a 10 dias" },
            new Periodo() { IdPeriodo = 2, NomePeriodo = "de 11 a 20 dias" },
            new Periodo() { IdPeriodo = 3, NomePeriodo = "de 21 a 30 dias" },
            new Periodo() { IdPeriodo = 4, NomePeriodo = "acima de 30 dias" },
            new Periodo() { IdPeriodo = 5, NomePeriodo = "Todos" },
        };
    }
}