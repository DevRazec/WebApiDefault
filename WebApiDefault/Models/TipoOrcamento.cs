using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("TipoOrcamento")]
    public class TipoOrcamento
    {
        [Key]
        public int IdTipoOrcamento { get; set; }
        public string NomeTipoOrcamento { get; set; }
        public bool Ativo { get; set; }       
    }
}