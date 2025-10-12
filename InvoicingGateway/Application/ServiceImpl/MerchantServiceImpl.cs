using Application.IServices;
using Domain.Entities;
using Domain.IRepository.IMerchantRepository;
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
    public class MerchantServiceImpl : IMerchantService
    {
        private readonly IMerchantRepo merchantRepo;
        public MerchantServiceImpl(IMerchantRepo merchantRepo)
        {
            this.merchantRepo = merchantRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(Merchant merchant)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                var merchants = await merchantRepo.GetAllAsync();
                if (merchants.Any(x => x.Id == merchant.Id))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.ID_IS_DUPLICATED);
                }
                if (merchants.Any(x => x.Phone == merchant.Phone))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.PHONE_IS_DUPLICATED);
                }
                if (merchants.Any(x => x.Email == merchant.Email))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.EMAIL_IS_DUPLICATED);
                }
                if (merchants.Any(x => x.Email == merchant.ProfileId))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.PROFILE_ID_IS_DUPLICATED);
                }
                res = await merchantRepo.AddAsync(merchant);

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
                var deleteRecord = await merchantRepo.DeleteAsync(id);
                return deleteRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Merchant>> GetAllAsync()
        {
            try
            {
                var lst = await merchantRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult<PagedListDto<Merchant>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            try
            {
                var result = await merchantRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);
                var res = PagedListDto<Merchant>.ToPagedList(result, await merchantRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult.Failed<PagedListDto<Merchant>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<IList<Merchant>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await merchantRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<Merchant> GetByIdAsync(int id)
        {
            try
            {
                var merchantData = await merchantRepo.GetByIdAsync(id);
                return merchantData;
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
                var merchants = await merchantRepo.GetAllAsync(generalFilter);
                return merchants.Count();
            }
            catch (Exception ex)
            {
               
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Merchant merchant)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await merchantRepo.UpdateAsync(id, merchant);
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
                var updateStatus = await merchantRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
