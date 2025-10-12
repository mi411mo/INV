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
    public class CategoryRepoImpl : ICategoryRepo
    {
        private readonly DataAccessRepository dar;
        private ICategoryDao categoryDao;
        public CategoryRepoImpl(DataAccessRepository _dar, ICategoryDao categoryDao)
        {
            dar = _dar;
            this.categoryDao = categoryDao;
        }
        public async Task<ResultRepo<object>> AddAsync(Category category)
        {
            try
            {
                long id = 0;
                var result = await categoryDao.InsertAsync(category, dar);
                return ReturnRepo.Success(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async  Task<string> DeleteAsync(int id)
        {
            try
            {
                var isSucceed = await categoryDao.Delete(id, dar);
                if (isSucceed)
                    return "Category deleted successfully";
                else
                    return "Category was not deleted";

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Category>> GetAllAsync()
        {
            try
            {
                var dt = await categoryDao.GetAll(dar);
                var lst = ConvertToModel<Category>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<Category>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await categoryDao.GetAll(generalFilter, dar);
                var categories = ConvertToModel<Category>.DataTableToModels(dt);
                return categories;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<IReadOnlyList<Category>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                var dt = await categoryDao.GetById(id, dar);
                var lst = ConvertToModel<Category>.DataTableToModels(dt);
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

        public async Task<ResultRepo<object>> UpdateAsync(int id, Category category)
        {
            try
            {
                var result = await categoryDao.Update(id, category, dar);
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
                var isSucceed = await categoryDao.UpdateStatus(id, status, dar);
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
