using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static WebApiDefault.Models.MesAnoAtual;

namespace WebApiDefault.Controllers
{
    public class MesAnoAtualController : ApiController
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConNeo2"].ConnectionString);

        public IQueryable<JsonMesAnoAtual> GetJsonMesAnoAtual()
        {
            SqlCommand SQL = new SqlCommand();
            SQL.Connection = con;
            SQL.CommandText = "Select distinct year(DataCadastro) as anos from Orcamento order by anos asc";
            con.Open();
            SqlDataReader reader = SQL.ExecuteReader();

            List<JsonMesAnoAtual> JsonD = new List<JsonMesAnoAtual>();
            List<Ano> Anos = new List<Ano>();

            while (reader.Read()) { Anos.Add(new Ano(Convert.ToString(reader["anos"].ToString()))); }
            JsonD.Add(new JsonMesAnoAtual(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString(), Anos));
            con.Close();

            return JsonD.AsQueryable();
        }
    }
}
