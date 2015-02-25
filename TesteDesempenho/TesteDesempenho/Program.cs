using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConexaoBd;
using EfContexto;

namespace TesteDesempenho
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DbConnection();
            ContextoEf contexto = new ContextoEf();


            #region Entityframework

            var now = DateTime.Now;
            var teste = contexto.Testes.ToList();
            var latter = DateTime.Now - now;
            Console.WriteLine("EntityFramework Consultou em: " + latter.TotalSeconds + " segundos");
           
            #endregion


            var listaTeste = new List<Teste>();


            #region DataTable

            now = DateTime.Now;
            var dt = db.ExecutarConsulta(CommandType.Text, "select * from Testes");
            foreach (DataRow linha in dt.Rows)           
            {
                var t = new Teste
                {
                    Id = Convert.ToInt32(linha["Id"]),
                    Nome = linha["Nome"].ToString()
                };
                listaTeste.Add(t);
            }
            latter = DateTime.Now - now;
            Console.WriteLine("DataAdapter Consultou em: " + latter.TotalSeconds + " segundos");
            
            #endregion


            listaTeste.Clear();



            #region DataReader

            now = DateTime.Now;
            var dr = db.ExecutarConsultaReader(CommandType.Text, "select * from Testes");
            while (dr.Read())
            {
                var t = new Teste()
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Nome = dr["Nome"].ToString()
                };
                listaTeste.Add(t);
            }
            latter = DateTime.Now - now;          
            Console.WriteLine("DataReader Consultou em: " + latter.TotalSeconds + " segundos");
            
            #endregion


            Console.WriteLine("Terminado");
            Console.ReadKey();
        }
    }
}
