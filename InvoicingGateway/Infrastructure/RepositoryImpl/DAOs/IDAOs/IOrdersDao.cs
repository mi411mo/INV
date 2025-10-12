using Domain.Entities;
using Domain.Models.Base;
using Infrastructure.RepositoryImpl.DAL;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DAOs.IDAOs
{
    public interface IOrdersDao
    {
        Task<bool> InsertAsync(OrderModel mdl, DataAccessRepository daRpst);
        Task<DataTable> GetAll(DataAccessRepository daRpst);
        Task<DataTable> GetAll(GeneralFilterDto generalFilter, DataAccessRepository daRpst);
        Task<DataTable> GetOrderById(int id, DataAccessRepository daRpst);
        Task<DataTable> GetOrderByRef(string orderRef, DataAccessRepository daRpst);
        Task<bool> Update(int id, OrderModel mdl, DataAccessRepository daRpst);
        Task<bool> Delete(int id, DataAccessRepository daRpst);
        Task<bool> Approve(string orderRef, DataAccessRepository daRpst);
        Task<bool> UpdateStatus(int id, int status, DataAccessRepository daRpst);
    }
}
