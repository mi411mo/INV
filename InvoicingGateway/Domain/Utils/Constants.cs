using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils
{
    public class Constants
    {
        public const string SUCCESS_Message = "تمت العملية بنجاح";
        public const string FATORA_PENDING_MSG = "تم استلام الطلب. بانتظار تأكيد العملية";
        public const string ORDER_ID_NOT_AVAILABE = "لا يوجد طلب بهذا الرقم";
        public const string PAYMENT_ID_NOT_AVAILABE = "لا يوجد عملية دفع بهذا الرقم";
        public const string ORDER_REF_NOT_AVAILABE = "لا يوجد طلب بهذا المرجع";
        public const string ENTITY_TYPE_NOT_AVAILABE = "يجب تحديد نوع الكيان لإظهار الاحصائيات";
        public const string INVOICE_ORDER_NOT_AVAILABE = "لا يوجد طلب بهذا الرقم لاصدار الفاتورة";
        public const string INVOICE_NOT_AVAILABE = "لا يوجد فاتورة بهذا الرقم";
        public const string INVOICE_IS_ALREADY_PAID = "هذه الفاتورة مدفوعة";
        public const string INSUFFIENT_LESS_AMOUNT_Paid = "المبلغ المدخل أقل من مبلغ الفاتورة";
        public const string INSUFFIENT_GREATER_AMOUNT_Paid = "المبلغ المدخل أكبر من مبلغ الفاتورة";
        public const string ORDER_IS_ALREADY_PAID = "هذا الطلب قد تم دفعه";
        public const string REQUEST_PAYMENT_IS_ALREADY_PAID = "هذه المطالبة المالية مدفوعة";
        public const string REQUEST_PAYMENT_NOT_AVAILABE = "لا يوجد مطالبة بهذا الرقم";
        public const string INVOICE_UPDATED_SUCCESSFULLY = "تم تغيير حالة الفاتورة بنجاح";
        public const string REQUEST_PAYMENT_UPDATED_SUCCESSFULLY = "تم تغيير حالة المطالبة بنجاح";
        public const string PRODUCT_NOT_AVAILABE = "لا يوجد منتج بهذا الرقم";
        public const string Customer_NOT_AVAILABE = "لا يوجد عميل بهذا الرقم";
        public const string CATEGORY_NOT_AVAILABE = "لا يوجد فئة بهذا الرقم";
        public const string INVOICE_PARAMETER_NOT_AVAILABE = "لا يوجد متغير بهذا الرقم";
        public const string MERCHANT_NOT_AVAILABE = "لا يوجد تاجر بهذا الرقم";
        public const string MERCHANT_NOT_ACTIVE = "هذا التاجر غير مفعل";
        public const string PHONE_IS_DUPLICATED = "رقم الهاتف مستخدم من قبل. يرجى استخدام رقم هاتف اخر";
        public const string CATEGORY_NAME_IS_DUPLICATED = "اسم الفئة مستخدم من قبل. يرجى استخدام اسم اخر";
        public const string PARAMETER_NAME_IS_DUPLICATED = "اسم المتغير مستخدم من قبل. يرجى استخدام اسم اخر";
        public const string PARAMETER_VALUE_IS_DUPLICATED = "قيمة المتغير مستخدم من قبل لنفس الفاتورة";
        public const string GeneralUserErrorMessage = " .خطأ في إدخال البيانات، يرجى التواصل مع الدعم الفني";
        public const string JSONStringWrongFormatingMessage = "The JSON object is formated in a wrong way. Please insert a valid format.";
        public const string GenericErrorMessage = ". حـدث خطأ! يرجى المحاولة مرة أخرى";
        public const string REFERENCE_IS_DUPLICATED = "المرجع مستخدم من قبل. يرجى استخدام رقم مرجع اخر";
        public const string ID_IS_DUPLICATED = "رقم المعرف مستخدم من قبل. يرجى استخدام رقم معرف اخر";
        public const string EMAIL_IS_DUPLICATED = "الايميل مستخدم من قبل. يرجى استخدام ايميل اخر";
        public const string PROFILE_ID_IS_DUPLICATED = "رقم الحساب مستخدم من قبل. يرجى استخدام رقم حساب اخر";
        public const string FOREIGN_KEY_DELETE_VIOLATION_MESSAGE = "لا يمكن حذف هذاالسجل لأنه مرتبط بسجلات أخرى";
        public const string FOREIGN_KEY_INSERT_VIOLATION_MESSAGE = "لا يمكن اضافة هذاالسجل. ";
        public const string SERVER_ERROR_MSG = "حدث خطـأ! يرجى إعادة التفيذ بعد فترة";
        public const string PROFILE_ID_HEADER = "Profile_Id";
        public const string CLIENT_ID_HEADER = "Client_Id";
        public const string FATORA_PLATFORM = "Fatora Platform";

        public const string ADMIN_CUSTOMER_ID = "0";
        public const string THARWAT_SUCCESS_CODE = "2000";
        public const string FATORA_PENDING_CODE = "2001";
        public const string VALIDATION_ERROR_CODE = "4000";
        public const string SERVER_ERROR_CODE = "5000";
        public const string PROVIDER_ERROR_CODE = "300";
        public const string NotFoundResponseCode = "404";
        public const string CLIENT_ERROR_CODE = "4000";
        public const string API_REQUEST_TIMEOUT_ERROR_CODE = "7000";
        public const string THARAWAT_BAD_REQUEST_CODE = "7008";
        public const string THARAWAT_SYSTEM_NOT_RESPONSE_CODE = "7051";
        public const string API_MODEL_CONVERSION_ERROR_CODE = "7506";
        public const string API_DATABASE_SYSTEM_ERROR_CODE = "7500";
        public const int ACCOUNTING_SYSTEM_ERROR_CODE = 7501;
        public const int RATING_SYSTEM_ERROR_CODE = 7502;
        public const int DATABASE_SYSTEM_ERROR_CODE = 7503;
        public const int FOREIGN_KEY_VIOLATION_ERROR_CODE = 547;

        
        public const string ProductRequestModelName = "ProductRequest";
        public const string OrderRequestModelName = "OrderRequest";
        public const string CustomerRequestModelName = "CustomerRequest";
        public const string MerchantRequestModelName = "MerchantRequest";
        public const string InvoiceRequestModelName = "InvoiceRequest";
        public const string PaymentRequestModelName = "PaymentRequest";
        public const string CategoryRequestModelName = "CategoryRequest";
        public const string CategoryTypeRequestModelName = "CategoryTypeRequest";
        public const string InvoiceParameterRequestModelName = "InvoiceParameterRequest";
        public const string RequestPaymentRequestModelName = "RequestPaymentRequest";
        public const string DirectPaymentRequestModelName = "DirectPaymentRequest";


        public static List<string> RequestModelsNames = new List<string>()
        { ProductRequestModelName, OrderRequestModelName, CustomerRequestModelName,  MerchantRequestModelName, InvoiceRequestModelName, PaymentRequestModelName, CategoryRequestModelName, CategoryTypeRequestModelName, InvoiceParameterRequestModelName, RequestPaymentRequestModelName, DirectPaymentRequestModelName};



        public static string GeneralJSONSchemasPath = AppDomain.CurrentDomain.BaseDirectory + @"Utils\JSONValidationUtility\Utils\JSONSchemas\";
        public static Dictionary<string, string> RequestModelsNameSchemaMappingDic = new Dictionary<string, string>();
        public static string ProductJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "ProductDtoJSchema.json");
        public static string OrderJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "OrderDtoJSchema.json");
        public static string CustomerJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "CustomerDtoJSchema.json");
        public static string InvoiceJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "InvoiceDtoJSchema.json");
        public static string MerchantJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "MerchantDtoJSchema.json");
        public static string PaymentJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "PaymentDtoJSchema.json");
        public static string CategoryJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "CategoryDtoJSchema.json");
        public static string CategoryTypeJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "CategoryTypeDtoJSchema.json");
        public static string InvoiceParametersJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "InvoiceParameterDtoJSchema.json");
        public static string RequestPaymentJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "RequestPaymentDtoJSchema.json");
        public static string DirectPaymentJSONSchemasPath = System.IO.Path.Combine(Constants.GeneralJSONSchemasPath, "DirectPaymentDtoJSchema.json");

        //scopes
        public const string CONFIRM_INVOICE_SCOPE = "fatora.invoice.confirm";
        public const string CREATE_INVOICE_SCOPE = "fatora.invoice.create";
        public const string GETALL_INVOICE_SCOPE = "fatora.invoice.getall";
        public const string UPDATE_INVOICE_SCOPE = "fatora.invoice.update";
        public const string GETBYREFERENCE_INVOICE_SCOPE = "fatora.invoice.getbyreference";


    }
}
