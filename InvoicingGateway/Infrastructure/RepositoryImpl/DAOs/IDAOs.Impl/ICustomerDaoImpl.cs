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
    public class ICustomerDaoImpl : ICustomerDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.CustomerTBName + $" where  Name IS NOT NULL";
        internal static string _CustomerTBName = SqlConstants.CustomerTBName.Trim();
        public static string CustomerTable => "Customers";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + CustomerTable + "  Where Id = @id", param);
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
                DataTable dt = await daRpst.GetDataAsync("Select * from " + CustomerTable + " ");
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
                    query += $" and  {_CustomerTBName}.Id ='{generalFilter.Id}'";

                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerName))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.CustomerName.Trim());
                    query += $" and  {_CustomerTBName}.Name LIKE N'%{name}%'";

                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerPhone))
                {
                    query += $" and  {_CustomerTBName}.Phone ='{generalFilter.CustomerPhone}'";

                }
                if (!string.IsNullOrEmpty(generalFilter.Email))
                {
                    query += $" and  {_CustomerTBName}.Email LIKE '%{generalFilter.Email}%'";

                }
                if (!string.IsNullOrEmpty(generalFilter.Name))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.Name.Trim());
                    query += $" and  {_CustomerTBName}.Name LIKE N'%{name}%'";

                }
                if (!string.IsNullOrEmpty(generalFilter.Address))
                {
                    query += $" and  {_CustomerTBName}.Address LIKE '%{generalFilter.Address}%'";

                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_CustomerTBName}.MerchantId ='{generalFilter.MerchantId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.IsActive.ToString()))
                {
                    query += $" and  {_CustomerTBName}.IsActive ='{generalFilter.IsActive}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_CustomerTBName}.Name LIKE N'%{searchKey}%' OR  {_CustomerTBName}.Phone LIKE N'%{searchKey}%' OR  {_CustomerTBName}.Email LIKE N'%{searchKey}%' OR  {_CustomerTBName}.Address LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_CustomerTBName}.CustomerId ='{generalFilter.CustomerId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderBy?.ToString()))
                {
                    if (!string.IsNullOrEmpty(generalFilter.OrderType?.ToString()))
                        query += $" ORDER BY '{generalFilter.OrderBy}'" + " " + generalFilter.OrderType;

                    else
                        query += $" ORDER By '{generalFilter.OrderBy}' ASC";

                }

                DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query);
                return dt;
            }
            catch(Exception ex)
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

                DataTable dt = await daRpst.GetDataAsync("Select * from " + CustomerTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Customer mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + CustomerTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, Customer mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[10];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@name", mdl.Name);
                param[2] = new SqlParameter("@email", mdl.Email);
                param[3] = new SqlParameter("@phone", mdl.Phone);
                param[4] = new SqlParameter("@address", mdl.Address);
                param[5] = new SqlParameter("@prefix", mdl.InvoicePrefix);
                param[6] = new SqlParameter("@status", mdl.IsActive);
                param[7] = new SqlParameter("@details", mdl.Details);
                param[8] = new SqlParameter("@merchantId", mdl.MerchantId);
                param[9] = new SqlParameter("@categoryType", mdl.CategoryType);

                int row = await daRpst.SetDataAsync("Update " + CustomerTable + " SET Name=@name, Address=@address, Email=@email, CategoryType=@categoryType, Phone=@phone, InvoicePrefix=@prefix, " +
                                                " MerchantId=@merchantId, IsActive=@status, Details=@details Where Id=@id", param);
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

                var row = await daRpst.SetDataAsync("Update " + CustomerTable + " Set IsActive = @status Where Id = @id", param);
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
