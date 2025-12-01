using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Billing.MaintenanceBills;

public class MaintenanceBillManager : DomainService
{
    private readonly IMaintenanceBillRepository _maintenanceBillRepository;

    public MaintenanceBillManager(
        IMaintenanceBillRepository maintenanceBillRepository,
        ICurrentTenant currentTenant)
    {
        _maintenanceBillRepository = maintenanceBillRepository;
    }

    private void ValidateAmounts(
       decimal waterCharges,
       decimal securityCharges,
       decimal currentBill,
       decimal arrears,
       decimal otherCharges,
       decimal refundOrBenefit,
       decimal anyOtherWorkCharges,
       decimal latePaymentSurcharge)
    {
        if (waterCharges < 0 ||
            securityCharges < 0 ||
            currentBill < 0 ||
            arrears < 0 ||
            otherCharges < 0 ||
            refundOrBenefit < 0 ||
            anyOtherWorkCharges < 0 ||
            latePaymentSurcharge < 0)
        {
            throw new NegativeAmountNotAllowedException();
        }
    }

    private (decimal paymentBeforeDueDate, decimal payableAfterDueDate) CalculateTotals(
        decimal waterCharges,
        decimal securityCharges,
        decimal currentBill,
        decimal arrears,
        decimal otherCharges,
        decimal refundOrBenefit,
        decimal anyOtherWorkCharges,
        decimal latePaymentSurcharge)
    {
        ValidateAmounts(
            waterCharges,
            securityCharges,
            currentBill,
            arrears,
            otherCharges,
            refundOrBenefit,
            anyOtherWorkCharges,
            latePaymentSurcharge);

        var baseAmount =
            waterCharges +
            securityCharges +
            currentBill +
            arrears +
            otherCharges +
            anyOtherWorkCharges;

        var paymentBeforeDueDate = baseAmount - refundOrBenefit;
        if (paymentBeforeDueDate < 0)
        {
            throw new MaintenanceBillTotalBeforeDueDateCannotBeNegativeException(paymentBeforeDueDate);
        }

        var payableAfterDueDate = paymentBeforeDueDate + latePaymentSurcharge;
        return (paymentBeforeDueDate, payableAfterDueDate);
    }

    public async Task<MaintenanceBill> CreateAsync(
        Guid consumerId,
        Guid plotInfoId,
        DateTime billingMonth,
        DateTime issueDate,
        DateTime dueDate,
        decimal waterCharges,
        decimal securityCharges,
        decimal currentBill,
        decimal arrears,
        decimal otherCharges,
        decimal refundOrBenefit,
        decimal anyOtherWorkCharges,
        decimal latePaymentSurcharge)
    {
        Check.NotNull(consumerId, nameof(consumerId));
        Check.NotNull(plotInfoId, nameof(plotInfoId));

        if (dueDate < issueDate)
        {
            throw new MaintenanceBillExpireDateMustBeAfterIssueDateException(issueDate, dueDate);
        }

        var existingBill = await _maintenanceBillRepository
            .FindByPlotAndBillingMonthAsync(plotInfoId, billingMonth);

        if (existingBill != null)
        {
            throw new MaintenanceBillAlreadyExistsException(plotInfoId, billingMonth);
        }

        var (paymentBeforeDueDate, payableAfterDueDate) = CalculateTotals(
            waterCharges,
            securityCharges,
            currentBill,
            arrears,
            otherCharges,
            refundOrBenefit,
            anyOtherWorkCharges,
            latePaymentSurcharge);

        return new MaintenanceBill(
            GuidGenerator.Create(),
            consumerId,
            plotInfoId,
            billingMonth,
            issueDate,
            dueDate,
            waterCharges,
            securityCharges,
            currentBill,
            arrears,
            otherCharges,
            refundOrBenefit,
            anyOtherWorkCharges,
            paymentBeforeDueDate,
            latePaymentSurcharge,
            payableAfterDueDate);
    }

    public async Task UpdateAsync(
        MaintenanceBill bill,
        DateTime billingMonth,
        DateTime issueDate,
        DateTime dueDate,
        decimal waterCharges,
        decimal securityCharges,
        decimal currentBill,
        decimal arrears,
        decimal otherCharges,
        decimal refundOrBenefit,
        decimal anyOtherWorkCharges,
        decimal latePaymentSurcharge)
    {
        Check.NotNull(bill, nameof(bill));

        if (dueDate < issueDate)
        {
            throw new MaintenanceBillExpireDateMustBeAfterIssueDateException(issueDate, dueDate);
        }

        var existingBill = await _maintenanceBillRepository
            .FindByPlotAndBillingMonthAsync(bill.PlotInfoId, billingMonth);

        if (existingBill != null && existingBill.Id != bill.Id)
        {
            throw new MaintenanceBillAlreadyExistsException(bill.PlotInfoId, billingMonth);
        }

        var (paymentBeforeDueDate, payableAfterDueDate) = CalculateTotals(
            waterCharges,
            securityCharges,
            currentBill,
            arrears,
            otherCharges,
            refundOrBenefit,
            anyOtherWorkCharges,
            latePaymentSurcharge);

        bill
        .ChangeDates(billingMonth, issueDate, dueDate)
        .ChangeCharges(
            waterCharges,
            securityCharges,
            currentBill,
            arrears,
            otherCharges,
            refundOrBenefit,
            anyOtherWorkCharges,
            paymentBeforeDueDate,
            latePaymentSurcharge,
            payableAfterDueDate);
    }

    public async Task MarkPaidAsync(MaintenanceBill bill)
    {
        Check.NotNull(bill, nameof(bill));
        bill.MarkAsPaid();
        await Task.CompletedTask;
    }

    public async Task MarkCancelledAsync(MaintenanceBill bill)
    {
        Check.NotNull(bill, nameof(bill));
        bill.MarkAsCancelled();
        await Task.CompletedTask;
    }
}
