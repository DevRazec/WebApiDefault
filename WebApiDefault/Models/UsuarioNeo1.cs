using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    [Table("Colaboradores")]
    public class UsuarioNeo1
    {
        [Key]
        public int id_colaborador { get; set; }
        public string login { get; set; }
        public string senha { get; set; }
        public string colaborador { get; set; }
        public string email { get; set; }
    }
}