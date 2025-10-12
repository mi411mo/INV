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
    public class IInvoiceParameterDaoImpl : IInvoiceParameterDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.InvoiceCustomParametersTBName + $" where  ParameterName IS NOT NULL";
        internal static string _InvoiceCustomParametersTBName = SqlConstants.InvoiceCustomParametersTBName.Trim();
        public static string InvoiceParameterTable => "InvoiceCustomParameters";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + InvoiceParameterTable + "  Where Id = @id", param);
                if (row > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetAll(DataAccessRepository daRpst)
        {
            try
            {
                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoiceParameterTable + " ");
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst)
        {
            try
            {
                string query = string.Empty;
                if (generalFilter.Id > 0)
                {
                    query += $" and  {_InvoiceCustomParametersTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_InvoiceCustomParametersTBName}.MerchantId ='{generalFilter.MerchantId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.IsActive.ToString()))
                {
                    query += $" and  {_InvoiceCustomParametersTBName}.IsActive ='{generalFilter.IsActive}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.ParameterName))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.ParameterName.Trim());
                    query += $" and  {_InvoiceCustomParametersTBName}.ParameterName LIKE N'%{name}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_InvoiceCustomParametersTBName}.CustomerId ='{generalFilter.CustomerId}'";
                }

                DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> GetById(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoiceParameterTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(InvoiceCustomParameter mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + InvoiceParameterTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, InvoiceCustomParameter mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@pName", mdl.ParameterName);
                param[2] = new SqlParameter("@merchantId", mdl.MerchantId);
                param[3] = new SqlParameter("@pType", mdl.ParameterType);
                param[4] = new SqlParameter("@isActive", mdl.IsActive);
                param[5] = new SqlParameter("@updatedAt", DateTime.Now);


                int row = await daRpst.SetDataAsync("Update " + InvoiceParameterTable + " SET ParameterName=@pName, MerchantId=@merchantId, ParameterType=@pType, IsActive=@isActive, UpdatedAt=@updatedAt WHERE Id=@id", param);
                if (row > 0) isSuccess = true;
            }
            catch(Exception ex)
            {
                throw;
            }

            return isSuccess;
        }

        public async Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst)
        {
            try
            {
                DateTime updatedAt = DateTime.Now;
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@status", status);
                param[2] = new SqlParameter("@updatedAt", DateTime.Now);

                var row = await daRpst.SetDataAsync("Update " + InvoiceParameterTable + " Set IsActive = @status, UpdatedAt=@updatedAt Where Id = @id", param);
                if (row > 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }      
    }
}
