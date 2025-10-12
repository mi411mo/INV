using Domain.Entities;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.ResultUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IBusinessIndependentService.IServices
{
    public interface IAuditLogService<T>
    {
        Task<bool> CreateActionAsync(ProfileInfo profileInfo, T newEntity);
        Task<bool> UpdateActionAsync(ProfileInfo profileInfo, T oldEntity, T newEntity);
        Task<bool> UpdateStatusActionAsync(ProfileInfo profileInfo, string oldStatus, string newStatus);
        Task<bool> DeleteActionAsync(ProfileInfo profileInfo, T oldEntity);
        Task<IList<AuditLog>> GetAllAsync(GeneralFilterDto generalFilter);
        Task<int> GetCountAsync(GeneralFilterDto generalFilter);
    }
}
