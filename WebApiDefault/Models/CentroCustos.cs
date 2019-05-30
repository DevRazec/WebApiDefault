using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("CentroCusto")]
    public class CentroCustos
    {
        [Key]
        public int IdCentroCusto { get; set; }
        public string CentroCusto { get; set; }
        public bool Ativo { get; set; }
    }
}