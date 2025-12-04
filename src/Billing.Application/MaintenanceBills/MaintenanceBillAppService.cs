using Billing.Permissions;
using Billing.PlotInfos;
using Billing.SocietyCharges;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Billing.MaintenanceBills;


[RemoteService(IsEnabled = false)]
[Authorize(BillingPermissions.MaintenanceBills.Default)]
public class MaintenanceBillAppService : BillingAppService, IMaintenanceBillAppService
{
    private readonly IMaintenanceBillRepository _maintenanceBillRepository;
    private readonly MaintenanceBillManager _maintenanceBillManager;
    private readonly IPlotInfoRepository _plotInfoRepository;
    private readonly ISocietyChargeRepository _societyChargeRepository;

    public MaintenanceBillAppService(
        IMaintenanceBillRepository maintenanceBillRepository,
        MaintenanceBillManager maintenanceBillManager,
        IPlotInfoRepository plotInfoRepository,
        ISocietyChargeRepository societyChargeRepository)
    {
        _maintenanceBillRepository = maintenanceBillRepository;
        _maintenanceBillManager = maintenanceBillManager;
        _plotInfoRepository = plotInfoRepository;
        _societyChargeRepository = societyChargeRepository;
    }

    [Authorize(BillingPermissions.MaintenanceBills.Create)]
    public async Task<MaintenanceBillDto> CreateAsync(CreateMaintenanceBillDto input)
    {

        var bill = await _maintenanceBillManager.CreateAsync(
            input.ConsumerId,
            input.PlotInfoId,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.WaterCharges,
            input.SecurityCharges,
            input.CurrentBill,
            input.Arrears,
            input.OtherCharges,
            input.RefundOrBenefit,
            input.AnyOtherWorkCharges,
            input.LatePaymentSurcharge
        );

        await _maintenanceBillRepository.InsertAsync(bill);

        return ObjectMapper.Map<MaintenanceBill, MaintenanceBillDto>(bill);
    }

    [Authorize(BillingPermissions.MaintenanceBills.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _maintenanceBillRepository.DeleteAsync(id);
    }

    public async Task<MaintenanceBillDto> GetAsync(Guid id)
    {
        var bill = await _maintenanceBillRepository.GetAsync(id);
        return ObjectMapper.Map<MaintenanceBill, MaintenanceBillDto>(bill);
    }

    public async Task<PagedResultDto<MaintenanceBillDto>> GetListAsync(GetMaintenanceBillListDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(MaintenanceBill.CreationTime);
        }

        var bills = await _maintenanceBillRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting,
            input.Filter,
            input.Status,
            input.BillingMonth,
            input.ConsumerId,
            input.PlotInfoId
        );

        var totalCount = await _maintenanceBillRepository.GetCountAsync(
            input.Filter,
            input.Status,
            input.BillingMonth,
            input.ConsumerId,
            input.PlotInfoId
        );

        return new PagedResultDto<MaintenanceBillDto>(
            totalCount,
            ObjectMapper.Map<List<MaintenanceBill>, List<MaintenanceBillDto>>(bills)
        );
    }

    [Authorize(BillingPermissions.MaintenanceBills.Edit)]
    public async Task UpdateAsync(Guid id, UpdateMaintenanceBillDto input)
    {
        var bill = await _maintenanceBillRepository.GetAsync(id);

        await _maintenanceBillManager.UpdateAsync(
            bill,
            input.BillingMonth,
            input.IssueDate,
            input.DueDate,
            input.WaterCharges,
            input.SecurityCharges,
            input.CurrentBill,
            input.Arrears,
            input.OtherCharges,
            input.RefundOrBenefit,
            input.AnyOtherWorkCharges,
            input.LatePaymentSurcharge
        );

        await _maintenanceBillRepository.UpdateAsync(bill);
    }

    public async Task<GenerateMaintenanceBillsResultDto> GenerateAsync(GenerateMaintenanceBillsDto input)
    {
        if (input.DueDate < input.IssueDate)
        {
            throw new MaintenanceBillExpireDateMustBeAfterIssueDateException(input.IssueDate, input.DueDate);
        }

        var result = new GenerateMaintenanceBillsResultDto();

        var plots = await _plotInfoRepository.GetBillablePlotsAsync();

        foreach(var plot in plots)
        {
            if (!plot.ConsumerId.HasValue || plot.PlotSize == null)
            {
                continue;
            }

            var existingBill = await _maintenanceBillRepository.FindByPlotAndBillingMonthAsync(plot.Id, input.BillingMonth);

            if (existingBill != null)
            {
                result.SkippedAlreadyExistCount++;
                continue;
            }

            var sizeName = plot.PlotSize.SizeName;
            var societyCharge = await _societyChargeRepository.FindByPlotSizeNameAsync(sizeName);

            if (societyCharge == null)
            {
                result.SkippedNoSocietyChargesCount++;
                continue;
            }

            var waterCharges = societyCharge.WaterCharges ?? 0m;
            var securityCharges = societyCharge.SecurityCharges ?? 0m;
            //var currentBill = societyCharge.MaintenanceCharges ?? 0m;
            var otherCharges = societyCharge.OtherCharges ?? 0m;

            // initial values – can be enhanced later
            var arrears = 0m;
            var refundOrBenefit = 0m;
            var anyOtherWorkCharges = 0m;
            var latePaymentSurcharge = input.LatePaymentSurcharge;
            var currentBill = waterCharges + securityCharges + otherCharges + latePaymentSurcharge;

            var bill = await _maintenanceBillManager.CreateAsync(
               plot.ConsumerId.Value,
               plot.Id,
               input.BillingMonth,
               input.IssueDate,
               input.DueDate,
               waterCharges,
               securityCharges,
               currentBill,
               arrears,
               otherCharges,
               refundOrBenefit,
               anyOtherWorkCharges,
               latePaymentSurcharge
            );

            await _maintenanceBillRepository.InsertAsync(bill);
            result.CreatedCount++;
        }

        return result;
    }
}
