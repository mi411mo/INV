using Application;
using Application.IBusinessIndependentService;
using Application.Interfaces.IBusinessLogic;
using Application.ServiceImpl.BusinessLogic;
using Application.Utils.JSONValidationUtility.Utils;
using Connectivity.OcelotAPIGatewayImpl;
using Domain.Utils;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;
using WebAPI.Handlers.V1.Categories;
using WebAPI.Handlers.V1.CategoryTypes;
using WebAPI.Handlers.V1.Customers;
using WebAPI.Handlers.V1.InvoiceCustomParameters;
using WebAPI.Handlers.V1.Invoices;
using WebAPI.Handlers.V1.Logs;
using WebAPI.Handlers.V1.Management;
using WebAPI.Handlers.V1.Merchants;
using WebAPI.Handlers.V1.Orders;
using WebAPI.Handlers.V1.Payments;
using WebAPI.Handlers.V1.Products;
using WebAPI.Handlers.V1.RequestPayments;
using WebAPI.Utils;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSwagger(services);
            services.AddCors();
            services.AddHttpContextAccessor();
            services.AddRepositortyServices();
            services.AddApplicationServices();
            services.AddSingleton<IOcelotAPIGateway, OcelotAPIGatewayImpl>();
            services.AddHttpClient();
            services.AddTransient<AuthorizationHelper>();
            ConfigureOcelot(services);
            ConfigureAuthentication(services);
            ConfigureAuthorization(services);
            services.AddDbContext<InvoicingContext>(opt => opt.UseSqlServer("Server=192.168.15.10;database=InvoicingGatewayDB; integrated security=false;User Id=sa;Password=Abc123456;"));


        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }



        private void ConfigureOcelot(IServiceCollection services)
        {
            services.AddOcelot(Configuration)

                .AddDelegatingHandler<GetAllMerchantsHandler>()
                .AddDelegatingHandler<GetMerchantByIdHandler>()
                .AddDelegatingHandler<GetAllUnauthMerchantsHandler>()
                .AddDelegatingHandler<CreateMerchantHandler>()
                .AddDelegatingHandler<CreateUnauthMerchantHandler>()
                .AddDelegatingHandler<UpdateMerchantHandler>()
                .AddDelegatingHandler<UpdateMerchantStatusHandler>()
                .AddDelegatingHandler<DeleteMerchantHandler>()
                //Customers Handlers
                .AddDelegatingHandler<GetAllCustomersHandler>()
                .AddDelegatingHandler<GetCustomerByIdHandler>()
                .AddDelegatingHandler<CreateUnauthCustomerHandler>()
                .AddDelegatingHandler<CreateCustomerHandler>()
                .AddDelegatingHandler<UpdateCustomerHandler>()
                .AddDelegatingHandler<DeleteCustomerHandler>()
                .AddDelegatingHandler<UpdateCustomerStatusHandler>()
                // Products Handlers
                .AddDelegatingHandler<CreateProductHandler>()
                .AddDelegatingHandler<DeleteProductHandler>()
                .AddDelegatingHandler<GetAllProductsHandler>()
                .AddDelegatingHandler<GetAllUnauthProductsHandler>()
                .AddDelegatingHandler<GetProductByIdHandler>()
                .AddDelegatingHandler<UpdateProductHandler>()
                .AddDelegatingHandler<UpdateProductStatusHandler>()
                // Orders Handlers
                .AddDelegatingHandler<GetAllOrdersHandler>()
                .AddDelegatingHandler<CreateOrderHandler>()
                .AddDelegatingHandler<ConfirmOrderHandler>()
                .AddDelegatingHandler<GetOrderByIdHandler>()
                .AddDelegatingHandler<UpdateOrderStatusHandler>()
                .AddDelegatingHandler<GetOrderByReferenceHandler>()
                // INvoices Handlers
                .AddDelegatingHandler<GetInvoiceByReferenceHandler>()
                .AddDelegatingHandler<GetAllInvoicesHandler>()
                .AddDelegatingHandler<GetAllUnauthInvoicesHandler>()
                .AddDelegatingHandler<GetInvoiceByIdHandler>()
                .AddDelegatingHandler<CreateInvoiceHandler>()
                .AddDelegatingHandler<UpdateInvoiceStatusHandler>()
                .AddDelegatingHandler<ConfirmInvoiceHandler>()
                // Payments Handlers
                .AddDelegatingHandler<GetAllPaymentsHandler>()
                .AddDelegatingHandler<ConfirmPaymentHandler>()
                .AddDelegatingHandler<GetPaymentByIdHandler>()
                .AddDelegatingHandler<GetPaymentByReferenceHandler>()
                .AddDelegatingHandler<CreatePaymentHandler>()
                .AddDelegatingHandler<UpdatePaymentStatusHandler>()
                .AddDelegatingHandler<GetAllPaymentMethodsHandler>()
                .AddDelegatingHandler<DirectPaymentHandler>()
                // Categories Handlers
                .AddDelegatingHandler<CreateCategoryHandler>()
                .AddDelegatingHandler<GetCategoryByIdHandler>()
                .AddDelegatingHandler<GetAllUnauthCategoriesHandler>()
                .AddDelegatingHandler<DeleteCategoryHandler>()
                .AddDelegatingHandler<GetAllCategoriesHandler>()
                .AddDelegatingHandler<UpdateCategoryStatusHandler>()
                .AddDelegatingHandler<UpdateCategoryHandler>()
                // CategoryTypes Handlers
                .AddDelegatingHandler<CreateCategoryTypeHandler>()
                .AddDelegatingHandler<GetCategoryTypeByIdHandler>()
                .AddDelegatingHandler<DeleteCategoryTypeHandler>()
                .AddDelegatingHandler<GetAllCategoryTypesHandler>()
                .AddDelegatingHandler<GetAllUnauthCategoryTypesHandler>()
                .AddDelegatingHandler<UpdateCategoryTypeStatusHandler>()
                .AddDelegatingHandler<UpdateCategoryTypeHandler>()
                // InvoiceParameters Handlers
                .AddDelegatingHandler<CreateInvoiceParameterHandler>()
                .AddDelegatingHandler<GetInvoiceParameterByIdHandler>()
                .AddDelegatingHandler<GetAllUnauthInvoiceParametersHandler>()
                .AddDelegatingHandler<DeleteInvoiceParameterHandler>()
                .AddDelegatingHandler<GetAllInvoiceParametersHandler>()
                .AddDelegatingHandler<UpdateInvoiceParameterStatusHandler>()
                .AddDelegatingHandler<UpdateInvoiceParameterHandler>()
                // Request Payments Handlers
                .AddDelegatingHandler<GetRequestPaymentByReferenceHandler>()
                .AddDelegatingHandler<GetAllRequestPaymentsHandler>()
                .AddDelegatingHandler<GetAllUnauthRequestPaymentsHandler>()
                .AddDelegatingHandler<GetRequestPaymentByIdHandler>()
                .AddDelegatingHandler<CreateRequestPaymentHandler>()
                .AddDelegatingHandler<UpdatePaymentRequestStatusHandler>()
                .AddDelegatingHandler<ConfirmRequestPaymentHandler>()
                //Statistics
                .AddDelegatingHandler<GetStatisticsHandler>()
                .AddDelegatingHandler<GetUnauthStatisticsHandler>()
                //Logs
                .AddDelegatingHandler<GetAllLogsHandler>()
                // management
                .AddDelegatingHandler<CheckProviderPaymentStatusHandler>()
                .AddDelegatingHandler<GetAllAuditLogHandler>();
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {

            services.AddAuthentication("Bearer")
            .AddOAuth2Introspection("Bearer", options =>
            {
                var variableName = Configuration["EnviornmentLookup:IAMMgmtSTSURLVariable"];
                var stsUrl = SystemEnviornmentLookup.GetEnvVariableValue(variableName);
                options.Authority = string.IsNullOrEmpty(stsUrl) ? Configuration["STS:URL"] : stsUrl;

                options.ClientId = Configuration["STS:ClientId"];
                options.ClientSecret = "Pass123@#"; // Consider using a more secure solution
                options.SaveToken = false;
                options.CacheDuration = TimeSpan.Zero;

                ConfigureHttpClients(options);
                options.Validate();
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Bearer"; // or "Mixed" if using mixed scheme
            });

            // Optionally you can remove the second authentication setup if it duplicates the first
            services.AddAuthentication("IDS4")
             .AddOAuth2Introspection("IDS4", options =>
             {
                 var variableName = Configuration["EnviornmentLookup:IAMMgmtSTSURLVariable"];
                 var stsURL = SystemEnviornmentLookup.GetEnvVariableValue(variableName);
                 options.Authority = stsURL;
                 options.ClientId = Configuration.GetSection("STS").GetSection("ClientId").Value;
                 options.ClientSecret = "Pass123@#"; // Consider using a more secure solution
                 ConfigureHttpClients(options);
             });
        }

        private void ConfigureHttpClients(OAuth2IntrospectionOptions options)
        {
            options.DiscoveryHttpHandler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            options.IntrospectionHttpHandler = new SocketsHttpHandler
            {
                SslOptions = new SslClientAuthenticationOptions
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                }
            };
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("scope", Configuration["STS:ClientId"])
                    .Build();
            });
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/gateway");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            app.UseRouting();
            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("X-Pagination"));
            app.UseAuthentication();
            app.UseAuthorization();

            if (!Directory.Exists(Constants.GeneralJSONSchemasPath))
                Directory.CreateDirectory(Constants.GeneralJSONSchemasPath);

            if (Configuration.GetValue<string>("MySettings:GenerateSchema") == "1")
                GenerateJsonSchemaFile.WriteModelsSchemas();

            await app.UseOcelot();
            ModelsSchemasLoader.loadModelsSchemas();
        }
    }
}