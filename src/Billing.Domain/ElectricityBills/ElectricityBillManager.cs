using Billing.MaintenanceBills;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ElectricityBills;

public class ElectricityBillManager : DomainService
{
    private readonly IElectricityBillRepository _billRepository;

    public ElectricityBillManager(IElectricityBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    private void ValidateReadings(decimal previousReading, decimal presentReading)
    {
        if (previousReading < 0 || presentReading < 0)
            throw new NegativeAmountNotAllowedException();

        if (presentReading < previousReading)
            throw new PresentAndPreviousReadingException(previousReading, presentReading);
    }

    private void ValidateDates(DateTime issueDate, DateTime dueDate)
    {
        if (dueDate < issueDate)
        {
            throw new MaintenanceBillExpireDateMustBeAfterIssueDateException(issueDate, dueDate);
        }
    }

    private (decimal payableDueDate, decimal payableAfterDueDate) CalculateBill(
        decimal currentMonthBill,
        decimal billAdjustment,
        decimal anyOtherCharges,
        decimal lpSurcharge)
    {
        if (currentMonthBill < 0 ||
            billAdjustment < 0 ||
            anyOtherCharges < 0 ||
            lpSurcharge < 0)
        {
            throw new NegativeAmountNotAllowedException();
        }

        var payableDueDate = currentMonthBill + billAdjustment + anyOtherCharges;
        var payableAfterDueDate = payableDueDate + lpSurcharge;

        return (payableDueDate, payableAfterDueDate);
    }


    public async Task<ElectricityBill> CreateAsync(
        Guid meterInfoId,
        decimal previousReading,
        decimal presentReading,
        DateTime meterReadingDate,
        DateTime billingMonth,
        DateTime issueDate,
        DateTime dueDate,
        decimal currentMonthBill,
        decimal billAdjustment,
        decimal anyOtherCharges,
        decimal lpSurcharge,
        BillStatus status)
    {
        ValidateReadings(previousReading, presentReading);
        ValidateDates(issueDate, dueDate);

        var existing = await _billRepository.FindByMeterAndBillingMonthAsync(meterInfoId, billingMonth);
        if (existing != null)
        {
            throw new ElectricityBillAlreadyExistsException(existing.MeterInfos.MeterNo, billingMonth);
        }

        var consumedUnits = presentReading - previousReading;

        var (payableDueDate, payableAfterDueDate) =
            CalculateBill(currentMonthBill, billAdjustment, anyOtherCharges, lpSurcharge);

        return new ElectricityBill(
            GuidGenerator.Create(),
            meterInfoId,
            previousReading,
            presentReading,
            consumedUnits,
            billingMonth,
            meterReadingDate,
            issueDate,
            dueDate,
            currentMonthBill,
            billAdjustment,
            anyOtherCharges,
            payableDueDate,
            lpSurcharge,
            payableAfterDueDate,
            status
        );
    }


    public async Task UpdateAsync(
        ElectricityBill bill,
        Guid meterInfoId,
        decimal previousReading,
        decimal presentReading,
        DateTime meterReadingDate,
        DateTime billingMonth,
        DateTime issueDate,
        DateTime dueDate,
        decimal currentMonthBill,
        decimal billAdjustment,
        decimal anyOtherCharges,
        decimal lpSurcharge,
        BillStatus status)
    {
        Check.NotNull(bill, nameof(bill));

        ValidateReadings(previousReading, presentReading);
        ValidateDates(issueDate, dueDate);

        var existing = await _billRepository.FindByMeterAndBillingMonthAsync(meterInfoId, billingMonth);
        if (existing != null && existing.Id != bill.Id)
        {
            throw new ElectricityBillAlreadyExistsException(existing.MeterInfos.MeterNo, billingMonth);
        }

        var consumedUnits = presentReading - previousReading;

        var (payableDueDate, payableAfterDueDate) =
            CalculateBill(currentMonthBill, billAdjustment, anyOtherCharges, lpSurcharge);

        bill.UpdateBillingCalculation(
            meterInfoId,
            previousReading,
            presentReading,
            consumedUnits,
            billingMonth,
            meterReadingDate,
            issueDate,
            dueDate,
            currentMonthBill,
            billAdjustment,
            anyOtherCharges,
            payableDueDate,
            lpSurcharge,
            payableAfterDueDate,
            status
        );
    }
}
