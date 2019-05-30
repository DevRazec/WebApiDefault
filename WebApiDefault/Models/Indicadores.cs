using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class Indicadores
    {
        public class JsonIndicadores
        {            
            public List<object> Servico;
            public List<object> Amostra;
            public List<object> Orcamento;
            public List<object> Laboratorio;
            public List<object> Periodo;
            public List<object> SituacaoAmostra;
            public List<object> SituacaoOrcamento;
            public List<object> TipoOrcamento;                   
        }
    }
}