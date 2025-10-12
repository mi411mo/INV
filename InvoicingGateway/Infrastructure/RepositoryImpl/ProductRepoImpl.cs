using Application.Utils.CustomException;
using Domain.Entities;
using Domain.IRepository.IServiceRepository;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using Infrastructure.Context;
using Infrastructure.Repositories.Impl.V1.DRY;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DAOs.IDAOs;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl
{
    public class ProductRepoImpl : IProductRepo
    {
        private readonly DataAccessRepository dar;
        private IProductDao productDao;
        protected readonly InvoicingContext context;
        public ProductRepoImpl(DataAccessRepository _dar, IProductDao productDao, InvoicingContext context)
        {
            dar = _dar;
            this.productDao = productDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(ProductModel serviceModel)
        {
            try
            {
                long id = 0;
                var result = await productDao.InsertAsync(serviceModel, dar);
                return ReturnRepo.Success(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var isSucceed = await productDao.Delete(id, dar);
                if (isSucceed)
                    return "Product was deleted successfully";
                else
                    return "Product was not deleted";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<ProductModel>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await productDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<ProductModel>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<ProductModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            var oDataOptions = (ODataQueryOptions<ProductModel>)_oDataOptions;
            var query = oDataOptions.ApplyTo(context.Products.AsQueryable()) as IQueryable<ProductModel>;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                 .OrderByDescending(e => e.Name)
                .ToListAsync();
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            try
            {
                var dt = await productDao.GetById(id, dar);
                var lst = ConvertToModel<ProductModel>.DataTableToModels(dt);
                var productData = lst.FirstOrDefault();
                return productData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, ProductModel serviceModel)
        {
            try
            {
                var result = await productDao.Update(id, serviceModel, dar);
                return ReturnRepo.Success(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> UpdateStatus(int id, int status)
        {
            try
            {
                var isSucceed = await productDao.UpdateStatus(id, status, dar);
                if (isSucceed)
                    return "Status was updated successfully";
                else
                    return "Status was not updated";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetCountAsync(object _oDataOptions)
        {
            if (_oDataOptions != null)
            {
                var oDataOptions = (ODataQueryOptions<ProductModel>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Products.AsQueryable()) as IQueryable<ProductModel>;
                return await query.CountAsync();
            }
            return await context.Products.CountAsync();
        }
    }
}
