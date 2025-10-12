using Application.IServices;
using Domain.Entities;
using Domain.IRepository.ICustomerRepository;
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
    public class CustomerServiceImpl : ICustomerService
    {
        private readonly ICustomerRepo customerRepo;
        public CustomerServiceImpl(ICustomerRepo customerRepo)
        {
            this.customerRepo = customerRepo;
        }
        public async Task<ResultRepo<object>> AddAsync(Customer customer)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                var customers = await customerRepo.GetAllAsync();
                if(customers.Any(x=>x.Phone == customer.Phone))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.PHONE_IS_DUPLICATED);
                }
                /*if (customers.Any(x => x.Email == customer.Email))
                {
                    return new ResultRepo<object>().setReturnResponse(ReturnStatusEnum.Failed, Constants.EMAIL_IS_DUPLICATED);
                }*/
                res = await customerRepo.AddAsync(customer);

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
                var deleteRecord = await customerRepo.DeleteAsync(id);
                return deleteRecord;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Customer>> GetAllAsync()
        {
            try
            {
                var lst = await customerRepo.GetAllAsync();
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ServiceResult<PagedListDto<Customer>>> GetAllAsync(PaginationFilter filter, object oDataOptions)
        {
            try
            {
                var result = await customerRepo.GetAllAsync(filter.PageNumber, filter.PageSize, oDataOptions);
                var res = PagedListDto<Customer>.ToPagedList(result, await customerRepo.GetCountAsync(oDataOptions), filter.PageNumber, filter.PageSize);
                return ServiceResult.Success(res);
            }
            catch (Exception)
            {
                return ServiceResult.Failed<PagedListDto<Customer>>(ServiceError.ServiceProvider);
            }
        }

        public async Task<IList<Customer>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await customerRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                var customerData = await customerRepo.GetByIdAsync(id);
                return customerData;
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
                var lst = await customerRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Customer customer)
        {
            ResultRepo<object> res = new ResultRepo<object>();
            try
            {
                res = await customerRepo.UpdateAsync(id, customer);
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
                var updateStatus = await customerRepo.UpdateStatus(id, status);
                return updateStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
