using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("SituacaoAmostra")]
    public class SituacaoAmostra
    {
        [Key]
        public int IdSituacaoAmostra { get; set; }
        public string NomeSituacaoAmostra { get; set; }
        public bool Ativo { get; set; }
    }
}