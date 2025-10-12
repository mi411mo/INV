using Domain.Entities;
using Domain.Models.Base;
using Domain.Models.Invoices.ResponseDto;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepository.IInvoiceRepository
{
    public interface IInvoiceRepo
    {
        Task<ResultRepo<object>> AddAsync(InvoiceModel invoiceModel);
        Task<ResultRepo<object>> UpdateAsync(int id, InvoiceModel invoiceModel);
        Task<bool> AddByOrderRefAsync(string orederRef);
        Task<bool> UpdateStatus(int id, int status);
        Task<IList<InvoiceModel>> GetAllAsync();
        Task<IList<InvoiceModel>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<InvoiceModel> GetByIdAsync(int id);
        Task<List<InvoiceModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions);
        Task<InvoiceModel> GetByTokenAsync(long payToken);
        Task<InvoiceModel> GetByInvNoAsync(string invNo);
        Task<long> GetCountAsync(object _oDataOptions);
    }
}
