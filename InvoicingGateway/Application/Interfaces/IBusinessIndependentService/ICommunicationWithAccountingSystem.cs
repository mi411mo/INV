using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IBusinessIndependentService
{
    public interface ICommunicationWithAccountingSystem
    {
        public Task<AccountResult> CreateWithdrwalRequestAsync(string profileId, decimal amount, string currencyCode, string companyCode
            , string ServiceCode,
            string TransactionReference, string TransactionType, string TransactionSubType, string TransactionDate, string RequestId
            , string CreatedBy, string ClientId,
            string ServiceType);
        public Task<AccountResult> WithdrawalAcknowledgeCompletionAsync(string profileId, decimal amount, string currencyCode, string status,
            string transactionReferece, string requestId);
        public Task<AccountResult> AddVoucherTransactionAccountAsync(string profileId, string targetId, decimal amount, string companyCode, string serviceCode,
            string transactionSubType, string currencyCode, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId,
            string serviceType, string clientId, string customerId, decimal feesTotal, string feesDetails, decimal commissionTotal, string commisionDetails, decimal extraTotal, string extraDetails, string voucherTransType, string userId = null, string details = null);
        public Task<AccountResult> SaveVoucherTransactionAccountAsync(string profileId, string targetId, decimal amount, string companyCode, string serviceCode,
           string transactionSubType, string currencyCode, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId, string serviceType, string clientId, string customerId,
           decimal feesTotal, string feesDetails, decimal commissionTotal, string commisionDetails, decimal extraTotal, string extraDetails, string voucherTransType, string userId = null, string details = null, string statement = null, string receiver = null);
        public Task<AccountResult> SaveVoucherTransactionAsync(string profileId, string targetId, decimal amount, string currencyCode, string companyCode, string serviceCode, decimal customerAmount, string custTransactionSubType, string custTransType, string custStatement, decimal providerAmount,
            string providerTransactionSubType, string providerTransType, string providerStatement, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId, string serviceType, string clientId, string customerId, string feesTotal, string feesCurrency,
            string feesDetails, string commissionTotal, string commisionCurrency, string commisionDetails, string extraTotal, string extraCurrency, string extraDetails, string userId = null, string details = null, string statement = null, string receiver = null);
        public Task<AccountResult> CreateDepositRequestAsync(string profileId, decimal amount, string currencyCode, string companyCode
            , string ServiceCode,
            string TransactionReference, string TransactionType, string TransactionSubType, string TransactionDate, string RequestId
            , string CreatedBy, string ClientId,
            string ServiceType);
        public Task<AccountResult> DepositAcknowledgeCompletionAsync(string profileId, decimal amount, string currencyCode, string status, string transactionReferece, string requestId);
        public Task<AccountResult> QueryBalanceAsync(string profileId, string currencyCode);

    }

    public class AccountResult
    {
        public bool Success { get; set; }
        public string message { get; set; }
        public string responseCode { get; set; }
        public decimal balance { get; set; }
        public decimal withdrawalAvailableBalance { get; set; }
        public decimal withdrawalOnholdBalance { get; set; }
        public decimal depositAvailableBalance { get; set; }
        public decimal depositOnholdBalance { get; set; }


        public override string ToString()
        {
            string result = JsonConvert.SerializeObject(this, Formatting.Indented);
            return result;
        }
    }
}
