using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessIndependentService.IServices
{
    public interface IInvoiceParameterValueService
    {
        Task<ResultRepo<object>> AddAsync(InvoiceCustomValue customValue);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomValue customValue);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<InvoiceCustomValue>> GetAllAsync();
        Task<IList<InvoiceCustomValue>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<InvoiceCustomValue>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<InvoiceCustomValue> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
