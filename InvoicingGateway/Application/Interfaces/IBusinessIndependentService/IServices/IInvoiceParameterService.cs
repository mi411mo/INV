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
    public interface IInvoiceParameterService
    {
        Task<ResultRepo<object>> AddAsync(InvoiceCustomParameter customParameter);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceCustomParameter customParameter);
        Task<string> UpdateStatusAsync(int id, int status);
        Task<string> DeleteAsync(int id);
        Task<IList<InvoiceCustomParameter>> GetAllAsync();
        Task<IList<InvoiceCustomParameter>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<ServiceResult<PagedListDto<InvoiceCustomParameter>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<InvoiceCustomParameter> GetByIdAsync(int id);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
