using Application.Interfaces.IBusinessIndependentService.IServices;
using Application.Interfaces.IBusinessLogic;
using Application.IServices;
using Application.ServiceImpl;
using Application.ServiceImpl.BusinessLogic;
using Application.Utils.ModelConverter;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Businessdependent.Services
            services.AddScoped<IOrderService, OrderServiceImpl>();
            services.AddScoped<IInvoiceService, InvoiceServiceImpl>();
            services.AddScoped<IProductService, ProductServiceImpl>();
            services.AddScoped<ICustomerService, CustomerServiceImpl>();
            services.AddScoped<IMerchantService, MerchantServiceImpl>();
            services.AddScoped<IPaymentService, PaymentServiceImpl>();
            services.AddScoped<ICategoryService, CategoryServiceImpl>();
            services.AddScoped<ILogService, LogServiceImpl>();
            services.AddScoped<ICategoryTypeService, CategoryTypeServiceImpl>();
            services.AddScoped<IInvoiceParameterService, InvoiceParameterServiceImpl>();
            services.AddScoped<IInvoiceParameterValueService, InvoiceParameterValueServiceImpl>();
            services.AddScoped<IRequestPaymentService, RequestPaymentServiceImpl>();
            services.AddScoped(typeof(IAuditLogService<>), typeof(AuditLogServiceImpl<>));

            // Business logic services
            services.AddScoped<IMerchantBusinessService, MerchantBusinessServiceImpl>();
            services.AddScoped<ICustomerBusinessService, CustomerBusinessServiceImpl>();
            services.AddScoped<IProductBusinessService, ProductBusinessServiceImpl>();
            services.AddScoped<IOrderBusinessService, OrderBusinessServiceImpl>();
            services.AddScoped<IInvoiceBusinessService, InvoiceBusinessServiceImpl>();
            services.AddScoped<IPaymentBusinessService, PaymentBusinessServiceImpl>();
            services.AddScoped<ICategoryBusinessService, CategoryBusinessServiceImpl>();
            services.AddScoped<ICategoryTypeBusinessService, CategoryTypeBusinessServiceImpl>();
            services.AddScoped<IInvoiceParameterBusinessService, InvoiceParameterBusinessServiceImpl>();
            services.AddScoped<IRequestPaymentBusinessService, RequestPaymentBusinessServiceImpl>();
            services.AddScoped<IAuditLogBusinessService, AuditLogBusinessServiceImpl>();

            //model Converter
            services.AddScoped<IAPIModelConverterService, ApiModelConverterServiceImpl>();
            services.AddSingleton<InputValidationService, InputValidationServiceImpl>();

            return services;
        }
    }
}
