using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static WebApiDefault.Models.Agenda;

namespace WebApiDefault.Controllers
{
    [Authorize]
    public class AgendaController : ApiController
    {
        string constr = ConfigurationManager.ConnectionStrings["ConNeo1"].ConnectionString;

        public IQueryable<JsonAgenda> GetJsonAgenda()
        {
            List<JsonAgenda> JsonD = new List<JsonAgenda>();

            JsonAgenda Lista = new JsonAgenda
            {
                MesAnoEvento = LstMesAnoEvento(),
                Evento = LstEvento()
            };

            JsonD.Add(Lista);

            return JsonD.AsQueryable();
        }

        public List<object> LstMesAnoEvento()
        {
            JsonAgenda Lista = new JsonAgenda
            {
                MesAnoEvento = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT ROW_NUMBER() OVER(ORDER BY b.Ano ASC) AS IdMesAno, b.Mes + '/' + b.Ano As NomeMesAno
	                                    FROM
	                                    (SELECT DATEPART(M, e.data_inicio) AS NumeroMes, DATENAME(M, e.data_inicio) AS Mes, DATENAME(YYYY, e.data_inicio) AS Ano
	                                    FROM Eventos e
	                                    WHERE e.data_inicio >= CONVERT(date, GETDATE())
	                                    GROUP BY DATENAME(YYYY, e.data_inicio), DATEPART(M, e.data_inicio), DATENAME(M, e.data_inicio)
	                                    ) b";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.MesAnoEvento.Add(new
                        {
                            IdMesAno = reader["IdMesAno"],
                            NomeMesAno = reader["NomeMesAno"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.MesAnoEvento;
                }
            }
        }

        public List<object> LstEvento()
        {
            JsonAgenda Lista = new JsonAgenda
            {
                Evento = new List<object>()
            };

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.CommandText = @"SELECT e.id_evento AS IdEvento, e.evento AS Evento, 
	                                    DAY(e.data_inicio) AS Dia, MONTH(e.data_inicio) As Mes, YEAR(e.data_inicio) AS Ano, 
	                                    FORMAT(e.data_inicio, 'HH:mm') AS Inicio,
	                                    e.hora_fim AS Fim,
	                                    CONVERT(varchar, e.data_inicio, 103) AS DataEvento,
	                                    FORMAT(e.data_inicio, 'MM/yyyy') AS MesAno,
	                                    DATENAME(m, e.data_inicio) + '/' + CONVERT(varchar, YEAR(e.data_inicio)) As NomeMesAno,
	                                    CONVERT(VARCHAR, GETDATE(), 103) + ' ' + CONVERT(VARCHAR, FORMAT(e.data_inicio, 'HH:mm')) AS DataHora, 
	                                    CONVERT(VARCHAR, FORMAT(e.data_inicio, 'HH:mm')) + ' - ' + CONVERT(VARCHAR, e.hora_fim) AS InicioFim,
	                                    CASE DATEPART(WEEKDAY, e.data_inicio)  
                                        WHEN 1 THEN 'Domingo' 
                                        WHEN 2 THEN 'Segunda' 
                                        WHEN 3 THEN 'Terça  ' 
                                        WHEN 4 THEN 'Quarta ' 
                                        WHEN 5 THEN 'Quinta ' 
                                        WHEN 6 THEN 'Sexta  ' 
                                        WHEN 7 THEN 'Sábado ' 
	                                    END	
	                                    AS DiaSemana,
	                                    DATEPART(WK, e.data_inicio) AS Semana,
	                                    CASE WHEN (DATEPART(WK, e.data_inicio) = DATEPART(WK, GETDATE())) then 1 when (DATEPART(WK, e.data_inicio) != DATEPART(WK, GETDATE())) then 0 end as EstaSemana,
	                                    CONVERT(VARCHAR, DATEADD(DAY, 1, CONVERT(DATE, GETDATE())), 103) AS Amanha, 
	                                    CASE WHEN (DATEADD(DAY, 1, CONVERT(DATE, GETDATE())) = CONVERT(DATE, e.data_inicio)) then 1 when (DATEADD(DAY, 1, CONVERT(DATE, GETDATE())) != CONVERT(DATE, e.data_inicio)) then 0 end as ProximoDia
	                                    FROM Eventos e
	                                    WHERE e.data_inicio >= CONVERT(DATE, GETDATE()) 
	                                    ORDER BY e.data_inicio ASC";

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Evento.Add(new
                        {
                            IdEvento = reader["IdEvento"],
                            Evento = reader["Evento"],
                            Dia = reader["Dia"],
                            Mes = reader["Mes"],
                            Ano = reader["Ano"],
                            Inicio = reader["Inicio"],
                            Fim = reader["Fim"],
                            DataEvento = reader["DataEvento"],
                            MesAno = reader["MesAno"],
                            NomeMesAno = reader["NomeMesAno"],
                            DataHora = reader["DataHora"],
                            InicioFim = reader["InicioFim"],
                            DiaSemana = reader["DiaSemana"],
                            Semana = reader["Semana"],
                            EstaSemana = reader["EstaSemana"],
                            Amanha = reader["Amanha"],
                            ProximoDia = reader["ProximoDia"]
                        });
                    }
                    reader.Close();
                    cmd.Dispose();
                    con.Close();

                    return Lista.Evento;
                }
            }
        }
    }
}
