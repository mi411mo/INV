using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImpl.DRY
{
    public class SqlConstants
    {
        public const string OrderTBName = "Orders";
        public const string InvoiceTBName = "Invoices";
        public const string ProductTBName = "Products";
        public const string CustomerTBName = "Customers";
        public const string MerchnatTBName = "Merchants";
        public const string PaymentTBName = "Payments";
        public const string CategoryTBName = "Categories";
        public const string CategoryTypeTBName = "CategoryTypes";
        public const string InvoiceCustomParametersTBName = "InvoiceCustomParameters";
        public const string InvoiceParameterValuesTBName = "InvoiceCustomValues";
        public const string RequestPaymentsTBName = "RequestPayments";
        public const string LogsTBName = "Logs";
        public const string AuditLogsTBName = "AuditLogs";
        
    }
}
