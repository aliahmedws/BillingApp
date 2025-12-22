using Billing.MaintenanceBills;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.ElectricityBills;

public class ElectricityBillManager : DomainService
{
    private readonly IElectricityBillRepository _billRepository;

    private const decimal lateFineRate = 0.10m;

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

    private static decimal CalculateLateFine(decimal payableDueDate)
    {
        if (payableDueDate <= 0) return 0;

        return Math.Round(payableDueDate * lateFineRate, 2, MidpointRounding.AwayFromZero);
    }

    private (decimal payableDueDate, decimal lpSurcharge, decimal payableAfterDueDate) CalculateBill(
        decimal currentMonthBill,
        decimal totalGovernmentCharges,
        decimal totalIESCOCharges,
        decimal totalSocietyCharges,
        decimal anyOtherCharges,
        decimal billAdjustment, 
        decimal arrears)
    {
        if (currentMonthBill < 0 ||
            totalGovernmentCharges < 0 ||
            totalIESCOCharges < 0 ||
            totalSocietyCharges < 0 ||
            anyOtherCharges < 0 ||
            billAdjustment < 0 ||
            arrears < 0)
        {
            throw new NegativeAmountNotAllowedException();
        }

        var payableDueDate =
            currentMonthBill
            + totalGovernmentCharges
            + totalIESCOCharges
            + totalSocietyCharges
            + anyOtherCharges
            + arrears
            - billAdjustment;

        if (payableDueDate < 0)
            payableDueDate = 0;

        var lpSurcharge = CalculateLateFine(payableDueDate);
        var payableAfterDueDate = payableDueDate + lpSurcharge;

        return (payableDueDate, lpSurcharge, payableAfterDueDate);
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
        BillStatus status,
        decimal arrears,
        decimal totalGovernmentCharges,
        decimal totalIESCOCharges,
        decimal totalSocietyCharges)
    {
        ValidateReadings(previousReading, presentReading);
        ValidateDates(issueDate, dueDate);

        var existing = await _billRepository.FindByMeterAndBillingMonthAsync(meterInfoId, billingMonth);
        if (existing != null)
        {
            throw new ElectricityBillAlreadyExistsException(existing.MeterInfos.MeterNo, billingMonth);
        }

        var consumedUnits = presentReading - previousReading;

        var (payableDueDate, serverLpSurcharge, payableAfterDueDate) =
            CalculateBill(
                currentMonthBill,
                totalGovernmentCharges,
                totalIESCOCharges,
                totalSocietyCharges,
                anyOtherCharges,
                billAdjustment,
                arrears
            );


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
            serverLpSurcharge,
            payableAfterDueDate,
            status,
            arrears,
            totalGovernmentCharges,
            totalIESCOCharges,
            totalSocietyCharges
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
        BillStatus status,
        decimal arrears,
        decimal totalGovernmentCharges,
        decimal totalIESCOCharges,
        decimal totalSocietyCharges)
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

        var (payableDueDate, serverLpSurcharge, payableAfterDueDate) =
            CalculateBill(currentMonthBill, totalGovernmentCharges, totalIESCOCharges, totalSocietyCharges, anyOtherCharges, billAdjustment, arrears);

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
            serverLpSurcharge,
            payableAfterDueDate,
            status,
            arrears,
            totalGovernmentCharges,
            totalIESCOCharges,
            totalSocietyCharges
        );
    }
}
