using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Models.Invoices.ResponseDto;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IInvoiceService
    {
        Task<ResultRepo<object>> AddAsync(InvoiceModel invoiceModel);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceModel invoiceModel);
        Task<ResultRepo<InvoiceModel>> AddPaidInvByOrderRef(string orederRef, string payMethod);
        Task<bool> UpdateStatusAsync(int id, int status);
        Task<IList<InvoiceModel>> GetAllAsync();
        Task<IList<InvoiceModel>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<InvoiceModel> GetByIdAsync(int id);
        Task<InvoiceModel> GetByTokenAsync(long payToken);
        Task<ServiceResult<PagedListDto<InvoiceModel>>> GetAllAsync(PaginationFilter filter, object oDataOptions);
        Task<InvoiceModel> GetByInvoiceNo(string invNo);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
