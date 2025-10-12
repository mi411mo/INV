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
    public class InvoiceParameterValueRepoImpl : IInvoiceParameterValueRepo
    {
        private readonly DataAccessRepository dar;
        private IInvoiceParameterValueDao parameterValueDao;
        public InvoiceParameterValueRepoImpl(DataAccessRepository _dar, IInvoiceParameterValueDao parameterValueDao)
        {
            dar = _dar;
            this.parameterValueDao = parameterValueDao;
        }
        public async Task<ResultRepo<object>> AddAsync(InvoiceCustomValue customParameter)
        {
            try
            {
                long id = 0;
                var result = await parameterValueDao.InsertAsync(customParameter, dar);
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
                var isSucceed = await parameterValueDao.Delete(id, dar);
                if (isSucceed)
                    return "Invoice Custom Value deleted successfully";
                else
                    return "Invoice Custom Value was not deleted";

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
                var dt = await parameterValueDao.GetAll(dar);
                var lst = ConvertToModel<InvoiceCustomValue>.DataTableToModels(dt);
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
                var dt = await parameterValueDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<InvoiceCustomValue>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IReadOnlyList<InvoiceCustomValue>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<InvoiceCustomValue> GetByIdAsync(int id)
        {
            try
            {
                var dt = await parameterValueDao.GetById(id, dar);
                var lst = ConvertToModel<InvoiceCustomValue>.DataTableToModels(dt);
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

        public async Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomValue customParameter)
        {
            try
            {
                var result = await parameterValueDao.Update(id, customParameter, dar);
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
                var isSucceed = await parameterValueDao.UpdateStatus(id, status, dar);
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
