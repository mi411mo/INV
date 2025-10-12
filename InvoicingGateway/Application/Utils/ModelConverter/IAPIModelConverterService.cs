using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils.ModelConverter
{
    public interface IAPIModelConverterService
    {
        public Task<TRequest> ConvertToEntityModel<TRequest>(Object request) where TRequest : class; //where TRequest : BaseRequest;
        public Task<TResponse> ConvertToResponseDto<TResponse>(Object entity) where TResponse : class;
        public Task<IList<T>> ConvertToListResponseDto<T, R>(IList<R> lst) where T : class;
        public Task<TResponseDto> ConvertToGatewayModel<TResponseDto, TResponse>(TResponse response, Object request) where TResponseDto : class;

    }
}
