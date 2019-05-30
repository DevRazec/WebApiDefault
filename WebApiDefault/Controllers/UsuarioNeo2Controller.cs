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
    [Authorize]
    public class UsuarioNeo2Controller : ApiController
    {
        private DBConnectionNeo2 db = new DBConnectionNeo2();

        // GET: api/UsuariosNeo2       
        public IQueryable<UsuarioNeo2> GetUsuariosNeo2()
        {
            return db.UsuariosNeo2;
        }

        // GET: api/UsuarioNeo2/5      
        [ResponseType(typeof(UsuarioNeo2))]
        public IHttpActionResult GetUsuarioNeo1(int id)
        {
            UsuarioNeo2 usuario = db.UsuariosNeo2.Find(id);
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
            return db.UsuariosNeo2.Count(e => e.IdUsuario == id) > 0;
        }
    }
}
