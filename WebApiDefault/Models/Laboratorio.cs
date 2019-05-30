using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("Laboratorio")]
    public class Laboratorio
    {
        [Key]
        public int IdLaboratorio { get; set; }
        public string NomeLaboratorio { get; set; }
        public bool Ativo { get; set; }
    }
}