using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.MeterInfos;

public interface IMeterInfoRepository : IRepository<MeterInfo, Guid>
{
    Task<MeterInfo?> FindByMeterNoAsync(string meterNo);

    Task<List<MeterInfo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? meterNo,
        MeterType? meterType,
        MeterCategory? meterCategory,
        MeterStatus? meterStatus,
        DateTime? installationDate,
        Guid? phaseId,
        Guid? plotId);
    Task<long> GetCountAsync(
        string? filter,
        string? meterNo,
        MeterType? meterType,
        MeterCategory? meterCategory,
        MeterStatus? meterStatus,
        DateTime? installationDate,
        Guid? phaseId,
        Guid? plotId);
}
