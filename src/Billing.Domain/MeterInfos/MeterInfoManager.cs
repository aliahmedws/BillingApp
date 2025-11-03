using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.MeterInfos;

public class MeterInfoManager : DomainService
{
    private readonly IMeterInfoRepository _meterInfoRepository;

    public MeterInfoManager(IMeterInfoRepository meterInfoRepository)
    {
        _meterInfoRepository = meterInfoRepository;
    }

    public async Task<MeterInfo> CreateAsync(
        string meterNo,
        MeterType meterType,
        MeterCategory meterCategory,
        MeterStatus meterStatus,
        DateTime installationDate,
        decimal initialReading,
        Guid phaseId,
        Guid plotId,
        string? remarks = null)
    {
        Check.NotNullOrWhiteSpace(meterNo, nameof(meterNo));
        Check.Range(initialReading, nameof(initialReading), MeterInfoConsts.MinInitialReading, decimal.MaxValue);

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
            plotId,
            remarks
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
        Guid plotId,
        string? remarks)
    {
        Check.NotNull(meter, nameof(meter));
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
            .ChangeRemarks(remarks);
    }
}
