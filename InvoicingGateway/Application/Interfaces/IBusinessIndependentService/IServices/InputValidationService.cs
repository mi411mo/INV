using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessIndependentService.IServices
{
    public interface InputValidationService
    {
        Task<string> Validate<T>(string jSONstring) where T : new();
    }
}
