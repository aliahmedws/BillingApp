using Billing.BillTemplates;
using Billing.Localization;
using Billing.MaintenanceBills;
using Billing.MaintenancePaymentHistories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Content;

namespace Billing.MaintenanceBillTemplates;

[RemoteService(isEnabled: false)]
public class MaintenanceBillTemplateAppService : BillingAppService, IMaintenanceBillTemplateAppService
{

    private readonly IMaintenanceBillRepository _billRepository;
    private readonly IMaintenancePaymentHistoryRepository _paymentRepository;
    private readonly IStringLocalizer<BillingResource> _localizer;

    public MaintenanceBillTemplateAppService(
        IMaintenanceBillRepository billRepository,
        IMaintenancePaymentHistoryRepository paymentRepository,
        IStringLocalizer<BillingResource> localizer)
    {
        _billRepository = billRepository;
        _paymentRepository = paymentRepository;
        _localizer = localizer;
    }

    public async Task<IRemoteStreamContent> GetPrintHtmlAsync(Guid billId)
    {
        var bill = await _billRepository.GetByIdAsync(billId);
        if (bill == null)
            throw new UserFriendlyException("Bill not found");

        string wrapper = PrintTemplateLoader.Load("BillWrapper.html");
        string fragment = PrintTemplateLoader.Load("BillFragment.html");

        string billHtml = await GenerateSingleBillHtmlAsync(bill, fragment);

        string finalHtml = wrapper.Replace("{{ALL_BILLS}}", billHtml);

        var bytes = Encoding.UTF8.GetBytes(finalHtml);
        return new RemoteStreamContent(
            new MemoryStream(bytes),
            $"MaintenanceBill-{bill.ConsumerId}.html",
            "text/html"
        );
    }


    // 2. PRINT MULTIPLE FILTERED BILLS (same fragment + wrapper)
    public async Task<IRemoteStreamContent> PrintFilteredAsync(GetMaintenanceBillListDto input)
    {
        var bills = await _billRepository.GetListAsync(
            input.SkipCount,
            input.MaxResultCount,
            input.Sorting ?? nameof(MaintenanceBill.CreationTime),
            input.Filter,
            input.Status,
            input.BillingMonth,
            input.ConsumerId,
            input.PlotInfoId
        );

        if (bills.Count == 0)
            throw new UserFriendlyException("No bills found for selected filters.");

        string wrapper = PrintTemplateLoader.Load("BillWrapper.html");
        string fragment = PrintTemplateLoader.Load("BillFragment.html");

        StringBuilder allBillsHtml = new();

        foreach (var bill in bills)
        {
            string billHtml = await GenerateSingleBillHtmlAsync(bill, fragment);
            allBillsHtml.Append(billHtml);
        }

        string finalHtml = wrapper.Replace("{{ALL_BILLS}}", allBillsHtml.ToString());

        var bytes = Encoding.UTF8.GetBytes(finalHtml);
        return new RemoteStreamContent(
            new MemoryStream(bytes),
            "FilteredBills.html",
            "text/html"
        );
    }


    // SHARED METHOD — Generates HTML for ONE BILL
    private async Task<string> GenerateSingleBillHtmlAsync(
        MaintenanceBill bill,
        string fragment)
    {
        // Fetch last bills for payment history
        var lastBills = await _billRepository.GetLastTenBillsAsync(
            bill.ConsumerPersonalInfos.Id,
            bill.PlotInfos.Id,
            10
        );

        var combinedPayments = new List<(MaintenanceBill Bill, MaintenancePaymentHistory Pay)>();

        foreach (var b in lastBills)
        {
            var payments = await _paymentRepository.GetListAsync(
                0, 10, nameof(MaintenancePaymentHistory.CreationTime) + " DESC", b.Id, null
            );

            foreach (var p in payments)
                combinedPayments.Add((b, p));
        }

        var ordered = combinedPayments
            .OrderBy(x => x.Bill.BillingMonth)
            .ThenBy(x => x.Pay.PaymentDate)
            .ToList();

        string paymentRows = ordered.Count == 0
            ? "<tr><td colspan='5' style='text-align:center;'>No Payment History</td></tr>"
            : string.Join("", ordered.Select(p => $@"
                <tr>
                    <td>{p.Bill.BillingMonth:yyyy-MM}</td>
                    <td>{p.Bill.CurrentBill:N0}</td>
                    <td>{p.Pay.PaymentReceived:N2}</td>
                    <td>{p.Pay.PaymentDate:dd/MM/yyyy}</td>
                    <td>{p.Pay.TransactionId}</td>
                </tr>"));

        return fragment
            .Replace("{{SocietyName}}", _localizer["SocietyName"])
            .Replace("{{OfficeLine}}", _localizer["OfficeLine"])
            .Replace("{{ConsumerNo}}", _localizer["ConsumerNo"])
            .Replace("{{CustomerCode}}", "Nothing")

            .Replace("{{ConsumerId}}", bill.ConsumerId.ToString())
            .Replace("{{OwnerFullName}}", $"{bill.ConsumerPersonalInfos.FirstName} {bill.ConsumerPersonalInfos.LastName}")
            .Replace("{{PhoneNumber}}", bill.ConsumerPersonalInfos.Phone ?? "")

            .Replace("{{BillingMonthValue}}", bill.BillingMonth.ToString("MMM-yyyy"))
            .Replace("{{IssueDateValue}}", bill.IssueDate.ToShortDateString())
            .Replace("{{DueDateValue}}", bill.DueDate.ToShortDateString())

            .Replace("{{MainMiscValue}}", "Nothing")
            .Replace("{{WaterChargesValue}}", bill.WaterCharges.ToString("N2"))
            .Replace("{{SecurityChargesValue}}", bill.SecurityCharges.ToString("N2"))
            .Replace("{{CurrentBillValue}}", bill.CurrentBill.ToString("N2"))

            .Replace("{{ArrearsValue}}", bill.Arrears.ToString("N2"))
            .Replace("{{OtherChargesValue}}", bill.OtherCharges.ToString("N2"))
            .Replace("{{RefundOrBenefitValue}}", bill.RefundOrBenefit.ToString("N2"))
            .Replace("{{AnyOtherWorkChargesValue}}", bill.AnyOtherWorkCharges.ToString("N2"))
            .Replace("{{PaymentBeforeDueDateValue}}", bill.PaymentBeforeDueDate.ToString("N2"))
            .Replace("{{LatePaymentSurchargeValue}}", bill.LatePaymentSurcharge.ToString("N2"))
            .Replace("{{PayableAfterDueDateValue}}", bill.PayableAfterDueDate.ToString("N2"))

            .Replace("{{PurchaseCode}}",
                $"({bill.PlotInfos.PlotNo}/{bill.PlotInfos.Block.BlockName}/{bill.PlotInfos.Phase.PhaseName})")

            .Replace("{{PaymentRows}}", paymentRows)
            .Replace("{{NoticeLine}}", _localizer["NoticeLine"])
            .Replace("{{SocietyFooter}}", _localizer["SocietyFooter"]);
    }
}
