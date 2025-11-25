using AppBilling.MonthNames;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Billing.BillingCalculations;

public class BillingCalculationManager : DomainService
{
    private void ValidateBillValue(decimal? value, string message)
    {
        if (!value.HasValue) return;

        if (value < 0)
        {
            throw new BillingCalculationValueException($"{message}");
        }
    }

    private void ValidateAllBills(
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
        )
    {
        if (currentReading < 0)
            throw new CurrentReadingNegativeException();  

        if (issueDate.HasValue && dueDate.HasValue && issueDate > dueDate)
            throw new IssueDateAfterDueDateException();  

        ValidateBillValue(currentReading, nameof(currentReading));
        ValidateBillValue(previousReading, nameof(previousReading));
        ValidateBillValue(consumedUnits, nameof(consumedUnits));
        ValidateBillValue(totalGovtCharges, nameof(totalGovtCharges));
        ValidateBillValue(totalIescoCharges, nameof(totalIescoCharges));
        ValidateBillValue(amountBeforeDueDate, nameof(amountBeforeDueDate));
        ValidateBillValue(amountAfterDueDate, nameof(amountAfterDueDate));
    }

    public async Task<BillingCalculation> CreateAsync(
        Guid meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        MonthName billingMonth,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
        )
    {
        Check.NotNull(meterInfoId, nameof(meterInfoId));

        ValidateAllBills(
            currentReading,
            previousReading,
            consumedUnits,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );

        var billingCalculation = new BillingCalculation(
            GuidGenerator.Create(),
            meterInfoId,
            currentReading,
            previousReading,
            consumedUnits,
            billingMonth,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );

        return billingCalculation;
    }

    public async Task UpdateAsync(
        BillingCalculation billingCalculation,
        Guid meterInfoId,
        decimal currentReading,
        decimal? previousReading,
        decimal consumedUnits,
        MonthName billingMonth,
        decimal? totalGovtCharges,
        decimal? totalIescoCharges,
        DateTime? issueDate,
        DateTime? dueDate,
        decimal? amountBeforeDueDate,
        decimal? amountAfterDueDate
        )
    {
        Check.NotNull(billingCalculation, nameof(billingCalculation));

        ValidateAllBills(
            currentReading,
            previousReading,
            consumedUnits,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );

        billingCalculation.UpdateBillingCalculation(
            meterInfoId,
            currentReading,
            previousReading,
            consumedUnits,
            billingMonth,
            totalGovtCharges,
            totalIescoCharges,
            issueDate,
            dueDate,
            amountBeforeDueDate,
            amountAfterDueDate
        );
    }
}
