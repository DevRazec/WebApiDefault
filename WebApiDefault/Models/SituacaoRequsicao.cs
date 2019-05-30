using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("SituacaoRequisicao")]
    public class SituacaoRequsicao
    {
        [Key]
        public int IdSituacaoReq { get; set; }
        public string Situacao { get; set; }
        public bool Ativo { get; set; }
    }
}