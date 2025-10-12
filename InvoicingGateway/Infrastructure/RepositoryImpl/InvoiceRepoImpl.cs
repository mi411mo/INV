using Application.Utils.CustomException;
using Domain.Entities;
using Domain.IRepository.IInvoiceRepository;
using Domain.Models.Base;
using Domain.Models.Invoices.ResponseDto;
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
    public class InvoiceRepoImpl : IInvoiceRepo
    {
        private readonly DataAccessRepository dar;
        private IInvoiceDao invoiceDao;
        protected readonly InvoicingContext context;
        public InvoiceRepoImpl(DataAccessRepository _dar, IInvoiceDao invoiceDao,  InvoicingContext context)
        {
            dar = _dar;
            this.invoiceDao = invoiceDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(InvoiceModel invoiceModel)
        {
            try
            {
                var result = await invoiceDao.InsertAsync(invoiceModel, dar);
                return ReturnRepo.Success(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceModel>> GetAllAsync()
        {
            try
            {
                var dt = await invoiceDao.GetAll(dar);
                var lst = ConvertToModel<InvoiceModel>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<InvoiceModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            try
            {
                var oDataOptions = (ODataQueryOptions<InvoiceModel>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Invoices.AsQueryable()) as IQueryable<InvoiceModel>;

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByIdAsync(int id)
        {
            try
            {
                var dt = await invoiceDao.GetById(id, dar);
                var lst = ConvertToModel<InvoiceModel>.DataTableToModels(dt);
                var invoiceData = lst.FirstOrDefault();
                return invoiceData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByInvNoAsync(string invNo)
        {
            try
            {
                var dt = await invoiceDao.GetByInvNo(invNo, dar);
                var lst = ConvertToModel<InvoiceModel>.DataTableToModels(dt);
                var invoiceData = lst.FirstOrDefault();
                return invoiceData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InvoiceModel> GetByTokenAsync(long payToken)
        {
            try
            {
                var dt = await invoiceDao.GetByToken(payToken, dar);
                var lst = ConvertToModel<InvoiceModel>.DataTableToModels(dt);
                var invoiceData = lst.FirstOrDefault();
                return invoiceData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateStatus(int id, int status)
        {
            try
            {
                var isSucceed = await invoiceDao.UpdateStatus(id, status, dar);
                return isSucceed;

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
                var oDataOptions = (ODataQueryOptions<InvoiceModel>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Invoices.AsQueryable()) as IQueryable<InvoiceModel>;
                return await query.CountAsync();
            }
            return await context.Invoices.CountAsync();
        }

        public Task<bool> AddByOrderRefAsync(string orederRef)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, InvoiceModel invoiceModel)
        {
            try
            {
                var isSucceed = await invoiceDao.Update(id, invoiceModel, dar);
                return ReturnRepo.Success(isSucceed);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceModel>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await invoiceDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<InvoiceModel>.DataTableToModels(dt);

               /* if (!string.IsNullOrEmpty(generalFilter.CustomerId) && generalFilter.CustomerId != "0")
                {
                    lst = lst.Where(e => e.CustomerId == generalFilter.CustomerId).ToList();
                }*/
                return lst;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
