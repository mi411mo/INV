using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DAL
{
    public class DataAccessRepository
    {
        private readonly IDataAccessLayer dal;
        public DataAccessRepository() : this(SqlServerDataSource.Inse) { }

        public DataAccessRepository(IDataAccessLayer _dal)
        {
            dal = _dal;
        }

        public async Task<DataTable> GetDataAsync(string query, SqlParameter[] param = null)
        {
            DataTable dt = await dal.SelectDataAsync(query, param);
            return dt;
        }
        
        public async Task<object> GetValueAsync(string query, SqlParameter[] param = null)
        {
            DataTable dt = await dal.SelectDataAsync(query, param);
            return dt.Rows[0][0].ToString();
        }
        public async Task<int> SetDataAsync(string query, Dictionary<string, object> param)
        {
            int Result = await dal.ExecuteCommandAsync(query, param);
            return Result;
        }
        public async Task<int> SetDataAsync(string query, SqlParameter[] param)
        {
            int Result = await dal.ExecuteCommandAsync(query, param);
            return Result;
        }
        public async Task<int> SetDataAsync(string query, SqlParameter param)
        {
            int Result = await dal.ExecuteCommandAsync(query, param);
            return Result;
        }
        public async Task<long> SetDataInsertAsync(string query, SqlParameter[] param)
        {
            long Result = await dal.InsertCommandAsync(query, param);
            return Result;
        }

    }
}
