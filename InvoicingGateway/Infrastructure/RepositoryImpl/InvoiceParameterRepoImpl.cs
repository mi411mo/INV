using Domain.Entities;
using Domain.IRepository;
using Domain.IRepository.ICategoryTypeRepository;
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
    public class InvoiceParameterRepoImpl : IInvoiceParameterRepo
    {
        private readonly DataAccessRepository dar;
        private IInvoiceParameterDao parameterDao;
        public InvoiceParameterRepoImpl(DataAccessRepository _dar, IInvoiceParameterDao parameterDao)
        {
            dar = _dar;
            this.parameterDao = parameterDao;
        }
        public async Task<ResultRepo<object>> AddAsync(InvoiceCustomParameter customParameter)
        {
            try
            {
                long id = 0;
                var result = await parameterDao.InsertAsync(customParameter, dar);
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
                var isSucceed = await parameterDao.Delete(id, dar);
                if (isSucceed)
                    return "Invoice Custom Parameter deleted successfully";
                else
                    return "Invoice Custom Parameter was not deleted";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceCustomParameter>> GetAllAsync()
        {
            try
            {
                var dt = await parameterDao.GetAll(dar);
                var lst = ConvertToModel<InvoiceCustomParameter>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<InvoiceCustomParameter>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await parameterDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<InvoiceCustomParameter>.DataTableToModels(dt);
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

        public Task<IReadOnlyList<InvoiceCustomParameter>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<InvoiceCustomParameter> GetByIdAsync(int id)
        {
            try
            {
                var dt = await parameterDao.GetById(id, dar);
                var lst = ConvertToModel<InvoiceCustomParameter>.DataTableToModels(dt);
                var categoryData = lst.FirstOrDefault();
                return categoryData;
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

        public async Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomParameter customParameter)
        {
            try
            {
                var result = await parameterDao.Update(id, customParameter, dar);
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
                var isSucceed = await parameterDao.UpdateStatus(id, status, dar);
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
    }
}
