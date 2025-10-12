using Application.IBusinessIndependentService;
using Application.Utils;
using Application.Utils.CustomException;
using Domain.Utils;
using Grpc.Net.Client;
using Infrastructure;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Connectivity.CommunicationWithAccountingSysImpl
{
    // 40056 code limits "لقد إستنفدت عدد العمليات المسموح لك خلال اليوم. لا يمكنك إجراء اكثر من 10 عملية خلال اليوم"
    public class CommunicationWithAccountingSysImpl : ICommunicationWithAccountingSystem
    {
        private HttpClient hclient;
        public CommunicationWithAccountingSysImpl()
        {

            X509Certificate2 rsaCertificate = new X509Certificate2(
                ConfigHelper.Configuration.GetSection("Accounting").GetSection("CertificateSetting").GetSection("Path").Value,
                ConfigHelper.Configuration.GetSection("Accounting").GetSection("CertificateSetting").GetSection("Password").Value);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                bool.Parse(ConfigHelper.Configuration.GetSection("Accounting").GetSection("CertificateSetting").GetSection("Http2UnencryptedSupport").Value));
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support",
                bool.Parse(ConfigHelper.Configuration.GetSection("Accounting").GetSection("CertificateSetting").GetSection("Http2Support").Value));
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            handler.ClientCertificates.Add(rsaCertificate);
            //ServerCertificateCustomValidationCallback = (a, b, c, d) => true                         

            hclient = new HttpClient(handler);
        }

        public async Task<AccountResult> CreateWithdrwalRequestAsync(string profileId, decimal amount, string currencyCode, string companyCode, string ServiceCode,
           string TransactionReference, string TransactionType, string TransactionSubType, string TransactionDate, string RequestId, string CreatedBy, string ClientId,
           string ServiceType)
        {
            AccountResult result = null;
            try
            {

                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.CreateWithdrawalAsync(
                                  new CreateWithdrawalCommand
                                  {
                                      ProfileId = profileId,
                                      Amount = amount.ToString(),
                                      Currency = currencyCode,
                                      CompanyCode = companyCode,
                                      ServiceCode = ServiceCode,
                                      TransactionReference = TransactionReference,
                                      //TransactionType = TransactionType,
                                      TransactionSubType = TransactionType,
                                      TransactionDate = TransactionDate,
                                      RequestId = RequestId,
                                      CreatedBy = CreatedBy,
                                      ClientId = ClientId,
                                      ServiceType = ServiceType
                                  });

                result = new AccountResult { Success = bool.Parse(reply.Success), message = reply.Message, responseCode = reply.ResponseCode };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;

            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                if (ex.HResult == 4055 || ex.HResult == 4056)
                    throw;
                if (ex.HResult == 4023)
                    throw;
                throw new TharawatAccountingSysException(Constants.SERVER_ERROR_MSG, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }

        public async Task<AccountResult> WithdrawalAcknowledgeCompletionAsync(string profileId, decimal amount, string currencyCode, string status
            , string transactionReferece, string requestId)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.WithdrawalAcknowledgeCompletionAsync(
                                  new WithdrawalAcknowledgeCompletionCommand {
                                      ProfileId = profileId,
                                      Amount = amount.ToString(),
                                      Currency = currencyCode ,
                                      Status = status,
                                      TransactionReference = transactionReferece,
                                      RequestId = requestId
                                  });

                result = new AccountResult 
                { 
                    Success = bool.Parse(reply.Success),
                    message = reply.Message, responseCode = reply.ResponseCode,
                    balance = decimal.Parse(reply.Balance),
                    withdrawalAvailableBalance = decimal.Parse(reply.WithdrawalAvailableBalance),
                    withdrawalOnholdBalance = decimal.Parse(reply.WithdrawalOnHold)
                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }
        public async Task<AccountResult> AddVoucherTransactionAccountAsync(string profileId, string targetId, decimal amount, string companyCode, string serviceCode, string transactionSubType,
            string currencyCode, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId, string serviceType, string clientId, 
            string customerId, decimal feesTotal = 0, string feesDetails ="", decimal commissionTotal = 0, string commisionDetails = "", decimal extraTotal = 0, string extraDetails = "", string voucherTransType = "DEBIT", string userId = "" , string details= null)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.AddVoucherTransactionAsync(
                                  new VoucherTransactionCommand
                                  {
                                      SourceProfileId = profileId,
                                      TargetProfileId = targetId,
                                      Amount = amount.ToString(),
                                      CompanyCode = companyCode,
                                      ServiceCode = serviceCode,
                                      TransactionSubType = transactionSubType,
                                      CurrencyCode = currencyCode,
                                      TransactionId = transactionReferece,
                                      TargetReference = agentTransactionRef??"",
                                      SourceReference = referemceNumber,
                                      Status = status,
                                      RequestId = requestId,
                                      ServiceType = serviceType,
                                      ClientId = clientId,
                                      FeesTotal = feesTotal.ToString(),
                                      FeesDetails = feesDetails,
                                      CommissionTotal = commissionTotal.ToString(),
                                      CommissionDetails = commisionDetails??"",
                                      ExtraTotal = extraTotal.ToString(),
                                      ExtraDetails = extraDetails??"",
                                      TransactionType = voucherTransType

                                  });

                result = new AccountResult
                {
                    Success = bool.Parse(reply.Success),
                    message = reply.Message,
                    responseCode = reply.ResponseCode,
                    
                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }
        public async Task<AccountResult> SaveVoucherTransactionAccountAsync(string profileId, string targetId, decimal amount, string companyCode, string serviceCode, string transactionSubType,
            string currencyCode, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId, string serviceType, string clientId,
            string customerId, decimal feesTotal = 0, string feesDetails = "", decimal commissionTotal = 0, string commisionDetails = "", decimal extraTotal = 0, string extraDetails = "", string voucherTransType = "DEBIT", string userId = "", string details = null, string statement = null, string receiver = null)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.SaveVoucherTransactionAsync(
                                  new VoucherTransactionCommand2
                                  {
                                      SourceProfileId = profileId,
                                      TargetProfileId = targetId,
                                      Amount = amount.ToString(),
                                      CompanyCode = companyCode,
                                      ServiceCode = serviceCode,
                                      TransactionSubType = transactionSubType,
                                      CurrencyCode = currencyCode,
                                      TransactionId = transactionReferece,
                                      TargetReference = agentTransactionRef ?? "",
                                      SourceReference = referemceNumber,
                                      Status = status,
                                      RequestId = requestId,
                                      ServiceType = serviceType,
                                      ClientId = clientId,
                                      FeesTotal = feesTotal.ToString(),
                                      FeesDetails = feesDetails,
                                      CommissionTotal = commissionTotal.ToString(),
                                      CommissionDetails = commisionDetails ?? "",
                                      ExtraTotal = extraTotal.ToString(),
                                      ExtraDetails = extraDetails ?? "",
                                      TransactionType = voucherTransType,
                                      ReceiverDetails = receiver ?? "",
                                      Statement = statement ?? ""

                                  });

                result = new AccountResult
                {
                    Success = bool.Parse(reply.Success),
                    message = reply.Message,
                    responseCode = reply.ResponseCode,

                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }
        public async Task<AccountResult> CreateDepositRequestAsync(string profileId, decimal amount, string currencyCode, string companyCode, string ServiceCode,
           string TransactionReference, string TransactionType, string TransactionSubType, string TransactionDate, string RequestId, string CreatedBy, string ClientId,
           string ServiceType)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.CreateDepositingAsync(
                                  new CreateDepositCommand
                                  {
                                      ProfileId = profileId,
                                      Amount = amount.ToString(),
                                      Currency = currencyCode,
                                      CompanyCode = companyCode,
                                      ServiceCode = ServiceCode,
                                      TransactionReference = TransactionReference,
                                      //TransactionType = TransactionType,
                                      TransactionSubType = TransactionType,
                                      TransactionDate = TransactionDate,
                                      RequestId = RequestId,
                                      CreatedBy = CreatedBy,
                                      ClientId = ClientId,
                                      ServiceType = ServiceType
                                  });

                result = new AccountResult { Success = bool.Parse(reply.Success), message = reply.Message, responseCode = reply.ResponseCode };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;

            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }

        public async Task<AccountResult> DepositAcknowledgeCompletionAsync(string profileId, decimal amount, string currencyCode, string status, string transactionReferece, string requestId)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.DepositingAcknowledgeCompletionAsync(
                                  new DepositAcknowledgeCompletionCommand { 
                                      ProfileId = profileId, 
                                      Amount = amount.ToString(), 
                                      Currency = currencyCode, 
                                      Status = status,
                                      TransactionReference = transactionReferece,
                                      RequestId = requestId
                                  });

                result = new AccountResult
                {
                    Success = bool.Parse(reply.Success),
                    message = reply.Message,
                    responseCode = reply.ResponseCode,
                    balance = decimal.Parse(reply.Balance),
                    depositAvailableBalance = decimal.Parse(reply.DepositAvailableBalance),
                    depositOnholdBalance = decimal.Parse(reply.DepositOnHold)
                };
                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }

        public async Task<AccountResult> QueryBalanceAsync(string profileId, string currencyCode)
        {
            AccountResult result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.QueryBalanceAsync(
                                  new QueryBalanceCommand { ProfileId = profileId, Currency = currencyCode });

                result = new AccountResult
                {
                    balance = decimal.Parse(reply.Balance),
                    withdrawalAvailableBalance = decimal.Parse(reply.WithdrawalAvailableBalance),
                    withdrawalOnholdBalance = decimal.Parse(reply.WithdrawalOnHold),
                    depositAvailableBalance = decimal.Parse(reply.DepositAvailableBalance),
                    depositOnholdBalance = decimal.Parse(reply.DepositOnHold),
                    message = reply.Message,
                    responseCode = reply.ResponseCode,
                    Success = bool.Parse(reply.Success)
                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }      

        public async Task<AccountResult> SaveVoucherTransactionAsync(string profileId, string targetId, decimal amount, string currencyCode, string companyCode, string serviceCode, decimal customerAmount, string custTransactionSubType, 
                 string custTransType, string custStatement, decimal providerAmount, string providerTransactionSubType, string providerTransType, string providerStatement, string transactionReferece, string agentTransactionRef, string referemceNumber, string status, string requestId, 
                 string serviceType, string clientId, string customerId, string feesTotal, string feesCurrency, string feesDetails, string commissionTotal, string commisionCurrency, string commisionDetails, string extraTotal, string extraCurrency,
                 string extraDetails, string userId = null, string details = null, string statement = null, string receiver = null)
        {
            AccountResult result = null;
            try
            {
                string accountCoreUrlEnvName = ConfigHelper.Configuration.GetSection("Accounting").GetSection("URL").Value;
                String url = Environment.GetEnvironmentVariable(accountCoreUrlEnvName, EnvironmentVariableTarget.Machine);
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Accounting.AccountingClient(channel);
                var reply = await client.SaveVoucherTransactionsAsync(
                                  new SaveVoucherTransactionCommand
                                  {
                                      SourceProfileId = profileId,
                                      TargetProfileId = targetId,
                                      Amount = amount.ToString(),
                                      CompanyCode = companyCode,
                                      ServiceCode = serviceCode,
                                      CustomerAmount = customerAmount.ToString(),
                                      CustomerTransactionSubType = custTransactionSubType,
                                      CustomerTransactionType = custTransType,
                                      CustomerStatement = custStatement ?? "",
                                      ProviderAmount = providerAmount.ToString(),
                                      ProviderTransactionSubType = providerTransactionSubType,
                                      ProviderTransactionType = providerTransType,
                                      ProviderStatement = providerStatement ?? "",
                                      CurrencyCode = currencyCode,
                                      TransactionId = transactionReferece,
                                      TargetReference = agentTransactionRef ?? "",
                                      SourceReference = referemceNumber,
                                      Status = status,
                                      RequestId = requestId,
                                      ServiceType = serviceType,
                                      ClientId = clientId,
                                      FeesTotal = feesTotal.ToString(),
                                      FeesCurrency = feesCurrency,
                                      FeesDetails = feesDetails,
                                      CommissionTotal = commissionTotal.ToString(),
                                      CommissionCurrency = commisionCurrency,
                                      CommissionDetails = commisionDetails ?? "",
                                      ExtraTotal = extraTotal.ToString() ?? "",
                                      ExtraCurrency = extraCurrency ?? "",
                                      ExtraDetails = extraDetails ?? "",                                      
                                      ReceiverDetails = receiver ?? "",
                                      Statement = statement ?? ""

                                  });

                result = new AccountResult
                {
                    Success = bool.Parse(reply.Success),
                    message = reply.Message,
                    responseCode = reply.ResponseCode,

                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.message, int.Parse(result.responseCode));
                else
                    result.message = Constants.SUCCESS_Message;
            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                throw new TharawatAccountingSysException(ex.Message, Constants.ACCOUNTING_SYSTEM_ERROR_CODE);
            }

            return result;
        }
    }
}
