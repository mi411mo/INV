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
    public class IPaymentDaoImpl : IPaymentDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.PaymentTBName + $" where  PaymentReference IS NOT NULL";
        internal static string _PaymentTBName = SqlConstants.PaymentTBName.Trim();
        public static string PaymentsTable => "Payments";
        
        public async Task<DataTable> GetAll(DataAccessRepository daRpst)
        {
            try
            {
                DataTable dt = await daRpst.GetDataAsync("Select * from " + PaymentsTable + " ");
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
                    query += $" and  {_PaymentTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_PaymentTBName}.MerchantId ='{generalFilter.MerchantId}'";
                }
                if ((int)generalFilter.PaymentStatus > 0)
                {
                    query += $" and  {_PaymentTBName}.PaymentStatus ='{(int)generalFilter.PaymentStatus}'";
                }
                if ((int)generalFilter.PaymentType > 0)
                {
                    query += $" and  {_PaymentTBName}.PaymentType ='{(int)generalFilter.PaymentType}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.PaymentReference))
                {
                    query += $" and  {_PaymentTBName}.PaymentReference = '{generalFilter.PaymentReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.TargetReference))
                {
                    query += $" and  {_PaymentTBName}.TargetReference = '{generalFilter.TargetReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.TargetReference))
                {
                    query += $" and  {_PaymentTBName}.TargetReference = '{generalFilter.TargetReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.RequestPaymentReference))
                {
                    query += $" and  {_PaymentTBName}.RequestPaymentReference = '{generalFilter.RequestPaymentReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.InvoiceNumber))
                {
                    query += $" and  {_PaymentTBName}.InvoiceReference = '{generalFilter.InvoiceNumber}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.InvoiceReference))
                {
                    query += $" and  {_PaymentTBName}.InvoiceReference = '{generalFilter.InvoiceReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.PaymentMethod))
                {
                    query += $" and  {_PaymentTBName}.PaymentMethod = '{generalFilter.PaymentMethod}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.OrderReference))
                {
                    query += $" and  {_PaymentTBName}.OrderReference = '{generalFilter.OrderReference}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_PaymentTBName}.TransactionReference LIKE N'%{searchKey}%' OR {_PaymentTBName}.InvoiceReference LIKE N'%{searchKey}%' OR {_PaymentTBName}.PaymentMethod LIKE N'%{searchKey}%' OR {_PaymentTBName}.OrderReference LIKE N'%{searchKey}%' OR {_PaymentTBName}.PaymentReference LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_PaymentTBName}.CustomerId ='{generalFilter.CustomerId}'";
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

        public async Task<DataTable> GetPaymentById(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + PaymentsTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetPaymentByInvRef(string invoiceNo, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@invNo", invoiceNo);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + PaymentsTable + " Where InvoiceReference = @invNo", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetPaymentByRef(string payRef, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@payRef", payRef);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + PaymentsTable + " Where PaymentReference = @payRef", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Payment mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + PaymentsTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, Payment mdl, DataAccessRepository daRpst)
        {
            try
            {
                DateTime updatedAt = DateTime.Now;
                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@Status", mdl.PaymentStatus);
                param[2] = new SqlParameter("@payMethod", mdl.PaymentMethod);
                param[3] = new SqlParameter("@transRef", mdl.TransactionReference);
                param[4] = new SqlParameter("@targetRef", mdl.TargetReference);
                param[5] = new SqlParameter("@payDate", mdl.PayDate);

                var row = await daRpst.SetDataAsync("Update " + PaymentsTable + " Set PaymentStatus = @Status, PaymentMethod = @payMethod, TransactionReference = @transRef, TargetReference = @targetRef, PayDate = @payDate Where Id = @id", param);
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
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@Status", status);
                param[2] = new SqlParameter("@payDate", DateTime.Now);

                var row = await daRpst.SetDataAsync("Update " + PaymentsTable + " Set PaymentStatus = @Status, PayDate = @payDate  Where Id = @id", param);
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
