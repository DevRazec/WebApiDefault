using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiDefault.Models;

namespace WebApiDefault.Controllers
{
    public class PeriodoMVCController : Controller
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

        // GET: PeriodoMVC
        public ActionResult Index()
        {
            return View(GetPeriodo());
        }        
    }
}