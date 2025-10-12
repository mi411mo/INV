using Domain.Entities;
using Domain.Models.Base;
using Infrastructure.RepositoryImpl.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs
{
    public interface ICategoryTypeDao
    {
        Task<bool> InsertAsync(CategoryType mdl, DataAccessRepository daRpst);
        Task<DataTable> GetAll(DataAccessRepository daRpst);
        Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst);
        Task<DataTable> GetById(int id, DataAccessRepository daRpst);
        Task<bool> Update(int id, CategoryType mdl, DataAccessRepository daRpst);
        Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst);
        Task<bool> Delete(int id, DataAccessRepository daRpst);
    }
}
