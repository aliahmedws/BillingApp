using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.MeterInfos;

[RemoteService(IsEnabled = false)]
[Authorize(BillingPermissions.MeterInfos.Default)]
public class MeterInfoAppService : BillingAppService, IMeterInfoAppService
{
    private readonly IMeterInfoRepository _meterInfoRepository;
    private readonly MeterInfoManager _meterInfoManager;

    public MeterInfoAppService(
        IMeterInfoRepository meterInfoRepository,
        MeterInfoManager meterInfoManager)
    {
        _meterInfoRepository = meterInfoRepository;
        _meterInfoManager = meterInfoManager;
    }

    [Authorize(BillingPermissions.MeterInfos.Create)]
    public async Task<MeterInfoDto> CreateAsync(CreateMeterInfoDto input)
    {
        var meter = await _meterInfoManager.CreateAsync(
            input.MeterNo,
            input.MeterType,
            input.MeterCategory,
            input.MeterStatus,
            input.InstallationDate,
            input.InitialReading,
            input.PhaseId,
            input.BlockId,
            input.PlotId,
            input.MeterOwnerId,
            input.Remarks
        );

        await _meterInfoRepository.InsertAsync(meter);
        return ObjectMapper.Map<MeterInfo, MeterInfoDto>(meter);
    }

    [Authorize(BillingPermissions.MeterInfos.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _meterInfoRepository.DeleteAsync(id);
    }

    public async Task<MeterInfoDto> GetAsync(Guid id)
    {
        var meter = await _meterInfoRepository.GetAsync(id);
        return ObjectMapper.Map<MeterInfo, MeterInfoDto>(meter);
    }

    public async Task<PagedResultDto<MeterInfoDto>> GetListAsync(GetMeterInfoListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(MeterInfo.MeterNo);
        }

        var items = await _meterInfoRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting!,
            input.Filter,
            input.MeterNo,
            input.MeterType,
            input.MeterCategory,
            input.MeterStatus,
            input.InstallationDate,
            input.PhaseId,
            input.PlotId,
            input.MeterOwnerId
        );

        var totalCount = await _meterInfoRepository.GetCountAsync(
            input.Filter,
            input.MeterNo,
            input.MeterType,
            input.MeterCategory,
            input.MeterStatus,
            input.InstallationDate,
            input.PhaseId,
            input.PlotId,
            input.MeterOwnerId
        );

        var dtos = ObjectMapper.Map<List<MeterInfo>, List<MeterInfoDto>>(items);

        return new PagedResultDto<MeterInfoDto>(totalCount, dtos);
    }

    [Authorize(BillingPermissions.MeterInfos.Edit)]
    public async Task UpdateAsync(Guid id, UpdateMeterInfoDto input)
    {
        var meter = await _meterInfoRepository.GetAsync(id);

        await _meterInfoManager.UpdateAsync(
            meter,
            input.MeterNo,
            input.MeterType,
            input.MeterCategory,
            input.MeterStatus,
            input.InstallationDate,
            input.InitialReading,
            input.PhaseId,
            input.BlockId,
            input.PlotId,
            input.MeterOwnerId,
            input.Remarks
        );

        await _meterInfoRepository.UpdateAsync(meter);
    }
}

