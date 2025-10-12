using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils;
using Domain.Utils.Enums;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DRY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs.Impl
{
    public class IOrdersDaoImpl : IOrdersDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.OrderTBName + $" where  OrderReference IS NOT NULL";
        internal static string _OrdersTBName = SqlConstants.OrderTBName.Trim();
        public static string OrdersTable => "Orders";

        public async Task<bool> Approve(string orderRef, DataAccessRepository daRpst)
        {
            try
            {
                DateTime updatedAt = DateTime.Now;
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@ref", orderRef);
                param[1] = new SqlParameter("@Status", (int)OrderStatusEnum.Confirmed);

                var row = await daRpst.SetDataAsync("Update " + OrdersTable + " Set Status = @Status Where OrderReference = @ref", param);
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

        public Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTable> GetAll(DataAccessRepository daRpst)
        {
            try
            {
                DataTable dt = await daRpst.GetDataAsync("Select * from " + OrdersTable + " ");
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
                    query += $" and  {_OrdersTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_OrdersTBName}.MerchantId ='{generalFilter.MerchantId}'";
                }
                if ((int)generalFilter.OrderStatus > 0)
                {
                    query += $" and  {_OrdersTBName}.Status ='{(int)generalFilter.OrderStatus}'";
                }
                if (generalFilter.TotalAmount > 0)
                {
                    query += $" and  {_OrdersTBName}.TotalAmount ='{generalFilter.TotalAmount}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CurrencyCode))
                {
                    query += $" and  {_OrdersTBName}.CurrencyCode = '{generalFilter.CurrencyCode}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderReference))
                {
                    query += $" and  {_OrdersTBName}.OrderReference = '{generalFilter.OrderReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerName))
                {
                    query += $" and JSON_VALUE({_OrdersTBName}.CustomerInfo,'$.customerName') LIKE '%{generalFilter.CustomerName}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerPhone))
                {
                    query += $" and JSON_VALUE({_OrdersTBName}.CustomerInfo,'$.phone') = '{generalFilter.CustomerPhone}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.Email))
                {
                    query += $" and JSON_VALUE({_OrdersTBName}.CustomerInfo,'$.email') LIKE '%{generalFilter.Email}%'";
                }
                if (generalFilter.ProductId > 0)
                {
                    query += $" and JSON_VALUE({_OrdersTBName}.Products,'$.id') = '{generalFilter.ProductId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_OrdersTBName}.OrderReference LIKE N'%{searchKey}%' OR {_OrdersTBName}.CurrencyCode LIKE N'%{searchKey}%' OR {_OrdersTBName}.TotalAmount LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_OrdersTBName}.CustomerId ='{generalFilter.CustomerId}'";
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> GetOrderById(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + OrdersTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetOrderByRef(string orderRef, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ref", orderRef);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + OrdersTable + " Where OrderReference = @ref", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(OrderModel mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + OrdersTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public Task<bool> Update(int id, OrderModel mdl, DataAccessRepository daRpst)
        {
            /*bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[4];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@nameAr", mdl.nameAr);
                param[2] = new SqlParameter("@nameEn", mdl.nameEn);
                param[3] = new SqlParameter("@description", mdl.description);

                int row = await daRpst.SetDataAsync("Update " + tableName + " SET nameAr=@nameAr, nameEn=@nameEn, description=@description Where id=@id", param);
                if (row > 0) isSuccess = true;
            }
            catch
            {
                throw;
            }

            return isSuccess;*/
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@Status", status);
                param[2] = new SqlParameter("@updatedAt", DateTime.Now);

                var row = await daRpst.SetDataAsync("Update " + OrdersTable + " Set Status = @Status, UpdatedAt = @updatedAt Where Id = @id", param);
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
