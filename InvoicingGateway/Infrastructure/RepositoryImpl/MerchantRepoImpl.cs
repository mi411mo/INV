using Domain.Entities;
using Domain.IRepository.IMerchantRepository;
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
    public class MerchantRepoImpl : IMerchantRepo
    {
        private readonly DataAccessRepository dar;
        private IMerchantDao merchantDao;
        protected readonly InvoicingContext context;
        public MerchantRepoImpl(DataAccessRepository _dar, IMerchantDao merchantDao, InvoicingContext context)
        {
            dar = _dar;
            this.merchantDao = merchantDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(Merchant merchant)
        {
            try
            {
                long id = 0;
                var result = await merchantDao.InsertAsync(merchant, dar);
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
                var isSucceed = await merchantDao.Delete(id, dar);
                if (isSucceed)
                    return "Merchant was deleted successfully";
                else
                    return "Merchant was not deleted";

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
                var dt = await merchantDao.GetAll(dar);
                var lst = ConvertToModel<Merchant>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<Merchant>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            var oDataOptions = (ODataQueryOptions<Merchant>)_oDataOptions;
            var query = oDataOptions.ApplyTo(context.Merchants.AsQueryable()) as IQueryable<Merchant>;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Merchant> GetByIdAsync(int id)
        {
            try
            {
                var dt = await merchantDao.GetById(id, dar);
                var lst = ConvertToModel<Merchant>.DataTableToModels(dt);
                var merchantData = lst.FirstOrDefault();
                return merchantData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, Merchant merchant)
        {
            try
            {
                var result = await merchantDao.Update(id, merchant, dar);
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
                var isSucceed = await merchantDao.UpdateStatus(id, status, dar);
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
                var oDataOptions = (ODataQueryOptions<Merchant>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Merchants.AsQueryable()) as IQueryable<Merchant>;
                return await query.CountAsync();
            }
            return await context.Merchants.CountAsync();
        }

        public async Task<IList<Merchant>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await merchantDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<Merchant>.DataTableToModels(dt);
                /*if (!string.IsNullOrEmpty(generalFilter.CustomerId) && generalFilter.CustomerId != "0")
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
