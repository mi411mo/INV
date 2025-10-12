using Application.IBusinessIndependentService;
using Application.IWebServices;
using Connectivity.CommunicationWithAccountingSysImpl;
using Domain.IRepository;
using Domain.IRepository.ICategoryTypeRepository;
using Domain.IRepository.ICustomerRepository;
using Domain.IRepository.IInvoiceRepository;
using Domain.IRepository.IMerchantRepository;
using Domain.IRepository.IOrderRepository;
using Domain.IRepository.IPaymentRepository;
using Domain.IRepository.IServiceRepository;
using Infrastructure.RepositoryImpl;
using Infrastructure.RepositoryImpl.DAL;
using Infrastructure.RepositoryImpl.DAOs.IDAOs;
using Infrastructure.RepositoryImpl.DAOs.IDAOs.Impl;
using Infrastructure.WebServicesImpl;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositortyServices(this IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddScoped<DataAccessRepository>();
            services.AddScoped<IOrdersDao, IOrdersDaoImpl>();
            services.AddScoped<IOrderRepo, OrdersRepoImpl>();
            services.AddScoped<IInvoiceRepo, InvoiceRepoImpl>();
            services.AddScoped<IInvoiceDao, IInvoiceDaoImpl>();
            services.AddScoped<IProductDao, IProductDaoImpl>();
            services.AddScoped<IProductRepo, ProductRepoImpl>();
            services.AddScoped<ICustomerDao, ICustomerDaoImpl>();
            services.AddScoped<ICustomerRepo, CustomerRepoImpl>();
            services.AddScoped<IMerchantRepo, MerchantRepoImpl>();
            services.AddScoped<IMerchantDao, IMerchantDaoImpl>();
            services.AddScoped<IPaymentDao, IPaymentDaoImpl > ();
            services.AddScoped<IPaymentRepo, PaymentRepoImpl>();
            services.AddScoped<ICategoryDao, ICategoryDaoImpl>();
            services.AddScoped<ICategoryTypeDao, ICategoryTypeDaoImpl>();
            services.AddScoped<IInvoiceParameterDao, IInvoiceParameterDaoImpl>();
            services.AddScoped<IInvoiceParameterValueDao, IInvoiceParameterValueDaoImpl>();
            services.AddScoped<ICategoryRepo, CategoryRepoImpl>();
            services.AddScoped<ICategoryTypeRepo, CategoryTypeRepoImpl>();
            services.AddScoped<IInvoiceParameterRepo, InvoiceParameterRepoImpl>();
            services.AddScoped<IInvoiceParameterValueRepo, InvoiceParameterValueRepoImpl>();
            services.AddScoped<IPaymentRequestDao, IRequestPaymentDaoImpl>();
            services.AddScoped<IRequestPaymentRepo, RequestPaymentRepoImpl>();
            services.AddScoped<ILogDao, ILogDaoImpl>();
            services.AddScoped<ILogRepo, LogRepoImpl>();
            services.AddScoped<IAuditLogDao, IAuditLogDaoImpl>();
            services.AddScoped<IAuditLogRepo, AuditLogRepoImpl>();
            services.AddSingleton<ITokenWebService, TokenWebServiceImpl>();
            services.AddSingleton<IPaymentGatewayWebService, PaymentGatewayWebServiceImpl>();
            services.AddSingleton<ICommunicationWithAccountingSystem, CommunicationWithAccountingSysImpl>();

            return services;
        }
    }
}
