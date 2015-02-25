using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConexaoBd
{
    public class DbConnection
    {
        private SqlConnection _conn;

        private SqlConnection CriarConexao()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["strConnection"].ConnectionString);
        }

        private SqlParameterCollection _sqlParameterCollection = new SqlCommand().Parameters;

        private SqlCommand CriarComando(CommandType cmdType, string cmdSql)
        {
            _conn = CriarConexao();
            _conn.Open();
            var cmd = _conn.CreateCommand();
            cmd.CommandType = cmdType;
            cmd.CommandText = cmdSql;
            foreach (SqlParameter sqlParameter in _sqlParameterCollection)
            {
                cmd.Parameters.Add(new SqlParameter(sqlParameter.ParameterName, sqlParameter.Value));
            }

            return cmd;
        }

        public void LimparParametros()
        {
            _sqlParameterCollection.Clear();
        }

        public void AdicionarParametros(string nomeParametro, object valorParametro)
        {
            _sqlParameterCollection.AddWithValue(nomeParametro, valorParametro);
        }

        public void ExecutarComando(CommandType cmdType, string cmdSql)
        {
            try
            {
                var now = DateTime.Now;
                var cmd = CriarComando(cmdType, cmdSql);
                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();                
            }
        }

        public DataTable ExecutarConsulta(CommandType cmdType, string cmdSql)
        {
            try
            {
                var dt = new DataTable();
                var cmd = CriarComando(cmdType, cmdSql);  
              
                //var now = DateTime.Now;
                var sqlAdapter = new SqlDataAdapter(cmd);                
                sqlAdapter.Fill(dt);
                //var latter = DateTime.Now - now;
                //Console.WriteLine("SqlDataAdapter Consultou em: " + latter.TotalMilliseconds + " milisegundos");
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        public SqlDataReader ExecutarConsultaReader(CommandType cmdType, string cmdSql)
        {
            try
            {
                var dt = new DataTable();                
                var cmd = CriarComando(cmdType, cmdSql);
                SqlDataReader dr = null;
                //var now = DateTime.Now;                               
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);              
                //var latter = DateTime.Now - now;
                //Console.WriteLine("SqlDataReader Consultou em: " + latter.TotalMilliseconds + " milisegundos");
                return dr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

     }
}
