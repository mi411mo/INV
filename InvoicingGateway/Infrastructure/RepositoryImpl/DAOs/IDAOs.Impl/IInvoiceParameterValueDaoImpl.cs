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

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs.Impl
{
    public class IInvoiceParameterValueDaoImpl : IInvoiceParameterValueDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.InvoiceParameterValuesTBName + $" where  InvoiceId IS NOT NULL";
        internal static string _InvoiceParameterValuesTBName = SqlConstants.InvoiceParameterValuesTBName.Trim();
        public static string InvoiceValuesTable => "InvoiceCustomValues";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + InvoiceValuesTable + "  Where Id = @id", param);
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
                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoiceValuesTable + " ");
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
                    query += $" and  {_InvoiceParameterValuesTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.InvoiceId > 0)
                {
                    query += $" and  {_InvoiceParameterValuesTBName}.InvoiceId ='{generalFilter.InvoiceId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.ParameterValue.ToString()))
                {
                    query += $" and  {_InvoiceParameterValuesTBName}.ParameterValue ='{generalFilter.ParameterValue}'";
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

                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoiceValuesTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(InvoiceCustomValue mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + InvoiceValuesTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, InvoiceCustomValue mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[5];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@parId", mdl.ParameterId);
                param[2] = new SqlParameter("@invId", mdl.InvoiceId);
                param[3] = new SqlParameter("@pValue", mdl.ParameterValue);
                param[4] = new SqlParameter("@updatedAt", DateTime.Now);


                int row = await daRpst.SetDataAsync("Update " + InvoiceValuesTable + " SET ParameterId=@parId, InvoiceId=@invId, ParameterValue=@pValue, UpdatedAt=@updatedAt WHERE Id=@id", param);
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

                var row = await daRpst.SetDataAsync("Update " + InvoiceValuesTable + " Set IsActive = @status, UpdatedAt=@updatedAt Where Id = @id", param);
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
