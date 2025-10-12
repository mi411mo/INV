using Domain.Entities;
using Domain.IRepository;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using Infrastructure.Repositories.Impl.V1.DRY;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DAOs.IDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl
{
    public class RequestPaymentRepoImpl : IRequestPaymentRepo
    {
        private readonly DataAccessRepository dar;
        private IPaymentRequestDao requestDao;
        public RequestPaymentRepoImpl(DataAccessRepository _dar, IPaymentRequestDao requestDao)
        {
            dar = _dar;
            this.requestDao = requestDao;
        }
        public async Task<ResultRepo<object>> AddAsync(RequestPayment payModel)
        {
            try
            {
                var result = await requestDao.InsertAsync(payModel, dar);
                return ReturnRepo.Success(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<RequestPayment>> GetAllAsync()
        {
            try
            {
                var dt = await requestDao.GetAll(dar);
                var lst = ConvertToModel<RequestPayment>.DataTableToModels(dt);
                return lst;
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
                var dt = await requestDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<RequestPayment>.DataTableToModels(dt);
                return lst;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<RequestPayment>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<RequestPayment> GetByIdAsync(int id)
        {
            try
            {
                var dt = await requestDao.GetById(id, dar);
                var lst = ConvertToModel<RequestPayment>.DataTableToModels(dt);
                var payData = lst.FirstOrDefault();
                return payData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<RequestPayment> GetByPayRefAsync(string payRef)
        {
            try
            {
                var dt = await requestDao.GetByPayRef(payRef, dar);
                var lst = ConvertToModel<RequestPayment>.DataTableToModels(dt);
                var payData = lst.FirstOrDefault();
                return payData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<long> GetCountAsync(object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultRepo<object>> UpdateAsync(int id, RequestPayment payModel)
        {
            try
            {
                var isSucceed = await requestDao.Update(id, payModel, dar);
                return ReturnRepo.Success(isSucceed);

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
                var isSucceed = await requestDao.UpdateStatus(id, status, dar);
                return isSucceed;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
