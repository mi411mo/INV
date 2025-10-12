using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DAL
{
    public interface IDataAccessLayer
    {
        Task<DataTable> SelectDataAsync(string query, SqlParameter[] param);
        Task<int> ExecuteCommandAsync(string query, Dictionary<string, object> Para);
        Task<int> ExecuteCommandAsync(string query, SqlParameter[] param);
        Task<int> ExecuteCommandAsync(string query, SqlParameter param);
        Task<int> ExecuteCommandAsync(string query);
        Task<long> InsertCommandAsync(string query, SqlParameter[] param);
    }
}
