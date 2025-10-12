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
    public class ICategoryDaoImpl : ICategoryDao
    {
        private string SelectTrans { get; set; } = $"Select c.*,  (SELECT COUNT(*) From Products p WHERE p.CategoryId = c.Id) As ProductCount  FROM {SqlConstants.CategoryTBName.Trim()} c where c.CategoryName IS NOT NULL";
        internal static string _CategoryTBName = SqlConstants.CategoryTBName.Trim();
        public static string CategoryTable => "Categories";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + CategoryTable + "  Where Id = @id", param);
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
                DataTable dt = await daRpst.GetDataAsync("Select * from " + CategoryTable + " ");
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
                    query += $" and  c.Id ='{generalFilter.Id}'";

                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  c.MerchantId ='{generalFilter.MerchantId}'";

                }
                if (!string.IsNullOrEmpty(generalFilter.CategoryName))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.CategoryName.Trim());
                    query += $" and  c.CategoryName LIKE N'%{name}%'";

                }
                if (!string.IsNullOrEmpty(generalFilter.IsActive.ToString()))
                {
                    query += $" and   c.IsActive ='{generalFilter.IsActive}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( c.CategoryName LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   c.CustomerId ='{generalFilter.CustomerId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderBy?.ToString()))
                {
                    if (!string.IsNullOrEmpty(generalFilter.OrderType?.ToString()))
                        query += $" ORDER BY '{generalFilter.OrderBy}'" + " " + generalFilter.OrderType;

                    else
                        query += $" ORDER By '{generalFilter.OrderBy}' ASC";

                }

                DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query );
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

                DataTable dt = await daRpst.GetDataAsync($"Select c.*,  (SELECT COUNT(*) From Products p WHERE p.CategoryId = c.Id) As ProductCount  FROM {SqlConstants.CategoryTBName.Trim()} c where c.Id=@id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Category mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + CategoryTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, Category mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@name", mdl.CategoryName);
                param[2] = new SqlParameter("@merchantId", mdl.MerchantId);
                param[3] = new SqlParameter("@description", mdl.Description);
                param[4] = new SqlParameter("@categoryType", mdl.CategoryType);
                param[5] = new SqlParameter("@imageUrl", mdl.ImageURL);


                int row = await daRpst.SetDataAsync("Update " + CategoryTable + " SET CategoryName=@name, MerchantId=@merchantId, ImageURL=@imageUrl, CategoryType=@categoryType, Description=@description WHERE Id=@id", param);
                if (row > 0) isSuccess = true;
            }
            catch
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

                var row = await daRpst.SetDataAsync("Update " + CategoryTable + " Set IsActive = @status Where Id = @id", param);
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
