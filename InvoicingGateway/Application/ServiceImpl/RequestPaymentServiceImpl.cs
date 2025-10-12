using Application.Interfaces.IBusinessIndependentService.IServices;
using Domain.Entities;
using Domain.IRepository;
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
    public class RequestPaymentServiceImpl : IRequestPaymentService
    {
        private readonly IRequestPaymentRepo reqPayRepo;
        private readonly IMerchantRepo merchantRepo;
        public RequestPaymentServiceImpl(IRequestPaymentRepo reqPayRepo, IMerchantRepo merchantRepo)
        {
            this.reqPayRepo = reqPayRepo;
            this.merchantRepo = merchantRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(RequestPayment requestPay)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                var merchants = await merchantRepo.GetAllAsync();

                if (!merchants.Any(x => x.Id == requestPay.MerchantId))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_AVAILABE);
                }
                if (merchants.Any(x => x.Id == requestPay.MerchantId && x.IsActive == false))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.MERCHANT_NOT_ACTIVE);
                }

                return await reqPayRepo.AddAsync(requestPay);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IList<RequestPayment>> GetAllAsync()
        {
            try
            {
                var payLst = await reqPayRepo.GetAllAsync();
                return payLst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<RequestPayment>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await reqPayRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .OrderByDescending(e => e.Id)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ServiceResult<PagedListDto<RequestPayment>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestPayment> GetByIdAsync(int id)
        {
            try
            {
                var payData = await reqPayRepo.GetByIdAsync(id);
                return payData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RequestPayment> GetByPayRef(string payRef)
        {
            try
            {
                var payData = await reqPayRepo.GetByPayRefAsync(payRef);
                return payData;
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
                var lst = await reqPayRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, RequestPayment requestPay)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await reqPayRepo.UpdateAsync(id, requestPay);
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }

        public async Task<bool> UpdateStatusAsync(int id, int status)
        {
            try
            {
                var isUpdated = await reqPayRepo.UpdateStatus(id, status);
                return isUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
