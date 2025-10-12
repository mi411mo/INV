using Application.IServices;
using Domain.Entities;
using Domain.IRepository.IServiceRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepo productRepo;
        public ProductServiceImpl(IProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(ProductModel serviceModel)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await productRepo.AddAsync(serviceModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var deleteRecord = await productRepo.DeleteAsync(id);
                return deleteRecord;
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
                var lst = await productRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await productRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult<PagedListDto<ProductModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            try
            {
                var result = await productRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);
                var res = PagedListDto<ProductModel>.ToPagedList(result, await productRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception ex)
            {
                return ServiceResult.Failed<PagedListDto<ProductModel>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            try
            {
                var productData = await productRepo.GetByIdAsync(id);
                return productData;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, ProductModel serviceModel)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await productRepo.UpdateAsync(id, serviceModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return res;
        }

        public async Task<string> UpdateStatusAsync(int id, int status)
        {
            try
            {
                var updateStatus = await productRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
