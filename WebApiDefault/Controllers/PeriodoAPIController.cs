using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiDefault.Models;

namespace WebApiDefault.Controllers
{
    public class PeriodoAPIController : ApiController
    {       
        private DBConnectionVirtual db = new DBConnectionVirtual();

        public IEnumerable<Periodo> GetPeriodo()
        {
            return db.periodo.ToList();
        }

        public Periodo GetPeriodo(int id)
        {
            return db.periodo.FirstOrDefault(x => x.IdPeriodo == id);
        }
    }
}
