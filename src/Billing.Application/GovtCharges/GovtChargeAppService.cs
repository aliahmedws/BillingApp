using Billing.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.GovtCharges;

[RemoteService(isEnabled: false)]
[Authorize(BillingPermissions.GovtCharges.Default)]
public class GovtChargeAppService : BillingAppService, IGovtChargeAppService
{
    private readonly IGovtChargeRepository _govtChargeRepository;
    private readonly GovtChargeManager _govtChargeManager;
    public GovtChargeAppService(IGovtChargeRepository govtChargeRepository, GovtChargeManager govtChargeManager)
    {
        _govtChargeRepository = govtChargeRepository;
        _govtChargeManager = govtChargeManager;
    }

    public async Task<GovtChargeDto> GetAsync(Guid id)
    {
        var govtCharge = await _govtChargeRepository.GetAsync(id);
        return ObjectMapper.Map<GovtCharge, GovtChargeDto>(govtCharge);
    }

    public async Task<PagedResultDto<GovtChargeDto>> GetListAsync()
    {
        var govtCharges = await _govtChargeRepository.GetListAsync();
        return new PagedResultDto<GovtChargeDto>(
            govtCharges.Count,
            ObjectMapper.Map<List<GovtCharge>, List<GovtChargeDto>>(govtCharges)
        );
    }



    [Authorize(BillingPermissions.GovtCharges.Edit)]
    public async Task UpdateAsync(Guid id, UpdateGovtChargeDto input)
    {
        var govtCharge = await _govtChargeRepository.GetAsync(id);

        var totalTaxes =
            (input.Ed ?? 0) +
            (input.TvFee ?? 0) +
            (input.GST ?? 0) +
            (input.IncomeTax ?? 0) +
            (input.ExtraTax ?? 0) +
            (input.FurtherTax ?? 0) +
            (input.NjSurcharge ?? 0) +
            (input.SalesTax ?? 0) +
            (input.FcSurcharge ?? 0) +
            (input.TrSurcharge ?? 0) +
            (input.TaxOnFpa ?? 0);

        await _govtChargeManager.UpdateAsync(
            govtCharge,
            input.Ed,
            input.TvFee,
            input.GST,
            input.IncomeTax,
            input.ExtraTax,
            input.FurtherTax,
            input.NjSurcharge,
            input.SalesTax,
            input.FcSurcharge,
            input.TrSurcharge,
            input.TaxOnFpa,
            totalTaxes
            );
        await _govtChargeRepository.UpdateAsync(govtCharge);
    }
}
