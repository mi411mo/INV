using Application.DTOs.Base;
using Application.Utils;
using Domain.Entities;
using Domain.Models.Invoices.RequestDto;
using Domain.Models.Invoices.ResponseDto;
using Domain.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tharwat.Switch.Utilites.Application.Common.SystemEnviornment;

namespace Application.DTOs
{
    public class RedirectLinkPaymentsRequest
    {
        public string orderId { get; set; }
        public string requestId { get; set; }
        public string referenceNumber { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string redirectUrl { get; set; }
        public string postUrl { get; set; }
        public string eccomerceImage { get; set; }
        public string autherazationId { get; set; } = string.Empty;
        public string type { get; set; } = "Bill";
        public string merchantId { get; set; }
        public CustomerDetails customerDetails { get; set; }

        public RedirectLinkPaymentsRequest ToModel(InvoiceModel invoice, Merchant merchant, Payment payment)
        {
            var customerInfo = JsonConvert.DeserializeObject<CustomerInfo>(invoice.Customer);
            var imageUrl = ConfigHelper.Configuration.GetSection("ServiceURLs")["ImageUrl"];
            var invoiceRedirectUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["InvoiceRedirectUrl"];
            var postUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["PostUrl"];
            var invoiceRedirectUrl = SystemEnviornmentLookup.GetEnvVariableValue(invoiceRedirectUrlEnvName);
            var postUrl = SystemEnviornmentLookup.GetEnvVariableValue(postUrlEnvName);
            var payRedirectType = ConfigHelper.Configuration.GetSection("ServiceURLs")["PaymentRedirectType"];

            return new RedirectLinkPaymentsRequest()
            {
                orderId = invoice.InvoiceNumber,
                requestId = payment.RequestId,
                referenceNumber = payment.PaymentReference,
                amount = invoice.TotalAmountDue,
                currency = invoice.CurrencyCode,
                eccomerceImage = imageUrl,
                redirectUrl = invoiceRedirectUrl.Trim(),
                postUrl = postUrl.Trim() + payment.PaymentReference.Trim(),
                //TODO: To change it to "Bill" when Payment Gateway accepts Bill type
                type = payRedirectType,
                merchantId = merchant.ProfileId,
                customerDetails = new CustomerDetails()
                {
                    name = customerInfo.CustomerName,
                    phoneNumber = customerInfo.Phone ?? "",
                    email = customerInfo.Email ?? ""
                }                
            };

        }

        public RedirectLinkPaymentsRequest ToModel(OrderModel order, Merchant merchant, Payment payment)
        {
            var customerInfo = JsonConvert.DeserializeObject<CustomerInfo>(order.CustomerInfo);
            var imageUrl = ConfigHelper.Configuration.GetSection("ServiceURLs")["ImageUrl"];
            var orderRedirectUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["OrderRedirectUrl"];
            var postUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["PostUrl"];
            var orderRedirectUrl = SystemEnviornmentLookup.GetEnvVariableValue(orderRedirectUrlEnvName);
            var postUrl = SystemEnviornmentLookup.GetEnvVariableValue(postUrlEnvName);
            var payRedirectType = ConfigHelper.Configuration.GetSection("ServiceURLs")["PaymentRedirectType"];

            return new RedirectLinkPaymentsRequest()
            {
                orderId = order.OrderReference,
                requestId = payment.RequestId,
                referenceNumber = payment.PaymentReference,
                amount = order.TotalAmount,
                currency = order.CurrencyCode,
                eccomerceImage = imageUrl,
                redirectUrl = orderRedirectUrl.Trim(),
                postUrl = postUrl.Trim() + payment.PaymentReference,
                type = payRedirectType,
                merchantId = merchant.ProfileId,
                customerDetails = new CustomerDetails()
                {
                    name = customerInfo.CustomerName,
                    phoneNumber = customerInfo.Phone ?? "",
                    email = customerInfo.Email ?? ""
                }
            };

        }

        public RedirectLinkPaymentsRequest ToModel(RequestPayment reqPay, Merchant merchant, Payment payment)
        {
            var customerInfo = string.IsNullOrEmpty(reqPay.Customer) ? new CustomerInfo() : JsonConvert.DeserializeObject<CustomerInfo>(reqPay.Customer);
            var imageUrl = ConfigHelper.Configuration.GetSection("ServiceURLs")["ImageUrl"];
            var orderRedirectUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["OrderRedirectUrl"];
            var postUrlEnvName = ConfigHelper.Configuration.GetSection("ServiceURLs")["PostUrl"];
            var orderRedirectUrl = SystemEnviornmentLookup.GetEnvVariableValue(orderRedirectUrlEnvName);
            var postUrl = SystemEnviornmentLookup.GetEnvVariableValue(postUrlEnvName);
            var payRedirectType = ConfigHelper.Configuration.GetSection("ServiceURLs")["PaymentRedirectType"];

            return new RedirectLinkPaymentsRequest()
            {
                orderId = reqPay.RequestPaymentReference,
                requestId = payment.RequestId,
                referenceNumber = payment.PaymentReference,
                amount = reqPay.TotalAmount,
                currency = reqPay.CurrencyCode,
                eccomerceImage = imageUrl,
                redirectUrl = orderRedirectUrl.Trim(),
                postUrl = postUrl.Trim() + payment.PaymentReference,
                type = payRedirectType,
                merchantId = merchant.ProfileId,
                customerDetails = new CustomerDetails()
                {
                    name = customerInfo.CustomerName ?? string.Empty,
                    phoneNumber = customerInfo.Phone ?? string.Empty,
                    email = customerInfo.Email ?? string.Empty
                }
            };

        }

    }
}
