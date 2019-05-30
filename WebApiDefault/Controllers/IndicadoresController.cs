using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static WebApiDefault.Models.Indicadores;

namespace WebApiDefault.Controllers
{
    public class IndicadoresController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["ConNeo2"].ConnectionString;

        public IQueryable<JsonIndicadores> GetJsonIndicadores()
        {
            List<JsonIndicadores> JsonD = new List<JsonIndicadores>();            

            JsonIndicadores Lista = new JsonIndicadores
            {               
                Servico = LstServico(),
                Amostra = LstAmostra(),
                Orcamento = LstOrcamento(),
                Laboratorio = LstLaboratorio(),
                Periodo = LstPeriodo(),
                SituacaoAmostra = LstSituacaoAmostra(),
                SituacaoOrcamento = LstSituacaoOrcamento(),
                TipoOrcamento = LstTipoOrcamento()                           
            };

            JsonD.Add(Lista);

            return JsonD.AsQueryable();
        }        

        public List<object> LstServico()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                Servico = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY b.IdLaboratorio) AS IdServico, b.IdLaboratorio, b.IdPeriodo, COUNT(b.IdItemAmostra) AS Quantidade, FORMAT(SUM(b.TotalCliente), 'N', 'pt-br') AS Valor
	                                    FROM (
	                                    SELECT ia.IdItemAmostra, ia.IdSituacaoServico, ia.IdLaboratorio, '1' AS IdPeriodo, ia.TotalCliente
	                                    FROM ItemAmostra ia
	                                    WHERE (ia.IdSituacaoServico = 2) and (DATEDIFF(DAY, ia.DataEntrega, GETDATE()) between -10 and -1)
	                                    UNION ALL
	                                    SELECT ia.IdItemAmostra, ia.IdSituacaoServico, ia.IdLaboratorio, '2' AS IdPeriodo, ia.TotalCliente
	                                    FROM ItemAmostra ia
	                                    WHERE (ia.IdSituacaoServico = 2) and (DATEDIFF(DAY, ia.DataEntrega, GETDATE()) between -20 and -11)
	                                    UNION ALL 
	                                    SELECT ia.IdItemAmostra, ia.IdSituacaoServico, ia.IdLaboratorio, '3' AS IdPeriodo, ia.TotalCliente
	                                    FROM ItemAmostra ia
	                                    WHERE (ia.IdSituacaoServico = 2) and (DATEDIFF(DAY, ia.DataEntrega, GETDATE()) between -30 and -21)
	                                    UNION ALL 
	                                    SELECT ia.IdItemAmostra, ia.IdSituacaoServico, ia.IdLaboratorio, '4' AS IdPeriodo, ia.TotalCliente
	                                    FROM ItemAmostra ia
	                                    WHERE (ia.IdSituacaoServico = 2) and (DATEDIFF(DAY, ia.DataEntrega, GETDATE()) <= -31)
	                                    UNION ALL 
	                                    SELECT ia.IdItemAmostra, ia.IdSituacaoServico, ia.IdLaboratorio, '5' AS IdPeriodo, ia.TotalCliente
	                                    FROM ItemAmostra ia
	                                    WHERE (ia.IdSituacaoServico = 2) and (DATEDIFF(DAY, ia.DataEntrega, GETDATE()) <= -1)
	                                    ) b
	                                    GROUP BY b.IdLaboratorio, b.IdPeriodo";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Servico.Add(new
                        {
                            IdServico = reader["IdServico"],
                            IdLaboratorio = reader["IdLaboratorio"],
                            IdPeriodo = reader["IdPeriodo"],
                            Quantidade = reader["Quantidade"],
                            Valor = (reader["Valor"] != DBNull.Value) ? Convert.ToDouble(reader["Valor"]) : 0,
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Servico;
                }
            }
        }

        public List<object> LstAmostra()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                Amostra = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT ROW_NUMBER() OVER (ORDER BY b.IdSituacaoAmostra) AS IdAmostra, b.IdSituacaoAmostra, b.IdTipoOrcamento, COUNT(b.IdAmostra) AS Quantidade, FORMAT(SUM(b.TotalCliente), 'N') AS Valor
	                                    from	
	                                    (SELECT a.IdAmostra, a.IdSituacaoAmostra,
	                                    (SELECT TOP 1 o.IdTipoOrcamento                                        
	                                    FROM ItemAmostra ia 
	                                    join Orcamento o ON ia.IdOrcamento = o.IdOrcamento                                                                                                         
	                                    WHERE ia.IdAmostra = a.IdAmostra) AS IdTipoOrcamento,
	                                    (SELECT SUM(ia.TotalCliente)
	                                    FROM ItemAmostra ia
	                                    WHERE ia.IdAmostra = a.IdAmostra) AS TotalCliente
	                                    FROM Amostra a
	                                    WHERE (a.IdTipoAmostra = 1 and a.IdNumTe IS NOT NULL)	
	                                    UNION ALL	
	                                    SELECT a.IdAmostra, a.IdSituacaoAmostra, '3' AS IdTipoOrcamento,
	                                    (SELECT SUM(ia.TotalCliente)
	                                    FROM ItemAmostra ia
	                                    WHERE ia.IdAmostra = a.IdAmostra) AS TotalCliente
	                                    FROM Amostra a
	                                    WHERE (a.IdTipoAmostra = 1 and a.IdNumTe IS NOT NULL)	
	                                    ) b
	                                    WHERE b.IdSituacaoAmostra <= 4 and b.IdTipoOrcamento IS NOT NULL 
	                                    GROUP BY b.IdSituacaoAmostra, b.IdTipoOrcamento";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Amostra.Add(new
                        {
                            IdAmostra = reader["IdAmostra"],
                            IdSituacaoAmostra = reader["IdSituacaoAmostra"],
                            IdTipoOrcamento = reader["IdTipoOrcamento"],
                            Quantidade = reader["Quantidade"],
                            Valor = (reader["Valor"] != DBNull.Value) ? Convert.ToDouble(reader["Valor"]) : 0,
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Amostra;
                }
            }
        }

        public List<object> LstOrcamento()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                Orcamento = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT c.IdOrcamento, c.IdSituacaoOrcamento, c.IdTipoOrcamento, c.Quantidade, c.Valor
	                                    FROM (
	                                    SELECT ROW_NUMBER() OVER (ORDER BY b.IdSituacaoOrcamento) AS IdOrcamento, b.IdSituacaoOrcamento, b.IdTipoOrcamento, b.Quantidade, b.Valor
	                                    FROM(
	                                    SELECT o.IdSituacaoOrcamento, o.IdTipoOrcamento, COUNT(o.IdOrcamento) AS Quantidade, FORMAT(SUM(o.TotalOrcamento), 'N') AS Valor	
	                                    FROM Orcamento o
	                                    WHERE o.IdTipoOrcamento = 1 and (o.IdSituacaoOrcamento <= 3 or o.IdSituacaoOrcamento = 10)
	                                    GROUP BY o.IdSituacaoOrcamento, o.IdTipoOrcamento	
	                                    ) b
	                                    UNION ALL
	                                    SELECT ROW_NUMBER() OVER (ORDER BY b.IdSituacaoOrcamento) AS IdOrcamento, b.IdSituacaoOrcamento, b.IdTipoOrcamento, b.Quantidade, b.Valor
	                                    FROM(
	                                    SELECT o.IdSituacaoOrcamento, o.IdTipoOrcamento, COUNT(o.IdOrcamento) AS Quantidade, FORMAT(SUM(o.TotalOrcamento), 'N') AS Valor	
	                                    FROM Orcamento o
	                                    WHERE o.IdTipoOrcamento = 2 and (o.IdSituacaoOrcamento <= 3 or o.IdSituacaoOrcamento = 10)
	                                    GROUP BY o.IdSituacaoOrcamento, o.IdTipoOrcamento
	                                    UNION ALL
	                                    SELECT o.IdSituacaoOrcamento, '3' AS IdTipoOrcamento, COUNT(o.IdOrcamento) AS Quantidade, FORMAT(SUM(o.TotalOrcamento), 'N') AS Valor
	                                    FROM Orcamento o
	                                    WHERE (o.IdSituacaoOrcamento <= 3 or o.IdSituacaoOrcamento = 10)
	                                    GROUP BY o.IdSituacaoOrcamento
	                                    ) b 
	                                    ) c";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Orcamento.Add(new
                        {
                            IdOrcamento = reader["IdOrcamento"],
                            IdSituacaoOrcamento = reader["IdSituacaoOrcamento"],
                            IdTipoOrcamento = reader["IdTipoOrcamento"],
                            Quantidade = reader["Quantidade"],
                            Valor = (reader["Valor"] != DBNull.Value) ? Convert.ToDouble(reader["Valor"]) : 0
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Orcamento;
                }
            }
        }

        public List<object> LstLaboratorio()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                Laboratorio = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT l.IdLaboratorio, l.NomeLaboratorio
                                        FROM Laboratorio l WHERE l.Ativo = 1";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Laboratorio.Add(new
                        {
                            IdLaboratorio = reader["IdLaboratorio"],
                            NomeLaboratorio = reader["NomeLaboratorio"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Laboratorio;
                }
            }
        }

        public List<object> LstPeriodo()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                Periodo = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT p.IdPeriodo, p.NomePeriodo
                                        FROM Periodo p";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Periodo.Add(new
                        {
                            IdPeriodo = reader["IdPeriodo"],
                            NomePeriodo = reader["NomePeriodo"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Periodo;
                }
            }
        }

        public List<object> LstSituacaoAmostra()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                SituacaoAmostra = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT s.IdSituacaoAmostra, s.NomeSituacaoAmostra
                                        FROM SituacaoAmostra s WHERE s.IdSituacaoAmostra <= 4 and s.Ativo = 1";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.SituacaoAmostra.Add(new
                        {
                            IdSituacaoAmostra = reader["IdSituacaoAmostra"],
                            NomeSituacaoAmostra = reader["NomeSituacaoAmostra"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.SituacaoAmostra;
                }
            }
        }

        public List<object> LstSituacaoOrcamento()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                SituacaoOrcamento = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT s.IdSituacaoOrcamento, s.NomeSituacaoOrcamento
                                        FROM SituacaoOrcamento s WHERE s.Ativo = 1";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.SituacaoOrcamento.Add(new
                        {
                            IdSituacaoOrcamento = reader["IdSituacaoOrcamento"],
                            NomeSituacaoOrcamento = reader["NomeSituacaoOrcamento"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.SituacaoOrcamento;
                }
            }
        }

        public List<object> LstTipoOrcamento()
        {
            JsonIndicadores Lista = new JsonIndicadores
            {
                TipoOrcamento = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT t.IdTipoOrcamento, t.NomeTipoOrcamento
                                        FROM TipoOrcamento t WHERE t.Ativo = 1";


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.TipoOrcamento.Add(new
                        {
                            IdTipoOrcamento = reader["IdTipoOrcamento"],
                            NomeTipoOrcamento = reader["NomeTipoOrcamento"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.TipoOrcamento;
                }
            }
        }        
    }
}
