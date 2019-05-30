using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class DBConnectionNeo2 : DbContext
    {
        public DBConnectionNeo2() : base("name=DBStringNeo2")
        {
        }

        public virtual DbSet<UsuarioNeo2> UsuariosNeo2 { get; set; }
        public virtual DbSet<Laboratorio> Laboratorio { get; set; }
        public virtual DbSet<SituacaoAmostra> SituacaoAmostra { get; set; }
        public virtual DbSet<SituacaoOrcamento> SituacaoOrcamento { get; set; }        
        public virtual DbSet<SituacaoServico> SituacaoServico { get; set; }
        public virtual DbSet<TipoOrcamento> TipoOrcamento { get; set; }     
    }
}