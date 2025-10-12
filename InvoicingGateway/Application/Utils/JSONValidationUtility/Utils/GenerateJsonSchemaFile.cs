using Domain.Utils;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Services.RequestDto;
using Domain.Models.Orders.RequestDto;
using Domain.Models.Merchants;
using Domain.Models.Payments.RequestDto;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Customers;
using Domain.Models.Categories;
using Domain.Models.CategoryTypes;
using Domain.Models.InvoiceCustomParameters;
using Domain.Models.RequestPayments;

namespace Application.Utils.JSONValidationUtility.Utils
{
    public class GenerateJsonSchemaFile
    {
        private static void _GenerateSchemaFile()
        {
            try
            {
                JSchemaGenerator generator = new JSchemaGenerator();
                generator.ContractResolver = new CamelCasePropertyNamesContractResolver();

                JSchema stringModelSchema = generator.Generate(typeof(ProductRequestDto));
                string dTOModelSchema = stringModelSchema.ToString();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GeneralJSONSchemasPath2 = @"Application\Utils\JSONValidationUtility\Utils\JSONValidationUtility\Utils\JSONSchemas\";

        public static void WriteModelsSchemas()
        {

            try
            {

                foreach (string modelName in Constants.RequestModelsNames)
                {
                    if (!System.IO.File.Exists(Constants.ProductJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"ProductDtoJSchema.json", GenerateSchemaFile<ProductRequestDto>());

                    else if (!System.IO.File.Exists(Constants.OrderJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"OrderDtoJSchema.json", GenerateSchemaFile<OrderRequestDto>());
                    else if (!System.IO.File.Exists(Constants.CustomerJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"CustomerDtoJSchema.json", GenerateSchemaFile<CustomerRequestDto>());
                    else if (!System.IO.File.Exists(Constants.InvoiceJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"InvoiceDtoJSchema.json", GenerateSchemaFile<InvoiceRequestDto>());
                    else if (!System.IO.File.Exists(Constants.PaymentJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"PaymentDtoJSchema.json", GenerateSchemaFile<PaymentRequestDto>());
                    else if (!System.IO.File.Exists(Constants.MerchantJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"MerchantDtoJSchema.json", GenerateSchemaFile<MerchantRequestDto>());
                    else if (!System.IO.File.Exists(Constants.CategoryJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"CategoryDtoJSchema.json", GenerateSchemaFile<CategoryRequestDto>());
                    else if (!System.IO.File.Exists(Constants.CategoryTypeJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"CategoryTypeDtoJSchema.json", GenerateSchemaFile<CategoryTypeRequestDto>());
                    else if (!System.IO.File.Exists(Constants.InvoiceParametersJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"InvoiceParameterDtoJSchema.json", GenerateSchemaFile<InvoiceParametersRequestDto>());
                    else if (!System.IO.File.Exists(Constants.RequestPaymentJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"RequestPaymentDtoJSchema.json", GenerateSchemaFile<RequestPaymentRequestDto>());
                    else if (!System.IO.File.Exists(Constants.DirectPaymentJSONSchemasPath))
                        File.WriteAllText(Constants.GeneralJSONSchemasPath + @"DirectPaymentDtoJSchema.json", GenerateSchemaFile<DirectPaymentRequestDto>());

                }
            }
            catch (Exception) { throw; }

        }

        public static string GenerateSchemaFile<T>()
        {
            try
            {
                JSchemaGenerator generator = new JSchemaGenerator();
                generator.ContractResolver = new CamelCasePropertyNamesContractResolver();
                JSchema stringModelSchema = generator.Generate(typeof(T));
                string dTOModelSchema = stringModelSchema.ToString();
                return dTOModelSchema;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
