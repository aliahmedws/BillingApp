using Billing.BillTemplates;
using Billing.ElectricityBills;
using Billing.ElectricityPaymentHistories;
using Billing.GovtCharges;
using Billing.IescoCharges;
using Billing.Localization;
using Billing.TarrifSlabs;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;
using Volo.Abp.Domain.Repositories;

namespace Billing.ElectricityBillTemplates;

[RemoteService(isEnabled: false)]
public class ElectricityBillTemplateAppService : BillingAppService, IElectricityBillTemplateAppService
{
    private readonly IElectricityBillRepository _billRepository;
    private readonly IElectricityPaymentHistoryRepository _paymentRepository;
    private readonly IStringLocalizer<BillingResource> _localizer;
    private readonly ITarrifSlabRepository _tarrifSlabRepository;
    private readonly IGovtChargeRepository _govtChargeRepository;
    private readonly IIescoChargeRepository _iescoChargeRepository;

    public ElectricityBillTemplateAppService(
        IElectricityBillRepository billRepository,
        IElectricityPaymentHistoryRepository paymentRepository,
        IStringLocalizer<BillingResource> localizer,
        ITarrifSlabRepository tarrifSlabRepository,
        IGovtChargeRepository govtChargeRepository,
        IIescoChargeRepository iescoChargeRepository)
    {
        _billRepository = billRepository;
        _paymentRepository = paymentRepository;
        _tarrifSlabRepository = tarrifSlabRepository;
        _govtChargeRepository = govtChargeRepository;
        _iescoChargeRepository = iescoChargeRepository;
        _localizer = localizer;
    }

    public async Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid billId)
    {
        var bill = await _billRepository.GetByIdAsync(billId);
        if (bill == null)
            throw new UserFriendlyException("Electricity bill not found");

        var wrapper = PrintTemplateLoader.LoadFromFolder("ElectricityBillTemplates", "BillWrapper.html");
        var fragment = PrintTemplateLoader.LoadFromFolder("ElectricityBillTemplates", "BillFragment.html");

        var billHtml = await GenerateSingleBillHtmlAsync(bill, fragment);
        var finalHtml = wrapper.Replace("{{ALL_BILLS}}", billHtml);

        var ownerName = $"{bill.MeterInfos?.MeterOwner?.FirstName + ' ' + bill?.MeterInfos?.MeterOwner?.LastName}".Trim();

        if (string.IsNullOrWhiteSpace(ownerName))
            ownerName = "Unknown";

        var bytes = Encoding.UTF8.GetBytes(finalHtml);
        return new RemoteStreamContent(new MemoryStream(bytes), $"ElectricityBill-{ownerName}.html", "text/html");
    }

    public async Task<IRemoteStreamContent> PrintFilteredAsync(GetElectricityBillListDto input)
    {
        var bills = await _billRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting ?? nameof(ElectricityBill.CreationTime),
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

        if (bills.Count == 0)
            throw new UserFriendlyException("No electricity bills found for selected filters.");

        var wrapper = PrintTemplateLoader.LoadFromFolder("ElectricityBillTemplates", "BillWrapper.html");
        var fragment = PrintTemplateLoader.LoadFromFolder("ElectricityBillTemplates", "BillFragment.html");

        var allBills = new StringBuilder();

        foreach (var bill in bills)
        {
            allBills.Append(await GenerateSingleBillHtmlAsync(bill, fragment));
        }

        var finalHtml = wrapper.Replace("{{ALL_BILLS}}", allBills.ToString());

        var bytes = Encoding.UTF8.GetBytes(finalHtml);
        return new RemoteStreamContent(new MemoryStream(bytes), "FilteredElectricityBills.html", "text/html");
    }

    private async Task<string> GenerateSingleBillHtmlAsync(ElectricityBill bill, string fragment)
    {
        var owner = bill.MeterInfos?.MeterOwner;
        var ownerName = $"{owner?.FirstName} {owner?.LastName}".Trim();

        var phone = owner?.Phone ?? "";
        var street = owner?.Address?.Street ?? "";

        // Payment history (FIX: don't format DueDate as N0)
        var lastBills = await _billRepository.GetLastTenBillsAsync(bill.MeterInfos!.Id, bill.MeterInfos.MeterOwnerId, 10);

        var historyBills = lastBills
            .Where(x => x.Id != bill.Id)
            .Where(x => x.BillingMonth.Year != bill.BillingMonth.Year
                    || x.BillingMonth.Month != bill.BillingMonth.Month)
            .OrderByDescending(x => x.BillingMonth)
            .Take(10)
            .OrderBy(x => x.BillingMonth)
            .ToList();

        string paymentHistoryRows;
        if (historyBills.Count == 0)
        {
            paymentHistoryRows = "<tr><td colspan='5' class='center'>No Payment History</td></tr>";
        }
        else
        {
            var sb = new StringBuilder();

            foreach (var b in historyBills)
            {
                var pays = await _paymentRepository.GetListByBillIdAsync(b.Id);

                var paidAmt = pays.Sum(x => x.PaymentReceived);
                var lastPay = pays.OrderByDescending(x => x.PaymentDate).FirstOrDefault();

                sb.AppendLine($@"
                <tr>
                  <td>{b.BillingMonth:MMM-yyyy}</td>
                  <td class='right'>{b.PayableDueDateAmount:N0}</td>
                  <td class='right'>{(paidAmt == 0 ? "" : paidAmt.ToString("N0"))}</td>
                  <td class='center'>{(lastPay == null ? "" : lastPay.PaymentDate.ToString("yyyy-MM-dd"))}</td>
                  <td class='center'>{(lastPay == null ? "" : lastPay.TransactionId)}</td>
                </tr>");
            }

            paymentHistoryRows = sb.ToString();
        }


        // Tariff rows from TarrifSlab table (NO relation needed)
        var slabs = await _tarrifSlabRepository.GetListAsync(
            skipCount: 0,
            maxResultCount: 1000,
            sorting: "LowerSlab asc",
            filter: null,
            lowerSlab: null,
            upperSlab: null,
            unitPrice: null
        );

        var iescoTariffRows = BuildTariffRateOnlyRows(slabs);

        // Buyer code safer
        var buyerCode = $"{bill.MeterInfos?.Plot?.PlotNo} {bill.MeterInfos?.Phase?.PhaseName} {bill.MeterInfos?.Block?.BlockName}".Trim();

        var governmentCharges = await _govtChargeRepository.FirstOrDefaultAsync();
        var iescoCharges = await _iescoChargeRepository.FirstOrDefaultAsync();

        var totalEnergy = iescoCharges?.TotalEnergyCharges;
        var totalTaxes = governmentCharges?.TotalTaxes;

        var energyAndTaxes = totalEnergy + totalTaxes + iescoCharges?.IescoFixCharges
                + iescoCharges?.ServiceRent + iescoCharges?.VarFpa + iescoCharges?.QtrTariffAdj;

        //var payableDueDate = bill.PayableDueDateAmount + energyAndTaxes;
        var payableAfterDueDate = bill.PayableAfterDueDateAmount + energyAndTaxes;

        return fragment
            .Replace("{{HeaderTitle}}", _localizer["Electricity:HeaderTitle"])
            .Replace("{{HeaderSubTitle}}", _localizer["Electricity:HeaderSubTitle"])
            .Replace("{{ConsumerNoLabel}}", _localizer["Electricity:ConsumerNoLabel"])
            .Replace("{{PayThroughLine}}", _localizer["Electricity:PayThroughLine"])
            .Replace("{{ComplaintLine}}", _localizer["Electricity:ComplaintLine"])
            
            .Replace("{{ConsumerId}}", bill.Id.ToString())
            //.Replace("{{ConsumerNo}}", phone)
            .Replace("{{OwnerName}}", ownerName)
            .Replace("{{BuyerCode}}", buyerCode)
            .Replace("{{StreetNo}}", street)
            .Replace("{{BillingMonth}}", bill.BillingMonth.ToString("MMM-yyyy"))
            .Replace("{{MeterReadingDate}}", bill.MeterReadingDate.ToString("dd.MM.yyyy"))
            .Replace("{{MeterNo}}", bill.MeterInfos?.MeterNo ?? "")
            .Replace("{{PreviousReading}}", bill.PreviousReading.ToString("N0"))
            .Replace("{{PresentReading}}", bill.PresentReading.ToString("N0"))
            .Replace("{{ConsumedUnits}}", bill.ConsumedUnits.ToString("N0"))
            .Replace("{{IssueDate}}", bill.IssueDate.ToString("dd.MM.yyyy"))
            .Replace("{{DueDate}}", bill.DueDate.ToString("dd.MM.yyyy"))

            // IESCO section (FIX: TotalEnergyCharges now computed)
            .Replace("{{IescoTariffRows}}", iescoTariffRows)
            .Replace("{{TotalEnergyCharges}}", (iescoCharges?.TotalEnergyCharges ?? 0m).ToString("N0"))
            .Replace("{{IescoFixCharges}}", (iescoCharges?.IescoFixCharges ?? 0m).ToString("N0"))
            .Replace("{{IescoFurtherTax}}", "0")
            .Replace("{{ServiceRent}}", (iescoCharges?.ServiceRent ?? 0m).ToString("N0"))
            .Replace("{{VarFpa}}", (iescoCharges?.VarFpa ?? 0m).ToString("N0"))
            .Replace("{{QtrTariffAdj}}", (iescoCharges?.QtrTariffAdj ?? 0m).ToString("N0"))
            .Replace("{{TotalEnergyCharges2}}", (bill?.TotalIESCOCharges ?? 0m).ToString("N0"))


           .Replace("{{GovEd}}", (governmentCharges?.Ed ?? 0m).ToString("N2"))
           .Replace("{{GovTvFee}}", (governmentCharges?.TvFee ?? 0m).ToString("N0"))
           .Replace("{{GovGst}}", (governmentCharges?.GST ?? 0m).ToString("N0"))
           .Replace("{{GovIncomeTax}}", (governmentCharges?.IncomeTax ?? 0m).ToString("N0"))
           .Replace("{{GovExtraTax}}", (governmentCharges?.ExtraTax ?? 0m).ToString("N0"))
           .Replace("{{GovFurtherTax}}", (governmentCharges?.FurtherTax ?? 0m).ToString("N0"))
           .Replace("{{GovNjSrch}}", (governmentCharges?.NjSurcharge ?? 0m).ToString("N0"))
           .Replace("{{GovSalesTax}}", (governmentCharges?.SalesTax ?? 0m).ToString("N0"))
           .Replace("{{GovFcSrch}}", (governmentCharges?.FcSurcharge ?? 0m).ToString("N0"))
           .Replace("{{GovTrSrch}}", (governmentCharges?.TrSurcharge ?? 0m).ToString("N0"))
           .Replace("{{GovTaxOnFpa}}", (governmentCharges?.TaxOnFpa ?? 0m).ToString("N0"))
           .Replace("{{GovTotalTaxes}}", (bill?.TotalGovernmentCharges ?? 0m).ToString("N0"))


            // Society payable (FIX property names)
            .Replace("{{SocietyMsCharges}}", "0")
            .Replace("{{SocietyWaterCharges}}", "0")
            .Replace("{{MaintenanceCharges}}", "0")
            .Replace("{{OtherCharges}}", "0")
            .Replace("{{TotalSocietyCharges}}", bill?.TotalSocietyCharges.ToString("N0"))
            .Replace("{{SocietyNonConFine}}", "0")
            .Replace("{{SocietyArrears}}", bill?.Arrears.ToString("N0"))
            .Replace("{{SocietyCurrentMonthBill}}", bill?.CurrentMonthBill.ToString("N0"))
            .Replace("{{EnergyAndTaxes}}", energyAndTaxes?.ToString("N0"))
            .Replace("{{SocietyBillAdjustment}}", bill?.BillAdjustment.ToString("N0"))
            .Replace("{{SocietyAnyOtherCharges}}", bill?.AnyOtherCharges.ToString("N0"))
            .Replace("{{PayableDueDate}}", bill?.PayableDueDateAmount.ToString("N0"))
            .Replace("{{LatePaymentSurcharge}}", bill?.LPSurcharge.ToString("N0"))
            .Replace("{{PayableAfterDueDate}}", bill?.PayableAfterDueDateAmount.ToString("N0"))

            .Replace("{{PaymentHistoryRows}}", paymentHistoryRows)
            .Replace("{{NoticeLine}}", _localizer["Electricity:NoticeLine"])
            .Replace("{{BankUseTitle}}", _localizer["Electricity:BankUseTitle"]);
    }

    private static string BuildTariffRateOnlyRows(List<TarrifSlab> slabs)
    {
        if (slabs == null || slabs.Count == 0)
            return "<tr><td colspan='4' class='center'>No Tariff Slabs Configured</td></tr>";

        return string.Join("", slabs
            .OrderBy(x => x.LowerSlab)
            .ThenBy(x => x.UpperSlab)
            .Select(s => $@"
            <tr>
              <td>{FormatSlabLabel(s)}</td>
              <td class='right'>{s.UnitPrice:N2}</td>
              <td class='right'></td>
              <td class='right'></td>
            </tr>"));
    }

    private static string FormatSlabLabel(TarrifSlab s)
    {
        var lower = (int)Math.Floor(s.LowerSlab);
        var upper = s.UpperSlab.HasValue ? (int)Math.Floor(s.UpperSlab.Value) : (int?)null;

        return upper.HasValue
            ? $"{lower}-{upper.Value} Units"
            : $"{lower}- above Units";
    }

}
