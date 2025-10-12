using Application.Interfaces.IBusinessIndependentService.IServices;
using Domain.Entities;
using Domain.IRepository.ICategoryTypeRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils;
using Domain.Utils.Enums;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class CategoryTypeServiceImpl : ICategoryTypeService
    {
        private readonly ICategoryTypeRepo categoryRepo;
        public CategoryTypeServiceImpl(ICategoryTypeRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(CategoryType category)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                /*var customers = await categoryRepo.GetAllAsync();
                if (customers.Any(x => x.Name == category.Name))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.CATEGORY_NAME_IS_DUPLICATED);
                }*/
                res = await categoryRepo.AddAsync(category);

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
                var deleteRecord = await categoryRepo.DeleteAsync(id);
                return deleteRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<CategoryType>> GetAllAsync()
        {
            try
            {
                var lst = await categoryRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<CategoryType>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await categoryRepo.GetAllAsync(generalFilter);
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
                var lst = await categoryRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ServiceResult<PagedListDto<CategoryType>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryType> GetByIdAsync(int id)
        {
            try
            {
                var customerData = await categoryRepo.GetByIdAsync(id);
                return customerData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, CategoryType category)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await categoryRepo.UpdateAsync(id, category);
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
                var updateStatus = await categoryRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

       
    }
}
