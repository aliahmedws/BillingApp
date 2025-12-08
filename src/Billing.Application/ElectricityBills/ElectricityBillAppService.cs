using Billing.MeterInfos;
using Billing.Permissions;
using Billing.TarrifSlabs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using static System.Net.Mime.MediaTypeNames;

namespace Billing.ElectricityBills;


[RemoteService(IsEnabled = false)]
[Authorize(BillingPermissions.ElectricityBills.Default)]
public class ElectricityBillAppService : BillingAppService, IElectricityBillAppService
{
    private readonly IElectricityBillRepository _billRepository;
    private readonly ElectricityBillManager _billManager;
    private readonly IMeterInfoRepository _meterInfoRepository;
    private readonly ITarrifSlabRepository _tarrifSlabRepository;

    public ElectricityBillAppService(
        IElectricityBillRepository billRepository,
        ElectricityBillManager billManager,
        IMeterInfoRepository meterInfoRepository,
        ITarrifSlabRepository tarrifSlabRepository)
    {
        _billRepository = billRepository;
        _billManager = billManager;
        _meterInfoRepository = meterInfoRepository;
        _tarrifSlabRepository = tarrifSlabRepository;
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
            input.LPSurcharge
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
            input.LPSurcharge
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
            input.LPSurcharge
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
            input.LPSurcharge
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

        foreach(var slab in slabs)
        {
            if (units >= slab.LowerSlab && units <= slab.UpperSlab)
            {
                return units * slab.UnitPrice;
            }
        }

        return 0;
    }
}
