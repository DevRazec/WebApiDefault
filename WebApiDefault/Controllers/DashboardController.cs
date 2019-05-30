using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Helpers;
using System.Web.Http;
using WebApiDefault.Models;
using static WebApiDefault.Models.Dashboard;

namespace WebApiDefault.Controllers
{
    [Authorize]
    public class DashboardController : ApiController
    {        
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConNeo2"].ConnectionString);

        string ano = "2018";
        string mes = "10";     

        public IQueryable<JsonDashboard> GetJsonDashboard()
        {
            List<JsonDashboard> JsonD = new List<JsonDashboard>();
            JsonD.Add(new JsonDashboard(ConsultaOrcamentoTipo(mes, ano, "1"), ConsultaOrcamentoTipo(mes, ano, "2"), ConsultaQtdOrcamentoSit(mes, ano, "4"), ConsultaQtdOrcamentoSit(mes, ano, "7"), ConsultaQtdOrcamentoSit(mes, ano, "6"),
                    ConsultaQtdOrcamentoAno("1"), ConsultaQtdOrcMes("1"), ConsultaQtdOrcamentoAno("2"), ConsultaQtdOrcMes("2"), ConsultaQtdOrcamentoSitAno("4"), ConsultaQtdOrcamentoSitMes("4"), ConsultaQtdOrcamentoSitAno("7"),
                    ConsultaQtdOrcamentoSitMes("7"), ConsultaQtdOrcamentoSitAno("6"), ConsultaQtdOrcamentoSitMes("6"), ConsultaQtaOrcamentoSitAtual(), ConsultaTempoMedioOrcamento(ano), ConsultaTempoOrcamento(ano), ConsultaAnaliseCritica(), ConsultaValorOrcamento(mes, ano),
                    ConsultaTop15(mes, ano), ConsultaValorOrcamentoMesAno()));

            return JsonD.AsQueryable();
        }

        private string ConsultaOrcamentoTipo(string mes, string ano, string tipo)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT count(orc.IdOrcamento) AS orcamentoavulso FROM Orcamento orc where  month(orc.DataCadastro) = @mes and year(orc.DataCadastro) = @ano and IdSituacaoOrcamento != 5 and orc.IdTipoOrcamento= @Tipo";
            SQL.Parameters.AddWithValue("@mes", mes);
            SQL.Parameters.AddWithValue("@ano", ano);
            SQL.Parameters.AddWithValue("@Tipo", tipo);
            con.Open();
            string Resultado = SQL.ExecuteScalar().ToString();
            con.Close();

            return Resultado;
        }

        private List<QuantidadeOrcAno> ConsultaQtdOrcamentoAno(string tipo)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"select count(orc.IdOrcamento) as Quantidade , year(orc.DataCadastro) as ano from Orcamento orc where orc.IdTipoOrcamento = @Tipo and orc.IdSituacaoOrcamento != 5 group by year(orc.DataCadastro) order by ano asc";
            SQL.Parameters.AddWithValue("@Tipo", tipo);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            List<QuantidadeOrcAno> Quantidade = new List<QuantidadeOrcAno>();
            while (reader.Read()) { Quantidade.Add(new QuantidadeOrcAno(reader["ano"].ToString(), reader["Quantidade"].ToString())); }
            con.Close();

            return Quantidade;
        }
        private List<QuantidadeOrcMes> ConsultaQtdOrcMes(string tipo)
        {
            string ano = string.Empty;

            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"select count(orc.IdOrcamento) as Quantidade, month(orc.DataCadastro) as Mes, year(orc.DataCadastro) as ano from Orcamento orc where orc.IdTipoOrcamento = @Tipo
                                and orc.IdSituacaoOrcamento != 5 group by month(orc.DataCadastro), year(orc.DataCadastro) order by ano, mes";
            SQL.Parameters.AddWithValue("@Tipo", tipo);
            con.Open();

            SqlDataReader reader = SQL.ExecuteReader();
            List<QuantidadeOrcMes> Quantidade = new List<QuantidadeOrcMes>();
            List<QuantidadeOrcMeses> Meses = new List<QuantidadeOrcMeses>();

            while (reader.Read())
            {
                ano = reader["ano"].ToString();
                Meses.Add(new QuantidadeOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                while (reader.Read())
                {
                    if (ano == reader["ano"].ToString()) Meses.Add(new QuantidadeOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                    else
                    {
                        Quantidade.Add(new QuantidadeOrcMes(ano, Meses));
                        ano = reader["ano"].ToString();
                        Meses = new List<QuantidadeOrcMeses>();
                        Meses.Add(new QuantidadeOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                    }
                }
            }

            Quantidade.Add(new QuantidadeOrcMes(ano, Meses));
            reader.Close();
            con.Close();

            return Quantidade;
        }
        private string ConsultaQtdOrcamentoSit(string mes, string ano, string Sit)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT count(lsit.IdLogSituacao) from LogSituacao lsit where month(lsit.DataCadastro) = @mes and year(lsit.DataCadastro) = @ano and lsit.IdSituacao = @Sit and lsit.IdOrcamento is not null";
            SQL.Parameters.AddWithValue("@mes", mes);
            SQL.Parameters.AddWithValue("@ano", ano);
            SQL.Parameters.AddWithValue("@Sit", Sit);
            con.Open();
            string Resultado = SQL.ExecuteScalar().ToString();
            con.Close();

            return Resultado;
        }
        private List<QuantidadeSitOrcAno> ConsultaQtdOrcamentoSitAno(string Sit)
        {

            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT count(lsit.IdLogSituacao) as Quantidade, year(lsit.DataCadastro) as Ano from LogSituacao lsit where lsit.IdSituacao = @Sit and lsit.IdOrcamento 
                                is not null group by year(lsit.DataCadastro) order by Ano";
            SQL.Parameters.AddWithValue("@Sit", Sit);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            List<QuantidadeSitOrcAno> Quantidade = new List<QuantidadeSitOrcAno>();
            while (reader.Read()) { Quantidade.Add(new QuantidadeSitOrcAno(reader["Ano"].ToString(), reader["Quantidade"].ToString())); }
            con.Close();

            return Quantidade;
        }
        private List<QuantidadeSitOrcMes> ConsultaQtdOrcamentoSitMes(string Sit)
        {
            string ano = string.Empty;

            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT count(lsit.IdLogSituacao) as Quantidade, month(lsit.DataCadastro) as Mes,year(lsit.DataCadastro) as Ano from LogSituacao lsit where lsit.IdSituacao = @Sit and lsit.IdOrcamento 
                                is not null group by month(lsit.DataCadastro), year(lsit.DataCadastro) order by Ano, Mes";
            SQL.Parameters.AddWithValue("@Sit", Sit);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            List<QuantidadeSitOrcMes> Quantidade = new List<QuantidadeSitOrcMes>();
            List<QuantidadeSitOrcMeses> Meses = new List<QuantidadeSitOrcMeses>();

            while (reader.Read())
            {
                ano = reader["ano"].ToString();
                Meses.Add(new QuantidadeSitOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                while (reader.Read())
                {
                    if (ano == reader["ano"].ToString()) Meses.Add(new QuantidadeSitOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                    else
                    {
                        Quantidade.Add(new QuantidadeSitOrcMes(ano, Meses));
                        ano = reader["ano"].ToString();
                        Meses = new List<QuantidadeSitOrcMeses>();
                        Meses.Add(new QuantidadeSitOrcMeses(reader["Mes"].ToString(), reader["Quantidade"].ToString()));
                    }
                }
            }

            Quantidade.Add(new QuantidadeSitOrcMes(ano, Meses));
            reader.Close();
            con.Close();
            return Quantidade;
        }
        private List<QuantidadeSitAtual> ConsultaQtaOrcamentoSitAtual()
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT sitorc.NomeSituacaoOrcamento,count(orc.IdOrcamento) AS Quantidade FROM Orcamento orc 
                                join SituacaoOrcamento sitorc on sitorc.IdSituacaoOrcamento = orc.IdSituacaoOrcamento
                                where (orc.IdSituacaoOrcamento < 4 OR orc.IdSituacaoOrcamento = 10)   group by sitorc.IdSituacaoOrcamento, sitorc.NomeSituacaoOrcamento order by sitorc.IdSituacaoOrcamento";
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            List<QuantidadeSitAtual> Quantidade = new List<QuantidadeSitAtual>();
            while (reader.Read()) { Quantidade.Add(new QuantidadeSitAtual(reader["NomeSituacaoOrcamento"].ToString(), reader["Quantidade"].ToString())); }
            con.Close();

            return Quantidade;
        }
        private List<TempoMedioOrcamento> ConsultaTempoMedioOrcamento(string ano)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"select distinct TopOrcamento.NumOrcamento, case when(TopOrcamento.dif = 0) then '1' else TopOrcamento.dif end as Dif from Orcamento orc
                                join NumeroOrcamento norc on norc.IdNumOrcamento = orc.IdNumOrcamento
                                CROSS APPLY (select top(1) norc2.NumOrcamento, DATEDIFF ( dd , orc2.DataCadastro , lsit.DataCadastro ) as dif from Orcamento orc2
			                                join NumeroOrcamento norc2 on norc2.IdNumOrcamento = orc2.IdNumOrcamento
			                                join LogSituacao lsit on lsit.IdOrcamento = orc2.IdOrcamento
			                                 where lsit.IdSituacao = 3 and norc2.IdNumOrcamento = norc.IdNumOrcamento and lsit.IdOrcamento is not null order by lsit.DataCadastro asc) as TopOrcamento
			                                 where (orc.IdSituacaoOrcamento between 3 and 4 or orc.IdSituacaoOrcamento between 6 and 9 ) and year(orc.DataCadastro) = @Ano";
            SQL.Parameters.AddWithValue("@Ano", ano);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            var min = 1; var max = 1; var media = 0; var count = 1;
            int Qtd10 = 0; int Qtd20 = 0; int Qtd30 = 0; int Qtd60 = 0; int QtdMais60 = 0;

            while (reader.Read())
            {
                var quantidade = Convert.ToInt32(reader["Dif"].ToString());
                if (quantidade < min) min = quantidade;
                else if (quantidade > max) max = quantidade;
                media += quantidade;
                count++;

                if (quantidade <= 10) Qtd10++;
                else if (quantidade <= 20) Qtd20++;
                else if (quantidade <= 30) Qtd30++;
                else if (quantidade <= 60) Qtd60++;
                else QtdMais60++;

            }
            media = media / count;

            List<TempoMedioOrcamento> TempoMedio = new List<TempoMedioOrcamento>();
            TempoMedio.Add(new TempoMedioOrcamento(media.ToString(), min.ToString(), max.ToString()));
            reader.Close();
            con.Close();
            return TempoMedio;

        }
        private List<TempoOrcamento> ConsultaTempoOrcamento(string ano)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"select distinct TopOrcamento.NumOrcamento, case when(TopOrcamento.dif = 0) then '1' else TopOrcamento.dif end as Dif from Orcamento orc
                                join NumeroOrcamento norc on norc.IdNumOrcamento = orc.IdNumOrcamento
                                CROSS APPLY (select top(1) norc2.NumOrcamento, DATEDIFF ( dd , orc2.DataCadastro , lsit.DataCadastro ) as dif from Orcamento orc2
			                                join NumeroOrcamento norc2 on norc2.IdNumOrcamento = orc2.IdNumOrcamento
			                                join LogSituacao lsit on lsit.IdOrcamento = orc2.IdOrcamento
			                                 where lsit.IdSituacao = 3 and norc2.IdNumOrcamento = norc.IdNumOrcamento and lsit.IdOrcamento is not null order by lsit.DataCadastro asc) as TopOrcamento
			                                 where (orc.IdSituacaoOrcamento between 3 and 4 or orc.IdSituacaoOrcamento between 6 and 9 ) and year(orc.DataCadastro) = @Ano";
            SQL.Parameters.AddWithValue("@Ano", ano);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            int Qtd10 = 0; int Qtd20 = 0; int Qtd30 = 0; int Qtd60 = 0; int QtdMais60 = 0;

            while (reader.Read())
            {
                var quantidade = Convert.ToInt32(reader["Dif"].ToString());
                if (quantidade <= 10) Qtd10++;
                else if (quantidade <= 20) Qtd20++;
                else if (quantidade <= 30) Qtd30++;
                else if (quantidade <= 60) Qtd60++;
                else QtdMais60++;

            }
            List<TempoOrcamento> TempoOrcamento = new List<TempoOrcamento>();
            List<QtdTempoOrcamento> QtdTempoOrcamento = new List<QtdTempoOrcamento>();

            QtdTempoOrcamento.Add(new QtdTempoOrcamento(Qtd10.ToString()));
            QtdTempoOrcamento.Add(new QtdTempoOrcamento(Qtd20.ToString()));
            QtdTempoOrcamento.Add(new QtdTempoOrcamento(Qtd30.ToString()));
            QtdTempoOrcamento.Add(new QtdTempoOrcamento(Qtd60.ToString()));
            QtdTempoOrcamento.Add(new QtdTempoOrcamento(QtdMais60.ToString()));
            TempoOrcamento.Add(new TempoOrcamento() { Quantidade = QtdTempoOrcamento });

            reader.Close();
            con.Close();

            return TempoOrcamento;

        }
        private List<AnaliseCritica> ConsultaAnaliseCritica()
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT lab.NomeLaboratorio, count(lab.IdLaboratorio) Quantidade 
                                      FROM Multitecnica Mult
                                      join Laboratorio lab on lab.IdLaboratorio = Mult.IdLaboratorio
                                      join Orcamento orc on orc.IdOrcamento = Mult.IdOrcamento
                                      where Mult.Situacao = 0 and (orc.IdSituacaoOrcamento between 1 and 3 or orc.IdSituacaoOrcamento = 10)  group by lab.IdLaboratorio, lab.NomeLaboratorio order by  Quantidade desc ";
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();
            List<AnaliseCritica> Quantidade = new List<AnaliseCritica>();
            while (reader.Read()) { Quantidade.Add(new AnaliseCritica(reader["NomeLaboratorio"].ToString(), reader["Quantidade"].ToString())); }
            con.Close();

            return Quantidade;
        }
        private List<ValorOrcamento> ConsultaValorOrcamento(string mes, string ano)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT sum(orc.TotalOrcamento) as Valor FROM Orcamento orc
                                join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
                                 where lsit.IdSituacao = 4 and (year(lsit.DataCadastro) = @Ano and MONTH(lsit.DataCadastro)= @Mes) and orc.IdTipoOrcamento = 1;
 
                                SELECT  sum(orc.TotalOrcamento) as Valor FROM Orcamento orc
                                join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
                                 where lsit.IdSituacao = 4 and (year(lsit.DataCadastro) = @Ano and MONTH(lsit.DataCadastro)= @Mes) and orc.IdTipoOrcamento = 2;
 
                                SELECT  sum(orc.TotalOrcamento) as Valor FROM Orcamento orc where orc.IdSituacaoOrcamento = 3 ";
            SQL.Parameters.AddWithValue("@Mes", mes);
            SQL.Parameters.AddWithValue("@Ano", ano);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();

            List<ValorOrcamento> Quantidade = new List<ValorOrcamento>();
            string[] Valores = new string[3];
            int count = 0;
            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader["Valor"].ToString() == string.Empty) Valores[count] = "0";
                    else Valores[count] = reader["Valor"].ToString();
                    count++;
                }
                reader.NextResult();
            }


            // obtém a cultura local
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            // faz uma cópia das informações de formatação de número da cultura local
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            // fixa o símbolo da moeda estrangeira
            numberFormatInfo.CurrencySymbol = "R$";
            // obtém o valor em moeda estrangeira formatado conforme a cultura local
            //var valorFormatado = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(Valores[0]));



            Quantidade.Add(new ValorOrcamento { ValorAvulso = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(Valores[0])), ValorContrato = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(Valores[1])), ValorPendente = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(Valores[2])) });

            reader.Close();
            con.Close();

            return Quantidade;

        }
        private List<Top15> ConsultaTop15(string mes, string ano)
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT top(15) sum(orc.TotalOrcamento) AS Valor, emp.Razao FROM Orcamento orc 
                                            join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
			                                join empresa emp on emp.IdEmpresa = orc.IdEmpresa
                                     where orc.IdSituacaoOrcamento = 4 and  year(lsit.DataCadastro) = @Ano  and  lsit.IdSituacao = 4  group by orc.IdEmpresa, emp.Razao order by Valor desc;                                

                                SELECT top(15) sum(orc.TotalOrcamento) AS Valor, emp.Razao FROM Orcamento orc 
                                            join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
			                                join empresa emp on emp.IdEmpresa = orc.IdEmpresa
                                     where orc.IdSituacaoOrcamento = 4 and  year(lsit.DataCadastro) = @Ano and  month(lsit.DataCadastro) = @Mes  and  lsit.IdSituacao = 4  group by orc.IdEmpresa, emp.Razao order by Valor desc";

            SQL.Parameters.AddWithValue("@Mes", mes);
            SQL.Parameters.AddWithValue("@Ano", ano);
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();

            // obtém a cultura local
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            // faz uma cópia das informações de formatação de número da cultura local
            var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            // fixa o símbolo da moeda estrangeira
            numberFormatInfo.CurrencySymbol = "R$";


            List<Top15> Quantidade = new List<Top15>();
            List<Top15Ano> TopAno = new List<Top15Ano>();
            List<Top15Mes> TopMes = new List<Top15Mes>();
            //string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])), reader["Razao"].ToString()

            while (reader.Read()) TopAno.Add(new Top15Ano(string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])), reader["Razao"].ToString()));
            reader.NextResult();
            while (reader.Read()) TopMes.Add(new Top15Mes(string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])), reader["Razao"].ToString()));

            Quantidade.Add(new Top15(TopMes, TopAno));

            reader.Close();
            con.Close();

            return Quantidade;
        }
        private List<ValorOrcamentoMesAno> ConsultaValorOrcamentoMesAno()
        {
            string ano = string.Empty;

            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = @"SELECT sum(orc.TotalOrcamento) as Valor,MONTH(lsit.DataCadastro) as mes, year(lsit.DataCadastro)as ano FROM Orcamento orc
                                 join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
                                 where lsit.IdSituacao = 4  and orc.IdTipoOrcamento = 1 group by year(lsit.DataCadastro), MONTH(lsit.DataCadastro) order by ano asc, mes asc;
 
                                SELECT sum(orc.TotalOrcamento) as Valor,MONTH(lsit.DataCadastro) as mes, year(lsit.DataCadastro)as ano FROM Orcamento orc
                                join LogSituacao lsit on lsit.IdOrcamento = orc.IdOrcamento
                                where lsit.IdSituacao = 4  and orc.IdTipoOrcamento = 2 group by year(lsit.DataCadastro), MONTH(lsit.DataCadastro) order by ano asc, mes asc;";

            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();

            //// obtém a cultura local
            //var cultureInfo = Thread.CurrentThread.CurrentCulture;
            //// faz uma cópia das informações de formatação de número da cultura local
            //var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            //// fixa o símbolo da moeda estrangeira
            //numberFormatInfo.CurrencySymbol = "";
            //// obtém o valor em moeda estrangeira formatado conforme a cultura local
            ////var valorFormatado = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(Valores[0]));


            List<ValorOrcamentoMesAno> Quantidade = new List<ValorOrcamentoMesAno>();
            List<ListaValorOrcamentoMesAno> OrcContrato = new List<ListaValorOrcamentoMesAno>();
            List<ListaValorOrcamentoMesAno> OrcAvulso = new List<ListaValorOrcamentoMesAno>();
            List<DadosValorOrcamentoMesAno> DadosContrato = new List<DadosValorOrcamentoMesAno>();
            List<DadosValorOrcamentoMesAno> DadosAvulso = new List<DadosValorOrcamentoMesAno>();


            //Prieiro Tipo Avulso            
            while (reader.Read())
            {
                ano = reader["ano"].ToString();
                // DadosAvulso.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])) });
                DadosAvulso.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString().Replace(',', '.') });
                while (reader.Read())
                {
                    if (ano == reader["ano"].ToString()) DadosAvulso.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = (Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString()).Replace(',', '.') }); //DadosAvulso.Add(new DadosValorOrcamentoMesAno () { Mes = reader["mes"].ToString(), Valor = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])) });
                    else
                    {
                        OrcAvulso.Add(new ListaValorOrcamentoMesAno() { Ano = ano, Dados = DadosAvulso });
                        ano = reader["ano"].ToString();
                        DadosAvulso = new List<DadosValorOrcamentoMesAno>();
                        DadosAvulso.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString().Replace(',', '.') });
                        //DadosAvulso.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])) });
                    }
                }
            }
            OrcAvulso.Add(new ListaValorOrcamentoMesAno() { Ano = ano, Dados = DadosAvulso });

            reader.NextResult();

            //Segundo Tipo Contrato
            while (reader.Read())
            {
                ano = reader["ano"].ToString();
                //DadosContrato.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])) });
                DadosContrato.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString().Replace(',', '.') });
                while (reader.Read())
                {
                    if (ano == reader["ano"].ToString()) DadosContrato.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = (Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString()).Replace(',', '.') });
                    else
                    {
                        OrcContrato.Add(new ListaValorOrcamentoMesAno() { Ano = ano, Dados = DadosContrato });
                        ano = reader["ano"].ToString();
                        DadosContrato = new List<DadosValorOrcamentoMesAno>();
                        //DadosContrato.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = string.Format(numberFormatInfo, "{0:C}", Convert.ToDecimal(reader["Valor"])) });
                        DadosContrato.Add(new DadosValorOrcamentoMesAno() { Mes = reader["mes"].ToString(), Valor = Math.Round(Convert.ToDecimal(reader["Valor"]), 2, MidpointRounding.AwayFromZero).ToString().Replace(',', '.') });
                    }
                }
            }
            OrcContrato.Add(new ListaValorOrcamentoMesAno() { Ano = ano, Dados = DadosContrato });


            Quantidade.Add(new ValorOrcamentoMesAno() { Avulso = OrcAvulso, Contrato = OrcContrato });

            reader.Close();
            con.Close();

            return Quantidade;

        }
    }
}
