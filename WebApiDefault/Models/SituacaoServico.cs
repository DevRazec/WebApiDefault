using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("SituacaoServico")]
    public class SituacaoServico
    {
        [Key]
        public int IdSituacaoServico { get; set; }
        public string NomeSituacaoServico { get; set; }
        public bool Ativo { get; set; }
    }
}