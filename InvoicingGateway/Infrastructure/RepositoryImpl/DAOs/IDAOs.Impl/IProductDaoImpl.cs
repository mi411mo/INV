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
    public class IProductDaoImpl : IProductDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.ProductTBName + $" where  Name IS NOT NULL";
        internal static string _ProductTBName = SqlConstants.ProductTBName.Trim();
        public static string ProductsTable => "Products";

        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + ProductsTable + "  Where Id = @id", param);
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

        public async Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst)
        {
            try
            {
                string query = string.Empty;
                if (generalFilter.Id > 0)
                {
                    query += $" and  {_ProductTBName}.Id ='{generalFilter.Id}'";
                }
                if (generalFilter.ProductId > 0)
                {
                    query += $" and  {_ProductTBName}.Id ='{generalFilter.ProductId}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_ProductTBName}.MerchantId ='{generalFilter.MerchantId}'";
                }
                if (generalFilter.CategoryType > 0)
                {
                    query += $" and  {_ProductTBName}.CategoryType ='{generalFilter.CategoryType}'";
                }
                if (generalFilter.TotalAmount > 0)
                {
                    query += $" and  {_ProductTBName}.TotalAmount ='{generalFilter.TotalAmount}'";
                }
                if (generalFilter.Status > 0)
                {
                    query += $" and  {_ProductTBName}.Status ='{generalFilter.Status}'";
                }
                if (generalFilter.MaxPrice > 0 && generalFilter.MinPrice > 0)
                {
                    query += $" and  {_ProductTBName}.UnitPrice BETWEEN '{generalFilter.MinPrice}' AND '{generalFilter.MaxPrice}'";
                }
                if (generalFilter.MinPrice > 0)
                {
                    query += $" and {_ProductTBName}.UnitPrice >='{generalFilter.MinPrice}'";
                }
                if (generalFilter.MaxPrice > 0)
                {
                    query += $" and  {_ProductTBName}.UnitPrice <='{generalFilter.MaxPrice}'";
                }
                if (generalFilter.CategoryId > 0)
                {
                    query += $" and  {_ProductTBName}.CategoryId ='{generalFilter.CategoryId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CategoryIds))
                {
                    int[] catIds = generalFilter.CategoryIds.Split("%2c").Select(int.Parse).ToArray();
                    query += $" and  {_ProductTBName}.CategoryId IN ({string.Join(",", catIds)})";
                }
                if (!string.IsNullOrEmpty(generalFilter.CurrencyCode))
                {
                    query += $" and  {_ProductTBName}.CurrencyCode ='{generalFilter.CurrencyCode}'";
                } 
                if (!string.IsNullOrEmpty(generalFilter.Name))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.Name.Trim());
                    query += $" and  {_ProductTBName}.Name LIKE N'%{name}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_ProductTBName}.Name LIKE N'%{searchKey}%' OR {_ProductTBName}.CurrencyCode LIKE N'%{searchKey}%' OR {_ProductTBName}.CurrencyCode LIKE N'%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and   {_ProductTBName}.CustomerId ='{generalFilter.CustomerId}'";
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

                DataTable dt = await daRpst.GetDataAsync("Select * from " + ProductsTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(ProductModel mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + ProductsTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, ProductModel mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[14];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@name", mdl.Name);
                param[2] = new SqlParameter("@unitPrice", mdl.UnitPrice);
                param[3] = new SqlParameter("@discount", mdl.Discount);
                param[4] = new SqlParameter("@quantity", mdl.Quantity);
                param[5] = new SqlParameter("@totalAmount", mdl.TotalAmount);
                param[6] = new SqlParameter("@currency", mdl.CurrencyCode);
                param[7] = new SqlParameter("@category", mdl.CategoryId);
                param[8] = new SqlParameter("@subCat", mdl.SubCategoryId);
                param[9] = new SqlParameter("@status", mdl.Status);
                param[10] = new SqlParameter("@description", mdl.Description);
                param[11] = new SqlParameter("@merchantId", mdl.MerchantId);
                param[12] = new SqlParameter("@image", mdl.ImageURL);
                param[13] = new SqlParameter("@categoryType", mdl.CategoryType);


                int row = await daRpst.SetDataAsync("Update " + ProductsTable + " SET Name=@name, UnitPrice=@unitPrice, Discount=@discount, Quantity =@quantity, MerchantId =@merchantId, ImageURL =@image, " +
                                                "TotalAmount=@totalAmount, CurrencyCode=@currency, CategoryId=@category, CategoryType=@categoryType, SubCategoryId=@subCat, Status=@status, Description=@description Where Id=@id", param);
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
                param[1] = new SqlParameter("@Status", status);

                var row = await daRpst.SetDataAsync("Update " + ProductsTable + " Set Status = @Status Where Id = @id", param);
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
