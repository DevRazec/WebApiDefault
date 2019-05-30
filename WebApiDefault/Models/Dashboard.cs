using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDefault.Models
{
    public class Dashboard
    {
        public class JsonDashboard
        {
            public string Avulso { get; set; }
            public string Contrato { get; set; }
            public string Aprovado { get; set; }
            public string Declinado { get; set; }
            public string Cancelado { get; set; }
            public List<QuantidadeOrcAno> QuantidadeOrcAvulsoAno { get; set; }
            public List<QuantidadeOrcMes> QuantidadeOrcAvulsoMes { get; set; }
            public List<QuantidadeOrcAno> QuantidadeOrcContratoAno { get; set; }
            public List<QuantidadeOrcMes> QuantidadeOrcContratoMes { get; set; }
            public List<QuantidadeSitOrcAno> QuantidadeApAno { get; set; }
            public List<QuantidadeSitOrcMes> QuantidadeApMes { get; set; }
            public List<QuantidadeSitOrcAno> QuantidadeDecAno { get; set; }
            public List<QuantidadeSitOrcMes> QuantidadeDecMes { get; set; }
            public List<QuantidadeSitOrcAno> QuantidadeCanAno { get; set; }
            public List<QuantidadeSitOrcMes> QuantidadeCanMes { get; set; }
            public List<QuantidadeSitAtual> QuantidadeSitAtual { get; set; }
            public List<TempoMedioOrcamento> TempoMedioOrcamento { get; set; }
            public List<TempoOrcamento> TempoOrcamento { get; set; }
            public List<AnaliseCritica> AnaliseCritica { get; set; }
            public List<ValorOrcamento> ConsultaValorOrcamento { get; set; }
            public List<Top15> ConsultaTop15 { get; set; }
            public List<ValorOrcamentoMesAno> ConsultaValorOrcamentoMesAno { get; set; }

            public JsonDashboard(string QtdAvulso, string QtdContrato, string QtdAprovado, string QtdDeclinado, string QtdCancelado, List<QuantidadeOrcAno> QtdOrcAvulsoAno, List<QuantidadeOrcMes> QtdOrcAvulsoMes,
               List<QuantidadeOrcAno> QtdOrcContratoAno, List<QuantidadeOrcMes> QtdOrcContratoMes, List<QuantidadeSitOrcAno> QtdApAno, List<QuantidadeSitOrcMes> QtdApMes, List<QuantidadeSitOrcAno> QtdDecAno,
               List<QuantidadeSitOrcMes> QtdDecMes, List<QuantidadeSitOrcAno> QtdCanAno, List<QuantidadeSitOrcMes> QtdCanMes, List<QuantidadeSitAtual> QtdSitAtual, List<TempoMedioOrcamento> TempMedOrc, List<TempoOrcamento> TempOrc, List<AnaliseCritica> AnaCritica,
               List<ValorOrcamento> ConsultaValorOrc, List<Top15> Top15, List<ValorOrcamentoMesAno> ValorOrcamentoMesAno)
            {
                Avulso = QtdAvulso;
                Contrato = QtdContrato;
                Aprovado = QtdAprovado;
                Declinado = QtdDeclinado;
                Cancelado = QtdCancelado;
                QuantidadeOrcAvulsoAno = QtdOrcAvulsoAno;
                QuantidadeOrcAvulsoMes = QtdOrcAvulsoMes;
                QuantidadeOrcContratoAno = QtdOrcContratoAno;
                QuantidadeOrcContratoMes = QtdOrcContratoMes;
                QuantidadeApAno = QtdApAno;
                QuantidadeApMes = QtdApMes;
                QuantidadeDecAno = QtdDecAno;
                QuantidadeDecMes = QtdDecMes;
                QuantidadeCanAno = QtdCanAno;
                QuantidadeCanMes = QtdCanMes;
                QuantidadeSitAtual = QtdSitAtual;
                TempoMedioOrcamento = TempMedOrc;
                TempoOrcamento = TempOrc;
                AnaliseCritica = AnaCritica;
                ConsultaValorOrcamento = ConsultaValorOrc;
                ConsultaTop15 = Top15;
                ConsultaValorOrcamentoMesAno = ValorOrcamentoMesAno;
            }
        }

        public class ValorOrcamentoMesAno
        {
            public List<ListaValorOrcamentoMesAno> Contrato { get; set; }
            public List<ListaValorOrcamentoMesAno> Avulso { get; set; }
        }

        public class ListaValorOrcamentoMesAno
        {
            public string Ano { get; set; }
            public List<DadosValorOrcamentoMesAno> Dados { get; set; }
        }

        public class DadosValorOrcamentoMesAno
        {
            public string Mes { get; set; }
            public string Valor { get; set; }

            //public DadosValorOrcamentoMesAno(string mes, string valor)
            //{
            //    Mes = mes;
            //    Valor = valor;
            //}

        }

        public class ValorOrcamento
        {
            public string ValorAvulso { get; set; }
            public string ValorContrato { get; set; }
            public string ValorPendente { get; set; }
        }

        public class AnaliseCritica
        {
            public string Laboratorio { get; set; }
            public string Quantidade { get; set; }

            public AnaliseCritica(string lab, string Qtd)
            {
                Laboratorio = lab;
                Quantidade = Qtd;
            }
        }

        public class QuantidadeOrcAno
        {
            public string Quantidade { get; set; }
            public string Ano { get; set; }

            public QuantidadeOrcAno(string anos, string Quantidades)
            {
                Ano = anos;
                Quantidade = Quantidades;
            }
        }

        public class QuantidadeOrcMes
        {
            public string Ano { get; set; }
            public List<QuantidadeOrcMeses> Mes { get; set; }

            public QuantidadeOrcMes(string Anos, List<QuantidadeOrcMeses> Meses)
            {
                Ano = Anos;
                Mes = Meses;
            }
        }

        public class QuantidadeOrcMeses
        {
            public string Mes { get; set; }
            public string Quantidade { get; set; }

            public QuantidadeOrcMeses(string meses, string Quantidades)
            {
                Mes = meses;
                Quantidade = Quantidades;
            }
        }

        public class QuantidadeSitOrcAno
        {
            public string Quantidade { get; set; }
            public string Ano { get; set; }

            public QuantidadeSitOrcAno(string anos, string Quantidades)
            {
                Ano = anos;
                Quantidade = Quantidades;
            }
        }

        public class QuantidadeSitOrcMes
        {
            public string Ano { get; set; }
            public List<QuantidadeSitOrcMeses> Mes { get; set; }

            public QuantidadeSitOrcMes(string Anos, List<QuantidadeSitOrcMeses> Meses)
            {
                Ano = Anos;
                Mes = Meses;
            }
        }

        public class QuantidadeSitOrcMeses
        {
            public string Mes { get; set; }
            public string Quantidade { get; set; }
            public QuantidadeSitOrcMeses(string meses, string Quantidades)
            {
                Mes = meses;
                Quantidade = Quantidades;
            }
        }

        public class QuantidadeSitAtual
        {
            public string Situacao { get; set; }
            public string Quantidade { get; set; }

            public QuantidadeSitAtual(string Situacoes, string Quantidades)
            {
                Situacao = Situacoes;
                Quantidade = Quantidades;
            }
        }

        public class TempoMedioOrcamento
        {
            public string Medio { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }

            public TempoMedioOrcamento(string medio, string min, string max)
            {
                Medio = medio;
                Min = min;
                Max = max;
            }
        }

        public class TempoOrcamento
        {
            public List<QtdTempoOrcamento> Quantidade { get; set; }           
        }

        public class QtdTempoOrcamento
        {
            public string Quantidade { get; set; }
            public QtdTempoOrcamento(string Quantidades)
            {
                Quantidade = Quantidades;
            }
        }

        public class Top15
        {
            public List<Top15Mes> Top15Mes { get; set; }
            public List<Top15Ano> Top15Ano { get; set; }
            public Top15(List<Top15Mes> TopMes, List<Top15Ano> TopAno)
            {
                Top15Mes = TopMes;
                Top15Ano = TopAno;
            }

        }

        public class Top15Mes
        {
            public string Valor { get; set; }
            public string Empresa { get; set; }

            public Top15Mes(string valor, string empresa)
            {
                Valor = valor;
                Empresa = empresa;
            }
        }

        public class Top15Ano
        {
            public string Valor { get; set; }
            public string Empresa { get; set; }

            public Top15Ano(string valor, string empresa)
            {
                Valor = valor;
                Empresa = empresa;
            }

        }
    }
}