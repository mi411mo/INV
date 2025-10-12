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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs.Impl
{
    public class IInvoiceDaoImpl : IInvoiceDao
    {
        private string SelectTrans { get; set; } = $"SELECT inv.*, ( SELECT cp.ParameterName, cp.ParameterType, cv.ParameterValue FROM {SqlConstants.InvoiceParameterValuesTBName} cv INNER JOIN {SqlConstants.InvoiceCustomParametersTBName} cp ON cv.ParameterId = cp.Id WHERE cv.InvoiceId = inv.Id FOR JSON PATH) AS CustomParameters   FROM " + SqlConstants.InvoiceTBName + $" inv";
        internal static string _InvoicesTBName = SqlConstants.InvoiceTBName.Trim();
        public static string InvoicesTable => "Invoices";
        public async Task<DataTable> GetAll(DataAccessRepository daRpst)
        {
            try
            {
                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoicesTable + " ");
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
                //string query = $"LEFT JOIN "+ SqlConstants.InvoiceParameterValuesTBName + " cv ON inv.Id = cv.InvoiceId  LEFT JOIN "+ SqlConstants.InvoiceCustomParametersTBName +" cp ON cv.ParameterId = cp.Id";
                string query = $"where  inv.InvoiceNumber IS NOT NULL";

                if (generalFilter.Id > 0)
                {
                    query += $" and  inv.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  inv.MerchantId ='{generalFilter.MerchantId}'";
                }
                if ((int)generalFilter.InvoiceStatus > 0)
                {
                    query += $" and  inv.Status ='{(int)generalFilter.InvoiceStatus}'";
                }
                if (generalFilter.TotalAmount > 0)
                {
                    query += $" and  inv.TotalAmountDue ='{generalFilter.TotalAmount}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CurrencyCode))
                {
                    query += $" and  inv.CurrencyCode = '{generalFilter.CurrencyCode}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.InvoiceNumber))
                {
                    query += $" and  inv.InvoiceNumber = '{generalFilter.InvoiceNumber}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderReference))
                {
                    query += $" and  inv.OrderReference = '{generalFilter.OrderReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerName))
                {
                    query += $" and JSON_VALUE(inv.Customer,'$.customerName') LIKE '%{generalFilter.CustomerName}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerPhone))
                {
                    query += $" and JSON_VALUE(inv.Customer,'$.phone') = '{generalFilter.CustomerPhone}'";

                }
                if (!string.IsNullOrEmpty(generalFilter.Email))
                {
                    query += $" and JSON_VALUE(inv.Customer,'$.email') LIKE '%{generalFilter.Email}%'";

                }
                if (generalFilter.ProductId > 0)
                {
                    query += $" and JSON_VALUE(inv.Products,'$.id') = '{generalFilter.ProductId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( inv.InvoiceNumber LIKE N'%{searchKey}%' OR inv.OrderReference LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and  inv.CustomerId ='{generalFilter.CustomerId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderBy?.ToString()))
                {
                    if (!string.IsNullOrEmpty(generalFilter.OrderType?.ToString()))
                        query += $" ORDER BY " + "inv." + generalFilter.OrderBy + " " + generalFilter.OrderType;

                    else
                        query += $" ORDER BY " + "inv." + generalFilter.OrderBy + " ASC";

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
                string query = $"where  inv.Id = @id";
                //DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoicesTable + " Where Id = @id", param);
                DataTable dt = await daRpst.GetDataAsync(SelectTrans + " " + query, param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetByInvNo(string invNo, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@invoiceNo", invNo);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoicesTable + " Where InvoiceNumber = @invoiceNo", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetByToken(long payToken, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@payToken", payToken);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + InvoicesTable + " Where PaymentToken = @payToken And Status = 1", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(InvoiceModel mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + InvoicesTable + " (" + string.Join(",", ColumnsNames) + ") " +
                                         " Values(@" + string.Join(" ,@", ColumnsNames) + ") ", param.ToArray());

                if (id > 0)
                    isSuccess = true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return isSuccess;
        }

        public async Task<bool> Update(int id, InvoiceModel mdl, DataAccessRepository daRpst)
        {
            try
            {
                DateTime updatedAt = DateTime.Now;
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@amountRemain", mdl.AmountRemaining);
                param[2] = new SqlParameter("@amountPaid", mdl.AmountPaid);
                param[3] = new SqlParameter("@payMethod", mdl.PaymentMethods);
                param[4] = new SqlParameter("@status", (int)mdl.Status);
                param[5] = new SqlParameter("@updatedAt", DateTime.Now);

                var row = await daRpst.SetDataAsync("Update " + InvoicesTable + " Set AmountRemaining = @amountRemain, UpdatedAt = @updatedAt, AmountPaid = @amountPaid, PaymentMethods = @payMethod, Status = @status  Where Id = @id", param);
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

        public async Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst)
        {
            try
            {
                DateTime updatedAt = DateTime.Now;
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@Status", status);
                param[2] = new SqlParameter("@updatedAt", DateTime.Now);

                var row = await daRpst.SetDataAsync("Update " + InvoicesTable + " Set Status = @Status, UpdatedAt = @updatedAt Where Id = @id", param);
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
