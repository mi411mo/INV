using Domain.Models.GrpcDto.RequestDto;
using Domain.Models.GrpcDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IBusinessIndependentService
{
    public interface ICommunicationWithRatingSystem
    {
        public Task<ServiceRatingResponseDto> GetServiceRatingAsync(ServiceRatingRequestDto requestDto);
    }
}
