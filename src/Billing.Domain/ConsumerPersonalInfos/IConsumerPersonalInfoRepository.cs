using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.ConsumerPersonalInfos;

public interface IConsumerPersonalInfoRepository : IRepository<ConsumerPersonalInfo, Guid>
{
    Task<ConsumerPersonalInfo?> FindByCnicAsync(string cnic);
    Task<ConsumerPersonalInfo?> FindByPhoneAsync(string phone);
    Task<List<ConsumerPersonalInfo>> GetListAsync(
       int skipCount,
       int maxResultCount,
       string sorting,
       string? filter,
       string? firstName,
       string? lastName,
       string? cNIC,
       Gender? gender);
    Task<long> GetCountAsync(
       string? filter,
       string? firstName,
       string? lastName,
       string? cNIC,
       Gender? gender);
}
