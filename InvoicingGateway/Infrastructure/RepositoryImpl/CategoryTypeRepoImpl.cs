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
    public class CategoryTypeRepoImpl : ICategoryTypeRepo
    {
        private readonly DataAccessRepository dar;
        private ICategoryTypeDao categoryTypeDao;
        public CategoryTypeRepoImpl(DataAccessRepository _dar, ICategoryTypeDao categoryTypeDao)
        {
            dar = _dar;
            this.categoryTypeDao = categoryTypeDao;
        }
        public async Task<ResultRepo<object>> AddAsync(CategoryType category)
        {
            try
            {
                long id = 0;
                var result = await categoryTypeDao.InsertAsync(category, dar);
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
                var isSucceed = await categoryTypeDao.Delete(id, dar);
                if (isSucceed)
                    return "Category type deleted successfully";
                else
                    return "Category type was not deleted";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<CategoryType>> GetAllAsync()
        {
            try
            {
                var dt = await categoryTypeDao.GetAll(dar);
                var lst = ConvertToModel<CategoryType>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<CategoryType>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await categoryTypeDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<CategoryType>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IReadOnlyList<CategoryType>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<CategoryType> GetByIdAsync(int id)
        {
            try
            {
                var dt = await categoryTypeDao.GetById(id, dar);
                var lst = ConvertToModel<CategoryType>.DataTableToModels(dt);
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

        public async Task<ResultRepo<object>> UpdateAsync(int id, CategoryType category)
        {
            try
            {
                var result = await categoryTypeDao.Update(id, category, dar);
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
                var isSucceed = await categoryTypeDao.UpdateStatus(id, status, dar);
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
