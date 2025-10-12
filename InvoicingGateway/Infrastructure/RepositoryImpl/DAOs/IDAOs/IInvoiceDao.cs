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
    public interface IInvoiceDao
    {
        Task<bool> InsertAsync(InvoiceModel mdl, DataAccessRepository daRpst);
        Task<DataTable> GetAll(DataAccessRepository daRpst);
        Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst);
        Task<DataTable> GetById(int id, DataAccessRepository daRpst);
        Task<DataTable> GetByToken(long payToken, DataAccessRepository daRpst);
        Task<DataTable> GetByInvNo(string invNo, DataAccessRepository daRpst);
        Task<bool> Update(int id, InvoiceModel mdl, DataAccessRepository daRpst);
        Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst);
    }
}
