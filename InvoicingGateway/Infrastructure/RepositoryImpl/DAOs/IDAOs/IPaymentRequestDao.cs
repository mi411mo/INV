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
    public interface IPaymentRequestDao
    {
        Task<bool> InsertAsync(RequestPayment mdl, DataAccessRepository daRpst);
        Task<DataTable> GetAll(DataAccessRepository daRpst);
        Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst);
        Task<DataTable> GetById(int id, DataAccessRepository daRpst);
        Task<DataTable> GetByPayRef(string payRef, DataAccessRepository daRpst);
        Task<bool> Update(int id, RequestPayment mdl, DataAccessRepository daRpst);
        Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst);
    }
}
