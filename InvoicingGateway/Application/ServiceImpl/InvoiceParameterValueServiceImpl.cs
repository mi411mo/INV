using Application.Interfaces.IBusinessIndependentService.IServices;
using Domain.Entities;
using Domain.IRepository;
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
    public class InvoiceParameterValueServiceImpl : IInvoiceParameterValueService
    {
        private readonly IInvoiceParameterValueRepo parameterRepo;
        public InvoiceParameterValueServiceImpl(IInvoiceParameterValueRepo parameterRepo)
        {
            this.parameterRepo = parameterRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(InvoiceCustomValue customParameter)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                var customers = await parameterRepo.GetAllAsync();
                if (customers.Any(x => x.InvoiceId == customParameter.InvoiceId && x.ParameterId == customParameter.ParameterId))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.PARAMETER_VALUE_IS_DUPLICATED);
                }
                res = await parameterRepo.AddAsync(customParameter);

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
                var deleteRecord = await parameterRepo.DeleteAsync(id);
                return deleteRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceCustomValue>> GetAllAsync()
        {
            try
            {
                var lst = await parameterRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceCustomValue>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await parameterRepo.GetAllAsync(generalFilter);
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
        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await parameterRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ServiceResult<PagedListDto<InvoiceCustomValue>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<InvoiceCustomValue> GetByIdAsync(int id)
        {
            try
            {
                var customerData = await parameterRepo.GetByIdAsync(id);
                return customerData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomValue customParameter)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await parameterRepo.UpdateAsync(id, customParameter);
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
                var updateStatus = await parameterRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
