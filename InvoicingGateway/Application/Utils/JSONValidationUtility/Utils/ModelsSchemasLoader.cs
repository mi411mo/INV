using Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.JSONValidationUtility.Utils
{
    public class ModelsSchemasLoader
    {
        public static void loadModelsSchemas()
        {
            string JSONSchema = string.Empty;
            foreach (string modelName in Constants.RequestModelsNames)
            {
                if (modelName == Constants.OrderRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"OrderDtoJSchema.json");
                
                else if (modelName == Constants.ProductRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"ProductDtoJSchema.json");
                else if (modelName == Constants.CustomerRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"CustomerDtoJSchema.json");
                else if (modelName == Constants.MerchantRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"MerchantDtoJSchema.json");
                else if (modelName == Constants.PaymentRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"PaymentDtoJSchema.json");
                else if (modelName == Constants.InvoiceRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"InvoiceDtoJSchema.json");
                else if (modelName == Constants.CategoryTypeRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"CategoryTypeDtoJSchema.json");
                else if (modelName == Constants.InvoiceParameterRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"InvoiceParameterDtoJSchema.json");
                else if (modelName == Constants.RequestPaymentRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"RequestPaymentDtoJSchema.json");
                else if (modelName == Constants.DirectPaymentRequestModelName)
                    JSONSchema = File.ReadAllText(Constants.GeneralJSONSchemasPath + @"DirectPaymentDtoJSchema.json");


                Constants.RequestModelsNameSchemaMappingDic.Add(modelName, JSONSchema);

            }

        }
    }
}
