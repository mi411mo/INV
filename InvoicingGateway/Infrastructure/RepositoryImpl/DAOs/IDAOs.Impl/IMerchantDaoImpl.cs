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
    public class IMerchantDaoImpl : IMerchantDao
    {
        private string SelectTrans { get; set; } = "Select *  from " + SqlConstants.MerchnatTBName + $" where  ArabicName IS NOT NULL";
        internal static string _MerchnatTBName = SqlConstants.MerchnatTBName.Trim();
        public static string MerchantTable => "Merchants";
        public async Task<bool> Delete(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                var row = await daRpst.SetDataAsync("Delete From " + MerchantTable + "  Where Id = @id", param);
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
                DataTable dt = await daRpst.GetDataAsync("Select * from " + MerchantTable + " ");
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
                    query += $" and  {_MerchnatTBName}.Id ='{generalFilter.Id}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.IsActive.ToString()))
                {
                    query += $" and  {_MerchnatTBName}.IsActive ='{generalFilter.IsActive}'";
                }
                if (generalFilter.MerchantId > 0)
                {
                    query += $" and  {_MerchnatTBName}.Id ='{generalFilter.MerchantId}'";
                }
                if (generalFilter.CategoryType > 0)
                {
                    query += $" and  {_MerchnatTBName}.CategoryType ='{generalFilter.CategoryType}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.ArabicName))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.ArabicName.Trim());
                    query += $" and  {_MerchnatTBName}.ArabicName LIKE N'%{name}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.CustomerId?.ToString()) && generalFilter.CustomerId?.ToString() != "0")
                {
                    query += $" and  {_MerchnatTBName}.ProfileId ='{generalFilter.CustomerId}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.EnglishName))
                {
                    query += $" and  {_MerchnatTBName}.EnglishName LIKE '%{generalFilter.EnglishName}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.Name))
                {
                    var name = HttpUtility.UrlDecode(generalFilter.Name.Trim());
                    query += $" and  {_MerchnatTBName}.ArabicName LIKE N'%{name}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.Phone))
                {
                    query += $" and  {_MerchnatTBName}.Phone ='{generalFilter.Phone}'";
                }
                if (!string.IsNullOrEmpty(generalFilter.Email))
                {
                    query += $" and  {_MerchnatTBName}.Email LIKE '%{generalFilter.Email}%'";
                }
              
                if (!string.IsNullOrEmpty(generalFilter.Address))
                {
                    var address = HttpUtility.UrlDecode(generalFilter.Address.Trim());
                    query += $" and  {_MerchnatTBName}.Address LIKE N'%{address}%'";
                }
                if (!string.IsNullOrEmpty(generalFilter.SearchKey))
                {
                    var searchKey = HttpUtility.UrlDecode(generalFilter.SearchKey.Trim());
                    query += $" and ( {_MerchnatTBName}.Email LIKE N'%{searchKey}%' OR {_MerchnatTBName}.ArabicName LIKE N'%{searchKey}%' OR {_MerchnatTBName}.Phone LIKE N'%{searchKey}%'  OR {_MerchnatTBName}.EnglishName LIKE '%{searchKey}%')";
                }
                if (!string.IsNullOrEmpty(generalFilter.ProfileId))
                {
                    query += $" and  {_MerchnatTBName}.ProfileId ='{generalFilter.ProfileId}'";
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

        public async Task<DataTable> GetById(int id, DataAccessRepository daRpst)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@id", id);

                DataTable dt = await daRpst.GetDataAsync("Select * from " + MerchantTable + " Where Id = @id", param);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Merchant mdl, DataAccessRepository daRpst)
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

                var id = await daRpst.SetDataInsertAsync("Insert into " + MerchantTable + " (" + string.Join(",", ColumnsNames) + ") " +
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

        public async Task<bool> Update(int id, Merchant mdl, DataAccessRepository daRpst)
        {
            bool isSuccess = false;
            try
            {
                SqlParameter[] param = new SqlParameter[19];
                param[0] = new SqlParameter("@id", id);
                param[1] = new SqlParameter("@arName", mdl.ArabicName);
                param[2] = new SqlParameter("@enName", mdl.EnglishName);
                param[3] = new SqlParameter("@email", mdl.Email);
                param[4] = new SqlParameter("@phone", mdl.Phone);
                param[5] = new SqlParameter("@address", mdl.Address);
                param[6] = new SqlParameter("@prefix", mdl.InvoicePrefix);
                param[7] = new SqlParameter("@status", mdl.IsActive);
                param[8] = new SqlParameter("@details", mdl.Details);
                param[9] = new SqlParameter("@profileId", mdl.ProfileId);
                param[10] = new SqlParameter("@categoryType", mdl.CategoryType);
                param[11] = new SqlParameter("@logoImageUrl", mdl.LogoImageUrl);
                param[12] = new SqlParameter("@businessDescription", mdl.BusinessDescription);
                param[13] = new SqlParameter("@websiteUrl", mdl.WebsiteUrl);
                param[14] = new SqlParameter("@socialMedia", mdl.SocialMedia);
                param[15] = new SqlParameter("@storeLocation", mdl.StoreLocation);
                param[16] = new SqlParameter("@operatingHours", mdl.OperatingHours);
                param[17] = new SqlParameter("@customerReviews", mdl.CustomerReviews);
                param[18] = new SqlParameter("@markdownContent", mdl.MarkdownContent);

                int row = await daRpst.SetDataAsync("Update " + MerchantTable + " SET ArabicName=@arName, EnglishName=@enName, CategoryType=@categoryType, LogoImageUrl=@logoImageUrl, Address=@address, Email=@email, Phone=@phone, InvoicePrefix=@prefix, " +
                                                "IsActive=@status, Details=@details, ProfileId=@profileId, BusinessDescription=@businessDescription, WebsiteUrl=@websiteUrl, SocialMedia=@socialMedia, StoreLocation=@storeLocation, OperatingHours=@operatingHours, CustomerReviews=@customerReviews, MarkdownContent=@markdownContent  Where Id=@id", param);
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

                var row = await daRpst.SetDataAsync("Update " + MerchantTable + " Set IsActive = @status Where Id = @id", param);
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
