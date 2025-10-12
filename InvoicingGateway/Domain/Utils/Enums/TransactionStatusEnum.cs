using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Utils.Enums
{
    public enum TransactionStatusEnum : byte
    {
        NotFoundLog,
        Success = 1,
        Pending = 2,
        Failure = 3,
        ExceptionProcess = 4,
        ExceptionGetURI,
        FailedCallApi = 6,
        ExceptionCallApi = 7,
        ExceptionFromResponse,
        FailedByResponse,
        FailedAccountSystem = 10,
        ExceptionAccountSystem,
        FailedAddReceivedRequest,
        FailedAddSentRequest = 13,
        FailedAddReceivedResponse,
        FailedAddSentResponse = 15,
        FailedAddSummary,
        ExceptionPostProcess = 17,
        UnKnownError,
        Exception = 19
    }
}
