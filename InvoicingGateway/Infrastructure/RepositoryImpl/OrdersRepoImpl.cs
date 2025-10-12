using Application.Utils.CustomException;
using Domain.Entities;
using Domain.IRepository.IOrderRepository;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using Infrastructure.Context;
using Infrastructure.Repositories.Impl.V1.DRY;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DAOs.IDAOs;
using Microsoft.AspNet.OData.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl
{
    public class OrdersRepoImpl : IOrderRepo
    {
        private readonly DataAccessRepository dar;
        private IOrdersDao orderaDao;
        protected readonly InvoicingContext context;
        public OrdersRepoImpl(DataAccessRepository _dar, IOrdersDao orderaDao, InvoicingContext context)
        {
            dar = _dar;
            this.orderaDao = orderaDao;
            this.context = context;
        }
        public async Task<ResultRepo<object>> AddAsync(OrderModel ordersModel)
        {
            try
            {
                long id = 0;
                var result = await orderaDao.InsertAsync(ordersModel, dar);
                return ReturnRepo.Success(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ConfirmOrderAsync(string orderRef)
        {
            try
            {
                var isSucceed = await orderaDao.Approve(orderRef, dar);
                return isSucceed;
             
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ResultRepo<object>> CancelOrderAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<OrderModel>> GetAllAsync()
        {
            try
            {
                var dt = await orderaDao.GetAll(dar);
                var lst = ConvertToModel<OrderModel>.DataTableToModels(dt);
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IReadOnlyList<OrderModel>> GetAllAsync(int pageNumber, int pageSize, object _oDataOptions)
        {
            var oDataOptions = (ODataQueryOptions<OrderModel>)_oDataOptions;
            var query = oDataOptions.ApplyTo(context.Orders.AsQueryable()) as IQueryable<OrderModel>;

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OrderModel> GetOrderByIdAsync(int id)
        {
            try
            {
                var dt = await orderaDao.GetOrderById(id, dar);
                var lst = ConvertToModel<OrderModel>.DataTableToModels(dt);
                var orderData = lst.FirstOrDefault();
                return orderData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<ResultRepo<object>> UpdateAsync(OrderModel ordersModel)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UpdateStatus(int id, int status)
        {
            try
            {
                var isSucceed = await orderaDao.UpdateStatus(id, status, dar);
                if (isSucceed)
                    return "Status was updated successfully";
                else
                    return "Status was not updated";


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<long> GetCountAsync(object _oDataOptions)
        {
            if (_oDataOptions != null)
            {
                var oDataOptions = (ODataQueryOptions<OrderModel>)_oDataOptions;
                var query = oDataOptions.ApplyTo(context.Orders.AsQueryable()) as IQueryable<OrderModel>;
                return await query.CountAsync();
            }
            return await context.Orders.CountAsync();
        }

        public async Task<OrderModel> GetOrderRefAsync(string orderRef)
        {
            try
            {
                var dt = await orderaDao.GetOrderByRef(orderRef, dar);
                var lst = ConvertToModel<OrderModel>.DataTableToModels(dt);
                var orderData = lst.FirstOrDefault();
                return orderData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IList<OrderModel>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var dt = await orderaDao.GetAll(generalFilter, dar);
                var lst = ConvertToModel<OrderModel>.DataTableToModels(dt);
                /*if (!string.IsNullOrEmpty(generalFilter.CustomerId) && generalFilter.CustomerId != "0")
                {
                    lst = lst.Where(e => e.CustomerId == generalFilter.CustomerId).ToList();
                }*/
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
