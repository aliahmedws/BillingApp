using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.SocietyCharges;

[RemoteService(isEnabled: false)]
public class SocietyChargeAppService : BillingAppService, ISocietyChargeAppService
{
    private readonly ISocietyChargeRepository _societyChargeRepository;
    private readonly SocietyChargeManager _societyChargeManager;
    public SocietyChargeAppService(ISocietyChargeRepository societyChargeRepository, SocietyChargeManager societyChargeManager)
    {
        _societyChargeRepository = societyChargeRepository;
        _societyChargeManager = societyChargeManager;
    }

    public async Task<SocietyChargeDto> CreateAsync(CreateSocietyChargeDto input)
    {
        var totalSocietyCharges = 
            (input.SecurityCharges ?? 0) 
            + (input.MaintenanceCharges ?? 0) 
            + (input.WaterCharges ?? 0) 
            + (input.OtherCharges ?? 0);

        var societyCharge = await _societyChargeManager.CreateAsync(
            input.SecurityCharges,
            input.MaintenanceCharges,
            input.WaterCharges,
            input.OtherCharges,
            totalSocietyCharges
            );

        await _societyChargeRepository.InsertAsync(societyCharge);
        return ObjectMapper.Map<SocietyCharge, SocietyChargeDto>(societyCharge);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _societyChargeRepository.DeleteAsync(id);
    }

    public async Task<SocietyChargeDto> GetAsync(Guid id)
    {
        var societyCharge = await _societyChargeRepository.GetAsync(id);
        return ObjectMapper.Map<SocietyCharge, SocietyChargeDto>(societyCharge);
    }

    public async Task<PagedResultDto<SocietyChargeDto>> GetListAsync(GetSocietyChargeListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(SocietyCharge.TotalSocietyCharges);
        }
        var societyCharges = await _societyChargeRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.SecurityCharges,
            input.MaintenanceCharges,
            input.WaterCharges,
            input.OtherCharges,
            input.TotalSocietyCharges
        );
        var totalCount = await _societyChargeRepository.GetCountAsync(
            input.Filter,
            input.SecurityCharges,
            input.MaintenanceCharges,
            input.WaterCharges,
            input.OtherCharges,
            input.TotalSocietyCharges
            );

        return new PagedResultDto<SocietyChargeDto>(
            totalCount,
            ObjectMapper.Map<List<SocietyCharge>, List<SocietyChargeDto>>(societyCharges)
        );
    }

    public async Task UpdateAsync(Guid id, UpdateSocietyChargeDto input)
    {
        var societyCharge = await _societyChargeRepository.GetAsync(id);

        await _societyChargeManager.UpdateAsync(
            societyCharge,
            input.SecurityCharges,
            input.MaintenanceCharges,
            input.WaterCharges,
            input.OtherCharges,
            input.TotalSocietyCharges
            );

        await _societyChargeRepository.UpdateAsync(societyCharge);
    }

}
