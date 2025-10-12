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
    public interface IInvoiceParameterValueRepo
    {
        Task<ResultRepo<object>> AddAsync(InvoiceCustomValue customValue);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomValue customValue);
        Task<string> UpdateStatus(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<InvoiceCustomValue>> GetAllAsync();
        Task<IList<InvoiceCustomValue>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<IReadOnlyList<InvoiceCustomValue>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<InvoiceCustomValue> GetByIdAsync(int id);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
