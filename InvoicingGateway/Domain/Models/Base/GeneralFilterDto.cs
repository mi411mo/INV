using Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Base
{
    public class GeneralFilterDto
    {
        public string SearchKey { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Id { get; set; }
        public int Status { get; set; }
        public int ProductId { get; set; }
        public int Type { get; set; }
        public EntityTypeEnum EntityType { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public int CategoryType { get; set; }
        public int MerchantId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryIds { get; set; }
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string Name { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterName { get; set; }
        public string TableName { get; set; }
        public string EnName { get; set; }
        public string Phone { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CustomerName { get; set; }
        public string SourceId { get; set; }
        public string LogType { get; set; }
        public string? CustomerId { get; set; } = "0";
        public string CustomerPhone { get; set; }
        public string ProfileId { get; set; }
        public string OrderReference { get; set; }
        public string InvoiceNumber { get; set; }
        public string TransactionReference { get; set; }
        public string RequestPaymentReference { get; set; }
        public string PaymentReference { get; set; }
        public string InvoiceReference { get; set; }
        public string TargetReference { get; set; }
        public string CategoryName { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderBy { get; set; }
        public string OrderType { get; set; }
        public bool? IsActive { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public InvoiceStatusEnums InvoiceStatus { get; set; }
        public ProductsStatusEnum ProductStatus { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }

    }
}
