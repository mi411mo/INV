using Domain.Models.Categories;
using Domain.Models.CategoryTypes;
using Domain.Models.Customers;
using Domain.Models.InvoiceCustomParameters;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Merchants;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Payments.RequestDto;
using Domain.Models.RequestPayments;
using Domain.Models.Services.RequestDto;
using Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.JSONValidationUtility
{
    public class DtoModelSchemaGenerateor
    {
        public static string GenerateSchema<T>() where T : new()
        {
            string stringModelSchema = string.Empty;
            try
            {

                if (typeof(T) == typeof(ProductRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.ProductRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(OrderRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.OrderRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(InvoiceRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.InvoiceRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(MerchantRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.MerchantRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(CustomerRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.CustomerRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(PaymentRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.PaymentRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(CategoryRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.CategoryRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(CategoryTypeRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.CategoryTypeRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(InvoiceParametersRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.InvoiceParameterRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(RequestPaymentRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.RequestPaymentRequestModelName, out stringModelSchema);
                else if (typeof(T) == typeof(DirectPaymentRequestDto))
                    Constants.RequestModelsNameSchemaMappingDic.TryGetValue(Constants.DirectPaymentRequestModelName, out stringModelSchema);


                return stringModelSchema;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
