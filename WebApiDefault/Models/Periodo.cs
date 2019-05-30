using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class Periodo
    {
        [Key]
        public int IdPeriodo { get; set; }
        public string NomePeriodo { get; set; }       
    }    
}