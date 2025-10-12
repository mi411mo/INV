using Application.Interfaces.IBusinessIndependentService.IServices;
using Domain.Entities;
using Domain.IRepository;
using Domain.Models;
using Domain.Models.Base;
using Domain.Utils.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceImpl
{
    public class AuditLogServiceImpl<T> : IAuditLogService<T> where T : class
    {
        private readonly IAuditLogRepo auditLogRepo;
        public AuditLogServiceImpl(IAuditLogRepo auditLogRepo)
        {
            this.auditLogRepo = auditLogRepo;
        }
       
        public async Task<bool> CreateActionAsync(ProfileInfo profileInfo, T newEntity)
        {
            try
            {
                /*var tableName = typeof(T).Name;
                var tableAttribuite = typeof(T).GetCustomAttributes<TableAttribute>();
                if(tableAttribuite != null)
                    tableName = tableAttribuite.FirstOrDefault().Name;*/

                var auditLog = new AuditLog(profileInfo.UserId, AuditLogEnum.Create.ToString(), typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault().Name, DateTime.Now, string.Empty, JsonConvert.SerializeObject(newEntity), "All Columns", string.Empty, profileInfo.ClientId, profileInfo.ProfileId);
                return await auditLogRepo.AddAuditLogAsync(auditLog);
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> DeleteActionAsync(ProfileInfo profileInfo, T oldEntity)
        {
            try
            {
                var auditLog = new AuditLog(profileInfo.UserId, AuditLogEnum.Delete.ToString(), typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault().Name, DateTime.Now, JsonConvert.SerializeObject(oldEntity), string.Empty, string.Empty, string.Empty, profileInfo.ClientId, profileInfo.ProfileId);
                return await auditLogRepo.AddAuditLogAsync(auditLog);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IList<AuditLog>> GetAllAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await auditLogRepo.GetAllAsync(generalFilter);
                return lst.Skip((generalFilter.PageNumber - 1) * generalFilter.PageSize)
                .Take(generalFilter.PageSize)
                .OrderByDescending(e => e.Id)
                .ToList();
            }
            catch (Exception ex)
            {
                return new List<AuditLog>();
            }
        }

        public async Task<int> GetCountAsync(GeneralFilterDto generalFilter)
        {
            try
            {
                var lst = await auditLogRepo.GetAllAsync(generalFilter);
                return lst.Count();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateActionAsync(ProfileInfo profileInfo, T oldEntity, T newEntity)
        {
            try
            {
                List<string> affectedColumns = GetPropertyDifferences<T>(oldEntity, newEntity);
                var auditLog = new AuditLog(profileInfo.UserId, AuditLogEnum.Update.ToString(), typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault().Name, DateTime.Now, JsonConvert.SerializeObject(oldEntity), JsonConvert.SerializeObject(newEntity), JsonConvert.SerializeObject(affectedColumns), string.Empty, profileInfo.ClientId, profileInfo.ProfileId);
                return await auditLogRepo.AddAuditLogAsync(auditLog);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static List<string> GetPropertyDifferences<T>(T oldEntity, T newEntity)
        {
            if (oldEntity == null || newEntity == null)
                return new List<string> { "One or both entities are Null" };

            List<string> differences = new List<string>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach(PropertyInfo property in properties)
            {
                object value1 = property.GetValue(oldEntity);
                object value2 = property.GetValue(newEntity);

                if(!Equals(value1, value2))
                {
                    differences.Add(property.Name);
                }
            }

            return differences;
        }

        public async Task<bool> UpdateStatusActionAsync(ProfileInfo profileInfo, string oldStatus, string newStatus)
        {
            try
            {
                var auditLog = new AuditLog(profileInfo.UserId, AuditLogEnum.Update.ToString(), typeof(T).GetCustomAttributes<TableAttribute>().FirstOrDefault().Name, DateTime.Now, oldStatus, newStatus, "Status", string.Empty, profileInfo.ClientId, profileInfo.ProfileId);
                return await auditLogRepo.AddAuditLogAsync(auditLog);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
