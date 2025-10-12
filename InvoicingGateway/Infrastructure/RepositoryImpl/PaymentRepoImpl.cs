using Domain.Entities;
using Domain.IRepository.IPaymentRepository;
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
    public class PaymentRepoImpl : IPaymentRepo
    {
        private readonly DataAccessRepository dar;
        private IPaymentDao paymentDao;
        protected readonly InvoicingContext context;
        public PaymentRepoImpl(DataAccessRepository _dar, IPaymentDao paymentDao, InvoicingContext context)
        {
            dar = _dar;
            this.paymentDao = paymentDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(Payment payment)
        {
            try
            {
                long id = 0;
                var result = await paymentDao.InsertAsync(payment, dar);
                return ReturnRepo.Success(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ResultRepo<object>> CancelOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Payment>> GetAllAsync()
        {
            try
            {
                var dt = await paymentDao.GetAll(dar);
                var lst = ConvertToModel<Payment>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<Payment>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            var oDataOptions = (ODataQueryOptions<Payment>)_oDataOptions;
            var query = oDataOptions.ApplyTo(context.Payments.AsQueryable()) as IQueryable<Payment>;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<long> GetCountAsync(object _oDataOptions)
        {
            if (_oDataOptions != null)
            {
                var oDataOptions = (ODataQueryOptions<Payment>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Payments.AsQueryable()) as IQueryable<Payment>;
                return await query.CountAsync();
            }
            return await context.Payments.CountAsync();
        }

        public async Task<Payment> GetPaymentByIdAsync(int id)
        {
            try
            {
                var dt = await paymentDao.GetPaymentById(id, dar);
                var lst = ConvertToModel<Payment>.DataTableToModels(dt);
                var paymentData = lst.FirstOrDefault();
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Payment> GetPaymentByInvAsync(string invoiceNo)
        {
            try
            {
                var dt = await paymentDao.GetPaymentByInvRef(invoiceNo, dar);
                var lst = ConvertToModel<Payment>.DataTableToModels(dt);
                var paymentData = lst.FirstOrDefault();
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Payment payment)
        {
            try
            {
                var isSucceed = await paymentDao.Update(id, payment, dar);
                return ReturnRepo.Success(isSucceed);
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
                var isSucceed = await paymentDao.UpdateStatus(id, status, dar);
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

        public async Task<Payment> GetPaymentByRefAsync(string payRef)
        {
            try
            {
                var dt = await paymentDao.GetPaymentByRef(payRef, dar);
                var lst = ConvertToModel<Payment>.DataTableToModels(dt);
                var paymentData = lst.FirstOrDefault();
                return paymentData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Payment>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await paymentDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<Payment>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
