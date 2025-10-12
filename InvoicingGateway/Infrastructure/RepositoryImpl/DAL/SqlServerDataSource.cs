using Application.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;

namespace Infrastructure.RepositoryImpl.DAL
{
    sealed class SqlServerDataSource : IDataAccessLayer, IDisposable
    {

        private SqlServerDataSource()
        {
            string connectionDBStringEnvName = ConfigHelper.Configuration.GetSection("ConnectionStrings")["ConnectionString"];
            connectionString = SystemEnviornmentLookup.GetEnvVariableValue(connectionDBStringEnvName);

        }
        private static SqlServerDataSource sqlServerDataSource = null;
        public static SqlServerDataSource Inse
        {
            get
            {
                if (sqlServerDataSource == null)
                    sqlServerDataSource = new SqlServerDataSource();
                return sqlServerDataSource;
            }
        }

        String connectionString = SystemEnviornmentLookup.GetEnvVariableValue(ConfigHelper.Configuration.GetSection("ConnectionStrings")["ConnectionString"]);

        public async Task<DataTable> SelectDataAsync(string query, SqlParameter[] param)
        {

            DataTable dt = new DataTable();
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {

                SqlCommand sqlcmd = new SqlCommand(query, Connection);
                SqlDataAdapter da = new SqlDataAdapter(sqlcmd);
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlcmd.Parameters.Add(param[i]);
                    }
                }

                await Task.Run(() => da.Fill(dt));
            }
            return dt;
        }


        public async Task<int> ExecuteCommandAsync(string query, Dictionary<string, object> Para)
        {
            int result = 0;
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                //SqlCommand cmd = new SqlCommand(query, Connection);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                cmd.Connection = Connection;

                foreach (var item in Para)
                    if (item.Value != null) cmd.Parameters.AddWithValue(item.Key, item.Value);
                Connection.Open();
                 result = await cmd.ExecuteNonQueryAsync();
                Connection.Close();
            }
            return result;
        }

        //Method To Insert,Update and Delete Data From DataBase
        public async Task<int> ExecuteCommandAsync(string query, SqlParameter[] param)
        {
            int result = 0;
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                //SqlCommand sqlcmd = new SqlCommand(query, Connection);
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = query;
                sqlcmd.Connection = Connection;
                if (param != null)
                {
                    sqlcmd.Parameters.AddRange(param);
                }
                Connection.Open();
                result = await sqlcmd.ExecuteNonQueryAsync();
                Connection.Close();
            }
            return result;
        }

        public async Task<int> ExecuteCommandAsync(string query, SqlParameter param)
        {
            int result = 0;
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = query;
                sqlcmd.Connection = Connection;
                if (param != null)
                {
                    sqlcmd.Parameters.Add(param);
                }
                Connection.Open();
                result = await sqlcmd.ExecuteNonQueryAsync();
                Connection.Close();
            }
            return result;
        }

        public async Task<int> ExecuteCommandAsync(string query)
        {
            int result = 0;
            using (SqlConnection Connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, Connection);

                Connection.Open();
                result = await cmd.ExecuteNonQueryAsync();
                Connection.Close();
            }
            return result;
        }
        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<long> InsertCommandAsync(string query, SqlParameter[] param)
        {
            try
            {
                long result = 0;
                using (SqlConnection Connection = new SqlConnection(connectionString))
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.CommandType = CommandType.Text;
                    sqlcmd.CommandText = query + " SELECT CAST(SCOPE_IDENTITY() AS bigint)  WHERE @@ROWCOUNT > 0";
                    sqlcmd.Connection = Connection;
                    if (param != null)
                    {
                        sqlcmd.Parameters.AddRange(param);
                    }
                    Connection.Open();
                    result =  (long) await sqlcmd.ExecuteScalarAsync();
                    Connection.Close();
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
