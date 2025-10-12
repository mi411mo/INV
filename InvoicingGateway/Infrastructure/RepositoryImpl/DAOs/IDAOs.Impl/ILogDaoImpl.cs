using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DRY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs.Impl
{
    public class ILogDaoImpl : ILogDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.LogsTBName + $" where  SourceId IS NOT NULL";
        internal static string _LogsTBName = SqlConstants.LogsTBName.Trim();
        public static string LogsTable => "Logs";
        public async Task<DataTable> GetAll(DataAccessRepository daRpst)
        {
            DataTable dt = await daRpst.GetDataAsync(SelectTrans);
            return dt;
        }

        public async Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst)
        {
            string query = string.Empty;
            if (generalFilter.Id > 0)
            {
                query += $" and  {_LogsTBName}.Id ='{generalFilter.Id}'";
            }
            if (!string.IsNullOrEmpty(generalFilter.SourceId))
            {
                query += $" and  {_LogsTBName}.SourceId LIKE '%{generalFilter.SourceId}%'";

            }
            if (!string.IsNullOrEmpty(generalFilter.LogType))
            {
                query += $" and  {_LogsTBName}.LogType LIKE '%{generalFilter.LogType}%'";

            }           
            if (!string.IsNullOrEmpty(generalFilter.SearchKey))
            {
                var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                query += $" and ( {_LogsTBName}.LogType LIKE N'%{searchKey}%' OR  {_LogsTBName}.SourceId LIKE N'%{searchKey}%')";
            }
            if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
            {
                query += $" and   {_LogsTBName}.CustomerId ='{generalFilter.CustomerId}'";
            }

            DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query);
            return dt;

        }

        public async Task<DataTable> GetById(int id, DataAccessRepository daRpst)
        {
            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@id", id);

            DataTable dt = await daRpst.GetDataAsync("Select * from " + LogsTable + " Where Id = @id", param);
            return dt;
        }

        public async Task<bool> InsertAsync(Log mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                string[] ColumnsNames = StringExtensions.GetObjectPropertiesNames(mdl, false);
                Object[] ColumnsValues = StringExtensions.GetObjectPropertiesValues(mdl, false);

                List<SqlParameter> param = new List<SqlParameter>();
                string ColumnsNamess = string.Join(",", ColumnsNames), ColumnsValuess = string.Join(",", ColumnsValues);

                for (int i = 0; i < ColumnsNames.Length; i++)
                    param.Add(new SqlParameter("@" + ColumnsNames[i], ColumnsValues[i]));

                var id = await daRpst.SetDataInsertAsync("Insert into " + LogsTable + " (" + string.Join(",", ColumnsNames) + ") " +
                                         " Values(@" + string.Join(" ,@", ColumnsNames) + ") ", param.ToArray());


                if (id > 0)
                    isSuccess = true;
            }
            catch (Exception)
            {
                throw;
            }
            return isSuccess;
        }
    }
}
