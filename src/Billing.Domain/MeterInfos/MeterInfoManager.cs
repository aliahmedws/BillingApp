using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Billing.MeterInfos;

public class MeterInfoManager : DomainService
{
    private readonly IMeterInfoRepository _meterInfoRepository;
    private readonly ICurrentTenant _currentTenant;

    public MeterInfoManager(IMeterInfoRepository meterInfoRepository, ICurrentTenant currentTenant)
    {
        _meterInfoRepository = meterInfoRepository;
        _currentTenant = currentTenant;
    }

    public async Task<MeterInfo> CreateAsync(
        string meterNo,
        MeterType meterType,
        MeterCategory meterCategory,
        MeterStatus meterStatus,
        DateTime installationDate,
        decimal initialReading,
        Guid phaseId,
        Guid blockId,
        Guid plotId,
        Guid meterOwnerId,
        string? remarks = null)
    {
        Check.NotNullOrWhiteSpace(meterNo, nameof(meterNo));
        Check.Range(initialReading, nameof(initialReading), MeterInfoConsts.MinInitialReading, decimal.MaxValue);
        Check.NotNull(meterOwnerId, nameof(meterOwnerId));
        Check.NotNull(plotId, nameof(plotId));
        Check.NotNull(phaseId, nameof(phaseId));
        Check.NotNull(blockId, nameof(blockId));

        var existingMeter = await _meterInfoRepository.FindByMeterNoAsync(meterNo);
        if (existingMeter != null)
        {
            throw new MeterAlreadyExistsException(meterNo);
        }

        return new MeterInfo(
            GuidGenerator.Create(),
            meterNo,
            meterType,
            meterCategory,
            meterStatus,
            installationDate,
            initialReading,
            phaseId,
            blockId,
            plotId,
            meterOwnerId,
            remarks,
            _currentTenant.Id
        );
    }

    public async Task UpdateAsync(
        MeterInfo meter,
        string meterNo,
        MeterType meterType,
        MeterCategory meterCategory,
        MeterStatus meterStatus,
        DateTime installationDate,
        decimal initialReading,
        Guid phaseId,
        Guid blockId,
        Guid plotId,
        Guid meterOwnerId,
        string? remarks)
    {
        Check.NotNull(meter, nameof(meter));
        Check.NotNull(meterOwnerId, nameof(meterOwnerId));
        Check.NotNull(plotId, nameof(plotId));
        Check.NotNull(phaseId, nameof(phaseId));
        Check.NotNull(blockId, nameof(blockId));
        Check.NotNullOrWhiteSpace(meterNo, nameof(meterNo));

        var existingMeter = await _meterInfoRepository.FindByMeterNoAsync(meterNo);
        if (existingMeter != null && existingMeter.Id != meter.Id)
        {
            throw new MeterAlreadyExistsException(meterNo);
        }

        meter
            .ChangeType(meterType)
            .ChangeMeterCategory(meterCategory)
            .ChangeStatus(meterStatus)
            .ChangeInitialReading(initialReading)
            .ChangeRemarks(remarks)
            .SetTenant(_currentTenant.Id)
            .ChangeMeterOwner(meterOwnerId)
            .ChangeBlock(blockId)
            .ChangePhase(phaseId);
    }
}
