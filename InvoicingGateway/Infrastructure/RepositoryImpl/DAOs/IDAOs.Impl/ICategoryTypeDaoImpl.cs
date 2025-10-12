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
    public class ICategoryTypeDaoImpl : ICategoryTypeDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.CategoryTypeTBName + $" where  Name IS NOT NULL";
        internal static string _CategoryTypeTBName = SqlConstants.CategoryTypeTBName.Trim();
        public static string CategoryTypeTable => "CategoryTypes";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + CategoryTypeTable + "  Where Id = @id", param);
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
                DataTable dt = await daRpst.GetDataAsync("Select * from " + CategoryTypeTable + " ");
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
                    query += $" and  {_CategoryTypeTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.Type > 0)
                {
                    query += $" and  {_CategoryTypeTBName}.Type ='{generalFilter.Type}'";
                }
                if (generalFilter.CategoryType > 0)
                {
                    query += $" and  {_CategoryTypeTBName}.Type ='{generalFilter.CategoryType}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.IsActive.ToString()))
                {
                    query += $" and  {_CategoryTypeTBName}.IsActive ='{generalFilter.IsActive}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.Name))
                {
                    query += $" and  {_CategoryTypeTBName}.Name LIKE '%{generalFilter.Name}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.EnName))
                {
                    query += $" and  {_CategoryTypeTBName}.EnName LIKE '%{generalFilter.EnName}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_CategoryTypeTBName}.CustomerId ='{generalFilter.CustomerId}'";
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

                DataTable dt = await daRpst.GetDataAsync("Select * from " + CategoryTypeTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(CategoryType mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + CategoryTypeTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, CategoryType mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@name", mdl.Name);
                param[2] = new SqlParameter("@enName", mdl.EnName);
                param[3] = new SqlParameter("@type", mdl.Type);
                param[4] = new SqlParameter("@code", mdl.Code);
                param[5] = new SqlParameter("@isActive", mdl.IsActive);
                param[6] = new SqlParameter("@updatedAt", DateTime.Now);
                param[7] = new SqlParameter("@description", mdl.Description);


                int row = await daRpst.SetDataAsync("Update " + CategoryTypeTable + " SET Name=@name, EnName=@enName, Type=@type, Code=@code, IsActive=@isActive, UpdatedAt=@updatedAt, Description=@description WHERE Id=@id", param);
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
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@status", status);

                var row = await daRpst.SetDataAsync("Update " + CategoryTypeTable + " Set IsActive = @status Where Id = @id", param);
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
