using Billing.ElectricityPaymentHistories;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.Localization;
using Billing.MaintenanceBills;
using Billing.MeterInfos;
using Billing.Permissions;
using Billing.SocietyCharges;
using Billing.TarrifSlabs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

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
    private readonly IElectricityPaymentHistoryRepository _electricityPaymentHistoryRepository;
    private readonly IGovtChargeRepository _govtChargeRepository;
    private readonly ISocietyChargeRepository _societyChargeRepository;
    private readonly IIescoChargeRepository _iescoChargeRepository;

    public ElectricityBillAppService(
        IElectricityBillRepository billRepository,
        ElectricityBillManager billManager,
        IMeterInfoRepository meterInfoRepository,
        ITarrifSlabRepository tarrifSlabRepository,
        IStringLocalizer<BillingResource> localizer,
        IElectricityPaymentHistoryRepository electricityPaymentHistoryRepository,
        IGovtChargeRepository govtChargeRepository,
        ISocietyChargeRepository societyChargeRepository,
        IIescoChargeRepository iescoChargeRepository
      
        )
    {
        _electricityPaymentHistoryRepository = electricityPaymentHistoryRepository;
        _billRepository = billRepository;
        _billManager = billManager;
        _meterInfoRepository = meterInfoRepository;
        _tarrifSlabRepository = tarrifSlabRepository;
        _govtChargeRepository = govtChargeRepository;
        _societyChargeRepository = societyChargeRepository;
        _iescoChargeRepository = iescoChargeRepository;
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
            input.Arrears,
            input.TotalGovernmentCharges,
            input.TotalIESCOCharges,
            input.TotalSocietyCharges
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
            input.Arrears,
            input.TotalGovernmentCharges,
            input.TotalIESCOCharges,
            input.TotalSocietyCharges
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
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (input.Items == null || input.Items.Count == 0) return;

        var totalGovernmentCharges = await _govtChargeRepository.GetTotalCharges() ?? 0m;
        var totalIescoCharges = await _iescoChargeRepository.GetTotalIescoCharges() ?? 0m;

        //Collect meter ids
        var meterIds = input.Items
            .Select(x => x.MeterInfoId)
            .Where(x => x != Guid.Empty)
            .Distinct()
            .ToList();

        //meterId -> plotSizeName
        var meterPlotSizeMap = await _meterInfoRepository.GetPlotSizeNameMapByMeterIdsAsync(meterIds);

        //plotSizeName -> societyCharges (cached)
        var plotSizeNames = meterPlotSizeMap.Values
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var societyChargeMap = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        foreach (var plotSizeName in plotSizeNames)
        {
            // Assumption: returns decimal? (adjust if your repo returns decimal)
            var societyCharge = await _societyChargeRepository.GetTotalChargesByPlotSizeName(plotSizeName) ?? 0m;
            societyChargeMap[plotSizeName] = societyCharge;
        }

        //Create bills
        var bills = new List<ElectricityBill>(input.Items.Count);

        foreach (var item in input.Items)
        {
            var units = item.PresentReading - item.PreviousReading;
            if (units < 0)
                throw new UserFriendlyException(_localizer["Presentreadingcannotbelessthanpreviousreading."]);

            var currentMonthBill = await CalculateBillAsync((int)units);

            meterPlotSizeMap.TryGetValue(item.MeterInfoId, out var plotSizeName);

            decimal totalSocietyCharges = 0m;
            if (!plotSizeName.IsNullOrWhiteSpace() &&
                societyChargeMap.TryGetValue(plotSizeName!.Trim(), out var sc))
            {
                totalSocietyCharges = sc;
            }

            var bill = await _billManager.CreateAsync(
                meterInfoId: item.MeterInfoId,
                previousReading: item.PreviousReading,
                presentReading: item.PresentReading,
                meterReadingDate: input.MeterReadingDate,
                billingMonth: input.BillingMonth,
                issueDate: input.IssueDate,
                dueDate: input.DueDate,
                currentMonthBill: currentMonthBill,
                billAdjustment: 0m,
                anyOtherCharges: input.AnyOtherCharges,
                lpSurcharge: 0m,
                status: BillStatus.Unpaid,
                arrears: item.Arrears,
                totalGovernmentCharges: totalGovernmentCharges,
                totalIESCOCharges: totalIescoCharges,
                totalSocietyCharges: totalSocietyCharges
            );

            bills.Add(bill);
        }

        await _billRepository.InsertManyAsync(bills, autoSave: true);
    }

    public async Task<IRemoteStreamContent> GetListAsExcelFileAsync(GetElectricityBillListDto input)
    {
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

        using var workbook = new ClosedXML.Excel.XLWorkbook();
        var sheet = workbook.AddWorksheet("Electricity Bills");

        sheet.Cell(1, 1).Value = _localizer["Meter"].Value;
        sheet.Cell(1, 2).Value = _localizer["PreviousReading"].Value;
        sheet.Cell(1, 3).Value = _localizer["PresentReading"].Value;
        sheet.Cell(1, 4).Value = _localizer["Units"].Value;
        sheet.Cell(1, 5).Value = _localizer["BillingMonth"].Value;
        sheet.Cell(1, 6).Value = _localizer["MeterReadingDate"].Value;
        sheet.Cell(1, 7).Value = _localizer["IssueDate"].Value;
        sheet.Cell(1, 8).Value = _localizer["DueDate"].Value;
        sheet.Cell(1, 9).Value = _localizer["CurrentMonthBill"].Value;
        sheet.Cell(1, 10).Value = _localizer["Arrears"].Value;
        sheet.Cell(1, 11).Value = _localizer["AnyOtherCharges"].Value;
        sheet.Cell(1, 12).Value = _localizer["LPSurcharge"].Value;
        sheet.Cell(1, 13).Value = _localizer["Status"].Value;

        // Payment History Columns
        sheet.Cell(1, 14).Value = _localizer["PaymentTransactionId"].Value;
        sheet.Cell(1, 15).Value = _localizer["PaymentReceived"].Value;
        sheet.Cell(1, 16).Value = _localizer["PaymentDate"].Value;
        sheet.Cell(1, 17).Value = _localizer["PaymentMethod"].Value;

        sheet.Range("A1:Q1").Style.Font.Bold = true;

        int row = 2;

        foreach (var bill in bills)
        {
            var meter = await _meterInfoRepository.FindAsync(bill.MeterInfoId);

            var latestPayment = await _electricityPaymentHistoryRepository.GetLatestPaymentHistoryByBillIdAsync(bill.Id);

            sheet.Cell(row, 1).Value = meter?.MeterNo ?? "";
            sheet.Cell(row, 2).Value = bill.PreviousReading;
            sheet.Cell(row, 3).Value = bill.PresentReading;
            sheet.Cell(row, 4).Value = bill.ConsumedUnits;
            sheet.Cell(row, 5).Value = bill.BillingMonth.ToString("yyyy-MM");
            sheet.Cell(row, 6).Value = bill.MeterReadingDate.ToShortDateString();
            sheet.Cell(row, 7).Value = bill.IssueDate.ToShortDateString();
            sheet.Cell(row, 8).Value = bill.DueDate.ToShortDateString();
            sheet.Cell(row, 9).Value = bill.CurrentMonthBill;
            sheet.Cell(row, 10).Value = bill.Arrears;
            sheet.Cell(row, 11).Value = bill.AnyOtherCharges;
            sheet.Cell(row, 12).Value = bill.LPSurcharge;
            sheet.Cell(row, 13).Value = bill.Status.ToString();

            sheet.Cell(row, 14).Value = latestPayment?.TransactionId ?? "";
            sheet.Cell(row, 15).Value = latestPayment?.PaymentReceived ?? 0m;
            sheet.Cell(row, 16).Value = latestPayment?.PaymentDate.ToShortDateString() ?? "";
            sheet.Cell(row, 17).Value = latestPayment?.Method.ToString() ?? "";

            row++;
        }

        sheet.Columns().AdjustToContents();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Seek(0, SeekOrigin.Begin);

        return new RemoteStreamContent(
            stream,
            $"ElectricityBills_{DateTime.Now:yyyyMMddHHmm}.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        );
    }

}
