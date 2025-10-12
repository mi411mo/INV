using Domain.Entities;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository
{
    public interface IInvoiceParameterRepo
    {
        Task<ResultRepo<object>> AddAsync(InvoiceCustomParameter customParameter);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomParameter customParameter);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<InvoiceCustomParameter>> GetAllAsync();
        Task<IList<InvoiceCustomParameter>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<InvoiceCustomParameter>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<InvoiceCustomParameter> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
