using Application.IBusinessIndependentService;
using Application.Utils;
using Application.Utils.CustomException;
using Domain.Models.GrpcDto.RequestDto;
using Domain.Models.GrpcDto.ResponseDto;
using Domain.Utils;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Connectivity
{
    public class CommunicationWithRatingSysImpl : ICommunicationWithRatingSystem
    {
        private HttpClient hclient;
        public CommunicationWithRatingSysImpl()
        {

            X509Certificate2 rsaCertificate = new X509Certificate2(
                ConfigHelper.Configuration.GetSection("RatingSettings").GetSection("CertificateSetting").GetSection("Path").Value,
                ConfigHelper.Configuration.GetSection("RatingSettings").GetSection("CertificateSetting").GetSection("Password").Value);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                bool.Parse(ConfigHelper.Configuration.GetSection("RatingSettings").GetSection("CertificateSetting").GetSection("Http2UnencryptedSupport").Value));
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support",
                bool.Parse(ConfigHelper.Configuration.GetSection("RatingSettings").GetSection("CertificateSetting").GetSection("Http2Support").Value));
            var handler = new HttpClientHandler();

            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            handler.ClientCertificates.Add(rsaCertificate);
            //ServerCertificateCustomValidationCallback = (a, b, c, d) => true                         

            hclient = new HttpClient(handler);
        }

        public async Task<ServiceRatingResponseDto> GetServiceRatingAsync(ServiceRatingRequestDto requestDto)
        {
            ServiceRatingResponseDto result = null;
            try
            {
                // The port number must match the port of the gRPC server.
                //Get Env Var for Rating Core URL
                String url = Environment.GetEnvironmentVariable("ServicesPayAPIRatCoreURL", EnvironmentVariableTarget.Machine);
                //string url = ConfigHelper.Configuration.GetSection("RatingSettings").GetSection("URL").Value;
                using var channel = GrpcChannel.ForAddress(url, new GrpcChannelOptions() { HttpClient = hclient });
                var client = new Rating.RatingClient(channel);
                var reply = await client.GetServiceRatingAsync(
                                new ServiceRatingCommand
                                {
                                    CustomerProfileId = requestDto.CustomerProfileId,
                                    ProviderProfileId = requestDto.ProviderProfileId,
                                    CurrencyCode = requestDto.CurrencyCode,
                                    Amount = requestDto.Amount,
                                    CompanyCode = requestDto.CompanyCode,
                                    ServiceCode = requestDto.ServiceCode,
                                    TransactionDate = DateTime.Now.ToString(),
                                    ServiceType = requestDto.ServiceType,

                                });

                result = new ServiceRatingResponseDto ()
                {
                    Success = bool.Parse(reply.Success), 
                    Message = reply.Message,
                    ResponseCode = reply.ResponseCode,
                    TotalAmount = reply.Data.TotalAmount,
                    Currency = reply.Data.Currency,
                    Amount = reply.Data.Amount,
                    CommissionTotal = reply.Data.Commission,
                    CommissionCurrency = reply.Data.CommissionCurrency,
                    FeesTotal = reply.Data.Fees,
                    FeesCurrency = reply.Data.FeesCurrency,
                    ExtraTotal = reply.Data.Extra,
                    ExtraCurrency = reply.Data.ExtraCurrency,
                    TotalRate = reply.Data.TotalRate,
                    FeesDetail = reply.Data.FeesDetails.Select(x=> MapToList(x)).ToList(),
                    CommissionDetails = reply.Data.CommissionDetails.Select(x=> MapToList(x)).ToList(),
                    ExtraDetails = reply.Data.ExtraDetails.Select(x=> MapToList(x)).ToList()
                   
                };

                if (result.Success == false)
                    throw new TharawatAccountingSysException(result.Message, int.Parse(result.ResponseCode));
                else
                    result.Message = Constants.SUCCESS_Message;

            }
            catch (Exception ex)
            {
                // FIXME: replace error code with network connectivity error code
                if (ex.HResult == 4055 || ex.HResult == 4056)
                    throw;
                if (ex.HResult == 4023)
                    throw;
                throw new TharawatAccountingSysException(Constants.SERVER_ERROR_MSG, Constants.RATING_SYSTEM_ERROR_CODE);
            }

            return result;
        }

        Domain.Models.GrpcDto.ResponseDto.DetailsReply MapToList (DetailsReply detailsReply)
        {
            return new Domain.Models.GrpcDto.ResponseDto.DetailsReply
            {
                Id = detailsReply.Id,
                Name = detailsReply.Name,
                Amount = detailsReply.Amount,
                Currency = detailsReply.Currency,
                CalculationType = detailsReply.CalculationType,
                ServideRatingCode = detailsReply.ServideRatingCode,
                RatingPoliceName = detailsReply.RatingPoliceName,
            };
        }
    }
}
