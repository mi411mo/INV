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
    public class IAuditLogDaoImpl : IAuditLogDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.AuditLogsTBName + $" where  TableName IS NOT NULL";
        internal static string _AuditLogsTBName = SqlConstants.AuditLogsTBName.Trim();
        public static string AuditLogTable => "AuditLogs";
        public async Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst)
        {
            try
            {
                string query = string.Empty;
                if (generalFilter.Id > 0)
                {
                    query += $" and  c.Id ='{generalFilter.Id}'";

                }

                if (!string.IsNullOrEmpty(generalFilter.TableName))
                {
                    query += $" and  {_AuditLogsTBName}.TableName LIKE '%{generalFilter.TableName}%'";

                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_AuditLogsTBName}.Type LIKE N'%{searchKey}%' OR {_AuditLogsTBName}.TableName LIKE N'%{searchKey}%' OR {_AuditLogsTBName}.CustomerId LIKE N'%{searchKey}%' OR {_AuditLogsTBName}.ClientId LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   c.CustomerId ='{generalFilter.CustomerId}'";
                }

                DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(AuditLog mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + AuditLogTable + " (" + string.Join(",", ColumnsNames) + ") " +
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
