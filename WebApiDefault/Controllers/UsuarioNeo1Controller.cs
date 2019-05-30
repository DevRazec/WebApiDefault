using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiDefault.Models;

namespace WebApiDefault.Controllers
{
    //[Authorize]
    public class UsuarioNeo1Controller : ApiController
    {
        private DBConnectionNeo1 db = new DBConnectionNeo1();

        // GET: api/UsuarioNeo1         
        public IQueryable<UsuarioNeo1> GetUsuariosNeo1()
        {
            return db.UsuariosNeo1;
        }

        // GET: api/UsuarioNeo1/5       
        [ResponseType(typeof(UsuarioNeo1))]
        public IHttpActionResult GetUsuarioNeo1(int id)
        {
            UsuarioNeo1 usuario = db.UsuariosNeo1.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.UsuariosNeo1.Count(e => e.id_colaborador == id) > 0;
        }
    }
}
