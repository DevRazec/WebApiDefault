using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("SituacaoOrcamento")]
    public class SituacaoOrcamento
    {
        [Key]
        public int IdSituacaoOrcamento { get; set; }
        public string NomeSituacaoOrcamento { get; set; }
        public bool Ativo { get; set; }
    }
}