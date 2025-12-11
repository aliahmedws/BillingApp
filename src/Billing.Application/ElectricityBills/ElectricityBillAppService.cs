using Billing.Localization;
using Billing.MaintenanceBills;
using Billing.MeterInfos;
using Billing.Permissions;
using Billing.TarrifSlabs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.ElectricityBills;


[RemoteService(IsEnabled = false)]
[Authorize(BillingPermissions.ElectricityBills.Default)]
public class ElectricityBillAppService : BillingAppService, IElectricityBillAppService
{
    private readonly IElectricityBillRepository _billRepository;
    private readonly ElectricityBillManager _billManager;
    private readonly IMeterInfoRepository _meterInfoRepository;
    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    private readonly IStringLocalizer<BillingResource> _localizer;

    public ElectricityBillAppService(
        IElectricityBillRepository billRepository,
        ElectricityBillManager billManager,
        IMeterInfoRepository meterInfoRepository,
        ITarrifSlabRepository tarrifSlabRepository,
        IStringLocalizer<BillingResource> localizer
        )
    {
        _billRepository = billRepository;
        _billManager = billManager;
        _meterInfoRepository = meterInfoRepository;
        _tarrifSlabRepository = tarrifSlabRepository;
        _localizer = localizer;
    }

    public async Task<ElectricityBillDto> GetAsync(Guid id)
    {
        var bill = await _billRepository.GetAsync(id);
        return ObjectMapper.Map<ElectricityBill, ElectricityBillDto>(bill);
    }

    public async Task<PagedResultDto<ElectricityBillDto>> GetListAsync(GetElectricityBillListDto input)
    {
        //if (input.Sorting.IsNullOrWhiteSpace())
        //    input.Sorting = nameof(ElectricityBill.BillingMonth) + " DESC";

        var bills = await _billRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting!,
            input.Filter,
            input.MeterInfoId,
            input.PreviousReading,
            input.PresentReading,
            input.MeterReadingDate,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.CurrentMonthBill,
            input.BillAdjustment,
            input.AnyOtherCharges,
            input.LPSurcharge,
            input.Status
        );

        var totalCount = await _billRepository.GetCountAsync(
            input.Filter,
            input.MeterInfoId,
            input.PreviousReading,
            input.PresentReading,
            input.MeterReadingDate,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.CurrentMonthBill,
            input.BillAdjustment,
            input.AnyOtherCharges,
            input.LPSurcharge,
            input.Status
        );

        return new PagedResultDto<ElectricityBillDto>(
            totalCount,
            ObjectMapper.Map<List<ElectricityBill>, List<ElectricityBillDto>>(bills)
        );
    }

    [Authorize(BillingPermissions.ElectricityBills.Create)]
    public async Task<ElectricityBillDto> CreateAsync(CreateElectricityBillDto input)
    {
        var bill = await _billManager.CreateAsync(
            input.MeterInfoId,
            input.PreviousReading,
            input.PresentReading,
            input.MeterReadingDate,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.CurrentMonthBill,
            input.BillAdjustment,
            input.AnyOtherCharges,
            input.LPSurcharge,
            input.Status,
            input.Arrears
        );

        await _billRepository.InsertAsync(bill);

        return ObjectMapper.Map<ElectricityBill, ElectricityBillDto>(bill);
    }

    [Authorize(BillingPermissions.ElectricityBills.Edit)]
    public async Task UpdateAsync(Guid id, UpdateElectricityBillDto input)
    {
        var bill = await _billRepository.GetAsync(id);

        await _billManager.UpdateAsync(
            bill,
            input.MeterInfoId,
            input.PreviousReading,
            input.PresentReading,
            input.MeterReadingDate,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.CurrentMonthBill,
            input.BillAdjustment,
            input.AnyOtherCharges,
            input.LPSurcharge,
            input.Status,
            input.Arrears
        );

        await _billRepository.UpdateAsync(bill);
    }

    [Authorize(BillingPermissions.ElectricityBills.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _billRepository.DeleteAsync(id);
    }

    public async Task<decimal> CalculateBillAsync(int units)
    {
        var slabs = (await _tarrifSlabRepository.GetListAsync())
                    .OrderBy(s => s.LowerSlab)
                    .ToList();

        foreach (var slab in slabs)
        {
            if (units >= slab.LowerSlab && units <= slab.UpperSlab)
            {
                return units * slab.UnitPrice;
            }
        }

        return 0;
    }

    public async Task GenerateBulkAsync(BulkElectricityBillRequestDto input)
    {
        foreach(var item in input.Items)
        {
            var units = item.PresentReading - item.PreviousReading;

            if (units < 0)
                throw new UserFriendlyException(_localizer["Presentreadingcannotbelessthanpreviousreading."]);

            var currentMonthBill = await CalculateBillAsync((int)units);

            var bill = await _billManager.CreateAsync(
                item.MeterInfoId,
                item.PreviousReading,
                item.PresentReading,
                input.MeterReadingDate,
                input.BillingMonth,
                input.IssueDate,
                input.DueDate,
                currentMonthBill,
                billAdjustment: 0,
                input.AnyOtherCharges,
                input.LpSurcharge,
                BillStatus.Unpaid,
                item.Arrears
            );

            await _billRepository.InsertAsync(bill);
        }
    }
}
