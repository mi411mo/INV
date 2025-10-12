using Domain.Entities;
using Domain.IRepository.ICustomerRepository;
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
    public class CustomerRepoImpl : ICustomerRepo
    {
        private readonly DataAccessRepository dar;
        private ICustomerDao CustomerDao;
        protected readonly InvoicingContext context;
        public CustomerRepoImpl(DataAccessRepository _dar, ICustomerDao CustomerDao, InvoicingContext context)
        {
            dar = _dar;
            this.CustomerDao = CustomerDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(Customer customer)
        {
            try
            {
                long id = 0;
                var result = await CustomerDao.InsertAsync(customer, dar);
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
                var isSucceed = await CustomerDao.Delete(id, dar);
                if (isSucceed)
                    return "CustomerInfo was deleted successfully";
                else
                    return "CustomerInfo was not deleted";

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
                var dt = await CustomerDao.GetAll(dar);
                var lst = ConvertToModel<Customer>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            var oDataOptions = (ODataQueryOptions<Customer>)_oDataOptions;
            var query = oDataOptions.ApplyTo(context.Customers.AsQueryable()) as IQueryable<Customer>;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                var dt = await CustomerDao.GetById(id, dar);
                var lst = ConvertToModel<Customer>.DataTableToModels(dt);
                var customerData = lst.FirstOrDefault();
                return customerData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Customer customer)
        {
            try
            {
                var result = await CustomerDao.Update(id, customer, dar);
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
                var isSucceed = await CustomerDao.UpdateStatus(id, status, dar);
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
                var oDataOptions = (ODataQueryOptions<Customer>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Customers.AsQueryable()) as IQueryable<Customer>;
                return await query.CountAsync();
            }
            return await context.Customers.CountAsync();
        }

        public async Task<IList<Customer>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await CustomerDao.GetAll(generalFilter, dar);
                var customers = ConvertToModel<Customer>.DataTableToModels(dt);
               /* if (!string.IsNullOrEmpty(generalFilter.CustomerId) && generalFilter.CustomerId != "0")
                {
                    customers = customers.Where(e => e.CustomerId == generalFilter.CustomerId).ToList();
                }*/
                return customers;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
