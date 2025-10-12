using Application.DTOs;
using Domain.Entities;
using Domain.Models.Base;
using Domain.Models.Categories;
using Domain.Models.CategoryTypes;
using Domain.Models.Customers;
using Domain.Models.InvoiceCustomParameters;
using Domain.Models.InvoiceCustomValues;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Invoices.ResponseDto;
using Domain.Models.Merchants;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Orders.ResponseDto;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Payments.ResponseDto;
using Domain.Models.RequestPayments;
using Domain.Models.Services;
using Domain.Models.Services.RequestDto;
using Domain.Utils;
using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharwat.ResponseCodes.Lookup.Core.Interfaces;
using Tharwat.ResponseCodes.Lookup.Infrastructure.Services;

namespace Application.Utils.ModelConverter
{
    public class ApiModelConverterServiceImpl : IAPIModelConverterService
    {
        private static IResponseService responseService;
        public static void GetInstance()
        {
            if (responseService == null)
            {
                responseService = new ResponseService();
            }
        }
        public async Task<TResponseDto> ConvertToGatewayModel<TResponseDto, TResponse>(TResponse response, object request) where TResponseDto : class
        {
            try
            {
                switch (response)
                {
                    /*case OrderResponseDto e:
                        return (await ToOrderResponseDTO(e)) as TResponseDto;*/


                }
                throw new Exception(response.GetType().Name + " not Found Tharawat Models , Source should be in Tharawat Api Models");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TRequest> ConvertToEntityModel<TRequest>(object request) where TRequest : class
        {
            try
            {
                switch (request)
                {
                    case OrderRequestDto e:
                        return (await ToOrderEntity(e)) as TRequest;
                    case InvoiceRequestDto e:
                        return (await ToInvoiceEntity(e)) as TRequest;
                    case ProductRequestDto e:
                        return (await ToProducteEntity(e)) as TRequest;
                    case CustomerRequestDto e:
                        return (await ToCustomerEntity(e)) as TRequest;
                    case MerchantRequestDto e:
                        return (await ToMerchantEntity(e)) as TRequest;
                    case PaymentRequestDto e:
                        return (await ToPaymentEntity(e)) as TRequest;
                    case CategoryRequestDto e:
                        return (await ToCategoryEntity(e)) as TRequest;
                    case CategoryTypeRequestDto e:
                        return (await ToCategoryTypeEntity(e)) as TRequest;
                    case InvoiceParametersRequestDto e:
                        return (await ToInvoiceParameterEntity(e)) as TRequest;
                    case RequestPaymentRequestDto e:
                        return (await ToRequestPaymentEntity(e)) as TRequest;

                }
                throw new Exception(request.GetType().Name + " not Found Tharawat Models , Source should be in Tharawat Api Models");
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        // To Provider Models
        public async Task<OrderModel> ToOrderEntity(OrderRequestDto requestDto)
        {

            var entity = new OrderModel()
            {
                MerchantId = requestDto.MerchantId,
                TotalAmount = requestDto.TotalAmount,
                CurrencyCode = requestDto.CurrencyCode,
                CategoryType = requestDto.CategoryType,
                OrderReference = InvoiceUtility.GenerateOrderReference(),
                Status = (int)OrderStatusEnum.Pending,
                Description = requestDto.Description ?? "",
                Products = JSONHelper<List<ProductItems>>.GetJSONStr(requestDto.Products) ?? "",
                CustomerInfo = JSONHelper<CustomerInfo>.GetJSONStr(requestDto.CustomerInfo) ?? "",
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId=requestDto.UserId

            };
            await Task.CompletedTask;
            return entity;

        }
        public async Task<InvoiceModel> ToInvoiceEntity(InvoiceRequestDto requestDto)
        {

            var entity = new InvoiceModel()
            {
                MerchantId = requestDto.MerchantId,
                PaymentToken = InvoiceUtility.GeneratePayToken(),
                InvoiceNumber = InvoiceUtility.GeneratingInvoiceNo(),
                OrderReference = InvoiceUtility.GenerateOrderReference(),
                Customer = JSONHelper<CustomerInfo>.GetJSONStr(requestDto.Customer) ?? "",
                OrderId = requestDto.OrderId,
                TotalAmountDue = requestDto.TotalAmount + requestDto.AmountShipping - requestDto.Discount,
                AmountPaid = 0.0M,
                CategoryType = requestDto.CategoryType,
                AmountRemaining = requestDto.TotalAmount + requestDto.AmountShipping - requestDto.Discount,
                AmountShipping = requestDto.AmountShipping,
                Discount = requestDto.Discount,
                CurrencyCode = requestDto.CurrencyCode,
                Products = JSONHelper<List<ProductItems>>.GetJSONStr(requestDto.Products) ?? "",
                //AcceptedCurrencies = JSONHelper<string[]>.GetJSONStr(requestDto.AcceptedCurrencies) ?? "",
                PaymentMethods = JSONHelper<string[]>.GetJSONStr(requestDto.PaymentMethods) ?? "",
                Notification = JSONHelper<Notification>.GetJSONStr(requestDto.Notification) ?? "",
                Status = (int)InvoiceStatusEnums.Unpaid,
                Description = requestDto.Description ?? "",
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId

            };
            await Task.CompletedTask;
            return entity;

        }

        public async Task<RequestPayment> ToRequestPaymentEntity(RequestPaymentRequestDto requestDto)
        {

            var entity = new RequestPayment()
            {
                MerchantId = requestDto.MerchantId,
                RequestPaymentReference = InvoiceUtility.GeneratingInvoiceNo(),
                Customer = JSONHelper<CustomerInfo>.GetJSONStr(requestDto.Customer) ?? string.Empty,
                TotalAmount = requestDto.TotalAmount,
                AmountPaid = 0.0M,
                CategoryType = requestDto.CategoryType,
                CurrencyCode = requestDto.CurrencyCode,
                PaymentMethod = requestDto.PaymentMethod ?? string.Empty,
                Status = (int)InvoiceStatusEnums.Unpaid,
                Description = requestDto.Description ?? string.Empty,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId

            };
            await Task.CompletedTask;
            return entity;

        }
        public async Task<ProductModel> ToProducteEntity(ProductRequestDto requestDto)
        {

            var entity = new ProductModel()
            {
                Name = requestDto.Name,
                UnitPrice = requestDto.UnitPrice,
                Quantity = requestDto.Quantity,
                TotalAmount = requestDto.TotalAmount,
                Discount = requestDto.Discount,
                CategoryType = requestDto.CategoryType,
                CurrencyCode = requestDto.CurrencyCode,
                CategoryId = requestDto.CategoryId,
                SubCategoryId = requestDto.SubCategoryId,
                Status = (int)requestDto.Status,
                MerchantId = requestDto.MerchantId,
                ImageURL = requestDto.ImageURL,
                Description = requestDto.Description ?? "",
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                UserId = requestDto.UserId,
                ClientId = requestDto.ClientId,
            };
            await Task.CompletedTask;
            return entity;

        }
        public async Task<Customer> ToCustomerEntity(CustomerRequestDto requestDto)
        {

            var entity = new Customer()
            {
                Name = requestDto.Name,
                Phone = requestDto.Phone,
                Email = requestDto.Email,
                ProfileId = requestDto.ProfileId ?? string.Empty,
                InvoicePrefix = requestDto.InvoicePrefix,
                Address = requestDto.Address,
                MerchantId = requestDto.MerchantId,
                IsActive = requestDto.IsActive,
                CategoryType = requestDto.CategoryType,
                Details = requestDto.Details,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
            };
            await Task.CompletedTask;
            return entity;

        }
        public async Task<Merchant> ToMerchantEntity(MerchantRequestDto requestDto)
        {

            var entity = new Merchant()
            {
                ProfileId = requestDto.ProfileId,
                ArabicName = requestDto.ArabicName,
                EnglishName = requestDto.EnglishName,
                Phone = requestDto.Phone,
                Email = requestDto.Email,
                CategoryType = requestDto.CategoryType,
                IntegrationType = (int)requestDto.IntegrationType,
                AccessChannel = (int)requestDto.AccessChannel,
                Address = requestDto.Address,
                InvoicePrefix = requestDto.InvoicePrefix,
                IsActive = requestDto.IsActive,
                Details = requestDto.Details,
                LogoImageUrl = requestDto.LogoImageUrl,
                BusinessCategory = requestDto.BusinessCategory,
                BusinessDescription = requestDto.BusinessDescription,
                WebsiteUrl = requestDto.WebsiteUrl,
                SocialMedia = JSONHelper<Dictionary<string, string>>.GetJSONStr(requestDto.SocialMedia) ?? "",
                StoreLocation = JSONHelper<Location>.GetJSONStr(requestDto.StoreLocation) ?? "",
                OperatingHours = JSONHelper<Dictionary<string, string>>.GetJSONStr(requestDto.OperatingHours) ?? "",
                CustomerReviews = JSONHelper<List<Review>>.GetJSONStr(requestDto.CustomerReviews) ?? "",
                MarkdownContent = requestDto.MarkdownContent,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
            };
            await Task.CompletedTask;
            return entity;

        }

        public async Task<Payment> ToPaymentEntity(PaymentRequestDto requestDto)
        {

            var entity = new Payment()
            {
                TargetReference = requestDto.TargetReference ?? "",
                TransactionReference = requestDto.TransactionReference,
                PaymentReference = InvoiceUtility.GeneratePayReference(),
                AmountDue = requestDto.AmountDue,
                CurrencyCode = requestDto.CurrencyCode,
                MerchantId = requestDto.MerchantId,
                OrderReference = requestDto.OrderReference,
                InvoiceReference = requestDto.InvoiceReference,
                PaymentMethod = requestDto.PaymentMethod,
                PaymentStatus = PaymentStatusEnum.Pending,
                Details = requestDto.Details,
                IssueDate = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
                //PayDate = DateTime.Now

            };
            await Task.CompletedTask;
            return entity;

        }

        public async Task<Category> ToCategoryEntity(CategoryRequestDto requestDto)
        {

            var entity = new Category()
            {
                CategoryName = requestDto.CategoryName,
                ImageURL = requestDto.ImageURL,
                MerchantId = requestDto.MerchantId,
                Description = requestDto.Description,
                CategoryType = requestDto.CategoryType,
                IsActive = requestDto.IsActive,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
            };
            await Task.CompletedTask;
            return entity;

        }

        public async Task<CategoryType> ToCategoryTypeEntity(CategoryTypeRequestDto requestDto)
        {

            var entity = new CategoryType()
            {
                Name = requestDto.Name,
                EnName = requestDto.EnName,
                Type = (int)requestDto.Type,
                Code = requestDto.Code,
                Description = requestDto.Description,
                IsActive = requestDto.IsActive,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
            };
            await Task.CompletedTask;
            return entity;

        }
        public async Task<InvoiceCustomParameter> ToInvoiceParameterEntity(InvoiceParametersRequestDto requestDto)
        {

            var entity = new InvoiceCustomParameter()
            {
                ParameterName = requestDto.ParameterName,
                MerchantId = requestDto.MerchantId,
                ParameterType = (int)requestDto.ParameterType,
                IsActive = requestDto.IsActive,
                CreatedAt = DateTime.Now,
                CustomerId = requestDto.CustomerId,
                ClientId = requestDto.ClientId,
                UserId = requestDto.UserId
            };
            await Task.CompletedTask;
            return entity;

        }

        public async Task<TResponse> ConvertToResponseDto<TResponse>(object entity) where TResponse : class
        {
            try
            {
                switch (entity)
                {
                    case InvoiceModel e:
                        return (await ToInvoiceResponseDto(e)) as TResponse;
                    case Merchant e:
                        return (await ToMerchantResponseDto(e)) as TResponse;
                    case Customer e:
                        return (await ToCustomerResponseDto(e)) as TResponse;
                    case ProductModel e:
                        return (await ToProductResponseDto(e)) as TResponse;
                    case Payment e:
                        return (await ToPaymentResponseDto(e)) as TResponse;
                    case OrderModel e:
                             return (await ToOrderResponseDto(e)) as TResponse;
                    case Category e:
                        return (await ToCategoryResponseDto(e)) as TResponse;
                    case CategoryType e:
                        return (await ToCategoryTypeResponseDto(e)) as TResponse;
                    case InvoiceCustomParameter e:
                        return (await ToInvoiceParameterResponseDto(e)) as TResponse;
                    case RequestPayment e:
                        return (await ToRequestPaymentResponseDto(e)) as TResponse;

                }
                throw new Exception(entity.GetType().Name + " not Found Tharawat Models , Source should be in Tharawat Api Models");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<InvoiceResponseDto> ToInvoiceResponseDto(InvoiceModel invoiceEntity)
        {

            var responseDto = new InvoiceResponseDto()
            {
                Id = invoiceEntity.Id,
                MerchantId = invoiceEntity.MerchantId,
                PaymentToken = invoiceEntity.PaymentToken,
                InvoiceNumber = invoiceEntity.InvoiceNumber,
                CategoryType = invoiceEntity.CategoryType,
                Customer = string.IsNullOrEmpty(invoiceEntity.Customer) ? null : JSONHelper<CustomerInfo>.GetTypedModel(invoiceEntity.Customer),
                OrderId = int.Parse(invoiceEntity.OrderId.ToString()),
                TotalAmountDue = invoiceEntity.TotalAmountDue,
                AmountPaid = invoiceEntity.AmountPaid,
                AmountRemaining = invoiceEntity.AmountRemaining,
                AmountShipping = invoiceEntity.AmountShipping,
                Discount = invoiceEntity.Discount,
                CurrencyCode = invoiceEntity.CurrencyCode,
                Products = string.IsNullOrEmpty(invoiceEntity.Products) ? null : JSONHelper<List<ProductItems>>.GetTypedModel(invoiceEntity.Products),
                CustomParameters = string.IsNullOrEmpty(invoiceEntity.CustomParameters) ? null : JSONHelper<List<InvoiceCustomValueResponse>>.GetTypedModel(invoiceEntity.CustomParameters),
                //AcceptedCurrencies = JSONHelper<string[]>.GetJSONStr(requestDto.AcceptedCurrencies) ?? "",
                PaymentMethods = string.IsNullOrEmpty(invoiceEntity.PaymentMethods) ? null : JSONHelper<string[]>.GetTypedModel(invoiceEntity.PaymentMethods),
                Notification = string.IsNullOrEmpty(invoiceEntity.Notification) ? null : JSONHelper<Notification>.GetTypedModel(invoiceEntity.Notification),
                Status = invoiceEntity.Status,
                Description = invoiceEntity.Description ?? "",
                OrderReference = invoiceEntity.OrderReference ?? "",
                CreatedAt = invoiceEntity.CreatedAt,
                UpdatedAt = invoiceEntity.UpdatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<RequestPaymentResponseDto> ToRequestPaymentResponseDto(RequestPayment reqPay)
        {

            var responseDto = new RequestPaymentResponseDto()
            {
                Id = reqPay.Id,
                MerchantId = reqPay.MerchantId,
                RequestPaymentReference = reqPay.RequestPaymentReference,
                CategoryType = reqPay.CategoryType,
                Customer = string.IsNullOrEmpty(reqPay.Customer) ? null : JSONHelper<CustomerInfo>.GetTypedModel(reqPay.Customer),
                TotalAmount = reqPay.TotalAmount,
                AmountPaid = reqPay.AmountPaid,
                CurrencyCode = reqPay.CurrencyCode,
                PaymentMethod = reqPay.PaymentMethod ?? string.Empty,
                Status = reqPay.Status,
                Description = reqPay.Description ?? string.Empty,
                CreatedAt = reqPay.CreatedAt,
                UpdatedAt = reqPay.UpdatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }
        public async Task<MerchantResponseDto> ToMerchantResponseDto(Merchant entity)
        {

            var responseDto = new MerchantResponseDto()
            {
                Id = entity.Id,
                ProfileId = entity.ProfileId,
                ArabicName = entity.ArabicName,
                EnglishName = entity.EnglishName,
                Phone = entity.Phone,
                Email = entity.Email,
                CategoryType = entity.CategoryType,
                Address = entity.Address,
                InvoicePrefix = entity.InvoicePrefix,
                IntegrationType = entity.IntegrationType,
                AccessChannel = entity.AccessChannel,
                IsActive = entity.IsActive,
                Details = entity.Details,
                LogoImageUrl = entity.LogoImageUrl,
                BusinessCategory = entity.BusinessCategory,
                BusinessDescription = entity.BusinessDescription,
                WebsiteUrl = entity.WebsiteUrl,
                SocialMedia = string.IsNullOrEmpty(entity.SocialMedia) ? null : JSONHelper<Dictionary<string, string>>.GetTypedModel(entity.SocialMedia),
                StoreLocation = string.IsNullOrEmpty(entity.StoreLocation) ? null : JSONHelper<Location>.GetTypedModel(entity.StoreLocation),
                OperatingHours = string.IsNullOrEmpty(entity.OperatingHours) ? null  :JSONHelper<Dictionary<string, string>>.GetTypedModel(entity.OperatingHours),
                CustomerReviews = string.IsNullOrEmpty(entity.CustomerReviews) ? null : JSONHelper<List<Review>>.GetTypedModel(entity.CustomerReviews),
                MarkdownContent = entity.MarkdownContent,
                CreatedAt = entity.CreatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }
        public async Task<CustomerResponseDto> ToCustomerResponseDto(Customer entity)
        {

            var responseDto = new CustomerResponseDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                ProfileId = entity.ProfileId,
                Email = entity.Email,
                Address = entity.Address,
                MerchantId = entity.MerchantId,
                CategoryType = entity.CategoryType,
                InvoicePrefix = entity.InvoicePrefix,
                IsActive = entity.IsActive,
                Details = entity.Details
            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<PaymentResponseDto> ToPaymentResponseDto(Payment entity)
        {

            var responseDto = new PaymentResponseDto()
            {
                Id = entity.Id,
                TransactionReference = entity.TransactionReference,
                TargetReference = entity.TargetReference,
                InvoiceReference = entity.InvoiceReference,
                OrderReference = entity.OrderReference,
                RequestPaymentReference = entity.RequestPaymentReference,
                MerchantId = entity.MerchantId,
                AmountDue = entity.AmountDue,
                CurrencyCode = entity.CurrencyCode,
                IssueDate = entity.IssueDate,
                PayDate = entity.PayDate,
                PaymentMethod = entity.PaymentMethod,
                PaymentStatus = entity.PaymentStatus,
                PaymentType = entity.PaymentType,
                CustomerInfo =  string.IsNullOrEmpty(entity.CustomerInfo) ? null : JSONHelper<CustomerInfo>.GetTypedModel(entity.CustomerInfo),
                Details = entity.Details
            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<ProductResponseDto> ToProductResponseDto(ProductModel entity)
        {

            var responseDto = new ProductResponseDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                Status = entity.Status,
                UnitPrice = entity.UnitPrice,
                Discount = entity.Discount,
                MerchantId = entity.MerchantId,
                Quantity = entity.Quantity,
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                CategoryId = entity.CategoryId,
                CategoryType = entity.CategoryType,
                SubCategoryId = entity.SubCategoryId,
                ImageURL = entity.ImageURL,
                Description = entity.Description
            };
            await Task.CompletedTask;
            return responseDto;

        }

       public async Task<GetOrderResponseDto> ToOrderResponseDto(OrderModel entity)
        {

            var responseDto = new GetOrderResponseDto()
            {
                Id = entity.Id,
                MerchantId = entity.MerchantId,
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                OrderReference = entity.OrderReference,
                CategoryType = entity.CategoryType,
                Products = string.IsNullOrEmpty(entity.Products) ? null : JSONHelper<List<ProductItems>>.GetTypedModel(entity.Products),
                CustomerInfo = string.IsNullOrEmpty(entity.CustomerInfo) ? null : JSONHelper<CustomerInfo>.GetTypedModel(entity.CustomerInfo),
                Status = entity.Status,
                Description = entity.Description,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<CategoryResponseDto> ToCategoryResponseDto(Category entity)
        {

            var responseDto = new CategoryResponseDto()
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                ProductCount = entity.ProductCount,
                ImageURL = entity.ImageURL,
                MerchantId = entity.MerchantId,
                Description = entity.Description,
                CategoryType = entity.CategoryType,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt

            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<CategoryTypeResponseDto> ToCategoryTypeResponseDto(CategoryType entity)
        {

            var responseDto = new CategoryTypeResponseDto()
            {
                Id = entity.Id,
                Name = entity.Name,
                EnName = entity.EnName,
                Type = (CategoryTypeEnum)entity.Type,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }
        
        public async Task<InvoiceParametersResponseDto> ToInvoiceParameterResponseDto(InvoiceCustomParameter entity)
        {

            var responseDto = new InvoiceParametersResponseDto()
            {
                Id = entity.Id,
                ParameterName = entity.ParameterName,
                MerchantId = entity.MerchantId,
                ParameterType = (InvoiceCustomParametersTypeEnum)entity.ParameterType,
                IsActive = entity.IsActive,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
            await Task.CompletedTask;
            return responseDto;

        }

        public async Task<IList<T>> ConvertToListResponseDto<T,R>(IList<R> lst) where T : class
        {
            try
            {
                switch (lst)
                {
                    case IList<InvoiceModel> e:
                        return (await ToListInvoiceResponseDto(e)) as IList<T>;
                    case IList<OrderModel> e:
                        return (await ToListOrderResponseDto(e)) as IList<T>;
                    case IList<Payment> e:
                        return (await ToListPaymentResponseDto(e)) as IList<T>;
                    case IList<Merchant> e:
                        return (await ToListMerchantResponseDto(e)) as IList<T>;
                    case IList<RequestPayment> e:
                        return (await ToListRequestPayResponseDto(e)) as IList<T>;
                    case IList<PaymentMethod> e:
                        return (await ToListPaymentMethodsResponseDto(e)) as IList<T>;

                }
                throw new Exception(" not Found Tharawat Models , Source should be in Tharawat Api Models");
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public async Task<IList<InvoiceResponseDto>> ToListInvoiceResponseDto(IList<InvoiceModel> invEntityList)
        {
            var invoiceList = new List<InvoiceResponseDto>();
            foreach (var invoice in invEntityList)
            {
                var responseDto = new InvoiceResponseDto()
                {
                    Id = invoice.Id,
                    MerchantId = invoice.MerchantId,
                    PaymentToken = invoice.PaymentToken,
                    InvoiceNumber = invoice.InvoiceNumber,
                    CategoryType = invoice.CategoryType,
                    Customer = string.IsNullOrEmpty(invoice.Customer) ? null : JSONHelper<CustomerInfo>.GetTypedModel(invoice.Customer),
                    OrderId = int.Parse(invoice.OrderId.ToString()),
                    TotalAmountDue = invoice.TotalAmountDue,
                    AmountPaid = invoice.AmountPaid,
                    AmountRemaining = invoice.AmountRemaining,
                    AmountShipping = invoice.AmountShipping,
                    Discount = invoice.Discount,
                    CurrencyCode = invoice.CurrencyCode,
                    Products = string.IsNullOrEmpty(invoice.Products) ? null : JSONHelper<List<ProductItems>>.GetTypedModel(invoice.Products),
                    CustomParameters = string.IsNullOrEmpty(invoice.CustomParameters) ? null : JSONHelper<List<InvoiceCustomValueResponse>>.GetTypedModel(invoice.CustomParameters),
                    //AcceptedCurrencies = JSONHelper<string[]>.GetJSONStr(requestDto.AcceptedCurrencies) ?? "",
                    PaymentMethods = string.IsNullOrEmpty(invoice.PaymentMethods) ? null : JSONHelper<string[]>.GetTypedModel(invoice.PaymentMethods),
                    Notification = string.IsNullOrEmpty(invoice.Notification) ? null : JSONHelper<Notification>.GetTypedModel(invoice.Notification),
                    Status = invoice.Status,
                    Description = invoice.Description ?? "",
                    OrderReference = invoice.OrderReference ?? "",
                    CreatedAt = invoice.CreatedAt,
                    UpdatedAt = invoice.UpdatedAt
                };

                invoiceList.Add(responseDto);
            }

            await Task.CompletedTask;
            return invoiceList;

        }

        public async Task<IList<RequestPaymentResponseDto>> ToListRequestPayResponseDto(IList<RequestPayment> reqPayEntityList)
        {
            var reqPayList = new List<RequestPaymentResponseDto>();
            foreach (var reqPay in reqPayEntityList)
            {
                var responseDto = new RequestPaymentResponseDto()
                {
                    Id = reqPay.Id,
                    MerchantId = reqPay.MerchantId,
                    RequestPaymentReference = reqPay.RequestPaymentReference,
                    CategoryType = reqPay.CategoryType,
                    Customer = string.IsNullOrEmpty(reqPay.Customer) ? null : JSONHelper<CustomerInfo>.GetTypedModel(reqPay.Customer),
                    TotalAmount = reqPay.TotalAmount,
                    AmountPaid = reqPay.AmountPaid,
                    CurrencyCode = reqPay.CurrencyCode,
                    PaymentMethod = reqPay.PaymentMethod ?? string.Empty,
                    Status = reqPay.Status,
                    Description = reqPay.Description ?? string.Empty,
                    CreatedAt = reqPay.CreatedAt,
                    UpdatedAt = reqPay.UpdatedAt
                };

                reqPayList.Add(responseDto);
            }

            await Task.CompletedTask;
            return reqPayList;

        }

        public async Task<IList<MerchantResponseDto>> ToListMerchantResponseDto(IList<Merchant> merchantEntityList)
        {
            var merchantList = new List<MerchantResponseDto>();
            foreach (var entity in merchantEntityList)
            {
                var responseDto = new MerchantResponseDto()
                {
                    Id = entity.Id,
                    ProfileId = entity.ProfileId,
                    ArabicName = entity.ArabicName,
                    EnglishName = entity.EnglishName,
                    Phone = entity.Phone,
                    Email = entity.Email,
                    Address = entity.Address,
                    InvoicePrefix = entity.InvoicePrefix,
                    CategoryType = entity.CategoryType,
                    IntegrationType = entity.IntegrationType,
                    AccessChannel = entity.AccessChannel,
                    IsActive = entity.IsActive,
                    Details = entity.Details,
                    BusinessCategory = entity.BusinessCategory,
                    LogoImageUrl = entity.LogoImageUrl,
                    BusinessDescription = entity.BusinessDescription,
                    WebsiteUrl = entity.WebsiteUrl,
                    SocialMedia = string.IsNullOrEmpty(entity.SocialMedia) ? null : JSONHelper<Dictionary<string, string>>.GetTypedModel(entity.SocialMedia),
                    StoreLocation = string.IsNullOrEmpty(entity.StoreLocation) ? null : JSONHelper<Location>.GetTypedModel(entity.StoreLocation),
                    OperatingHours = string.IsNullOrEmpty(entity.OperatingHours) ? null : JSONHelper<Dictionary<string, string>>.GetTypedModel(entity.OperatingHours),
                    CustomerReviews = string.IsNullOrEmpty(entity.CustomerReviews) ? null : JSONHelper<List<Review>>.GetTypedModel(entity.CustomerReviews),
                    MarkdownContent = entity.MarkdownContent,
                    CreatedAt = entity.CreatedAt
                };

                merchantList.Add(responseDto);
            }

            await Task.CompletedTask;
            return merchantList;

        }

        public async Task<IList<GetOrderResponseDto>> ToListOrderResponseDto(IList<OrderModel> orderEntityList)
        {
            var orderList = new List<GetOrderResponseDto>();
            foreach (var order in orderEntityList)
            {
                var responseDto = new GetOrderResponseDto()
                {
                    Id = order.Id,
                    MerchantId = order.MerchantId,
                    TotalAmount = order.TotalAmount,
                    CurrencyCode = order.CurrencyCode,
                    OrderReference = order.OrderReference,
                    CategoryType = order.CategoryType,
                    Products = string.IsNullOrEmpty(order.Products) ? null : JSONHelper<List<ProductItems>>.GetTypedModel(order.Products),
                    CustomerInfo = string.IsNullOrEmpty(order.CustomerInfo) ? null : JSONHelper<CustomerInfo>.GetTypedModel(order.CustomerInfo),
                    Status = order.Status,
                    Description = order.Description,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt

                };

                orderList.Add(responseDto);
            }

            await Task.CompletedTask;
            return orderList;

        }

        public async Task<IList<PaymentResponseDto>> ToListPaymentResponseDto(IList<Payment> paymentEntityList)
        {
            var paymentList = new List<PaymentResponseDto>();
            foreach (var payment in paymentEntityList)
            {
                var responseDto = new PaymentResponseDto()
                {
                    Id = payment.Id,
                    TargetReference = payment.TargetReference ?? "",
                    TransactionReference = payment.TransactionReference,
                    PaymentReference = payment.PaymentReference,
                    CustomerInfo = string.IsNullOrEmpty(payment.CustomerInfo) ? null : JSONHelper<CustomerInfo>.GetTypedModel(payment.CustomerInfo),
                    AmountDue = payment.AmountDue,
                    CurrencyCode = payment.CurrencyCode,
                    MerchantId = payment.MerchantId,
                    OrderReference = payment.OrderReference,
                    InvoiceReference = payment.InvoiceReference,
                    RequestPaymentReference = payment.RequestPaymentReference,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.PaymentStatus,
                    PaymentType = payment.PaymentType,
                    Details = payment.Details,
                    IssueDate = payment.IssueDate,
                    PayDate = payment.PayDate

                };

                paymentList.Add(responseDto);
            }

            await Task.CompletedTask;
            return paymentList;

        }

        public async Task<IList<PaymentMethodsResponseDto>> ToListPaymentMethodsResponseDto(IList<PaymentMethod> paymentMethodList)
        {
            var payMethodList = new List<PaymentMethodsResponseDto>();
            foreach (var paymentMethod in paymentMethodList)
            {
                var responseDto = new PaymentMethodsResponseDto()
                {
                    Code = paymentMethod.Code,
                    Name = paymentMethod.Name,
                    NameEn = paymentMethod.NameEn,
                    ImageUrl = paymentMethod.ImageUrl,
                    SectorType = paymentMethod.SectorType
                };

                payMethodList.Add(responseDto);
            }

            await Task.CompletedTask;
            return payMethodList;

        }
    }
}
