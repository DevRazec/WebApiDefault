using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class DBConnectionNeo1 : DbContext
    {
        public DBConnectionNeo1() : base("name=DBStringNeo1")
        {
        }

        public virtual DbSet<UsuarioNeo1> UsuariosNeo1 { get; set; }
        public virtual DbSet<SituacaoRequsicao> SituacaoRequsicao { get; set; }
        public virtual DbSet<CentroCustos> CentroCustos { get; set; }      
    }
}