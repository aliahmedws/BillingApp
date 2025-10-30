using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.IescoCharges;


[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.IescoCharges.Default)]
public class IescoChargeAppService :  BillingAppService, IIescoChargeAppService
{
    private readonly IescoChargeManager _iescoChargeManager;
    private readonly IIescoChargeRepository _iescoChargeRepository;
    public IescoChargeAppService(
        IescoChargeManager iescoChargeManager,
        IIescoChargeRepository iescoChargeRepository)
    {
        _iescoChargeManager = iescoChargeManager;
        _iescoChargeRepository = iescoChargeRepository;
    }

    public async Task<IescoChargeDto> GetAsync(Guid id)
    {
        var iescoCharge = await _iescoChargeRepository.GetAsync(id);
        return ObjectMapper.Map<IescoCharge, IescoChargeDto>(iescoCharge);
    }

    public async Task<PagedResultDto<IescoChargeDto>> GetListAsync()
    {
        var iescoCharges = await _iescoChargeRepository.GetListAsync();
        return new PagedResultDto<IescoChargeDto>(
            iescoCharges.Count,
            ObjectMapper.Map<List<IescoCharge>, List<IescoChargeDto>>(iescoCharges)
        );
    }

    [Authorize(BillingPermissions.IescoCharges.Edit)]
    public async Task UpdateAsync(Guid id, UpdateIescoChargeDto input)
    {
        var iescoCharge = await _iescoChargeRepository.GetAsync(id);

        var totalIescoCharges =
            (input.TotalEnergyCharges ?? 0) +
            (input.IescoFixCharges ?? 0) +
            (input.ServiceRent ?? 0) +
            (input.VarFpa ?? 0) +
            (input.QtrTariffAdj ?? 0);

        await _iescoChargeManager.UpdateAsync(
            iescoCharge,
            input.TotalEnergyCharges,
            input.IescoFixCharges,
            input.ServiceRent,
            input.VarFpa,
            input.QtrTariffAdj,
            totalIescoCharges
        );
        await _iescoChargeRepository.UpdateAsync(iescoCharge);
    }
}
