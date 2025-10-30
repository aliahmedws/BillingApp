using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.GovtCharges;

public class GovtChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            // ❌ 1. Check for negative values
            if (value < 0)
            {
                throw new GovtChargeValueLimitException($"{fieldName} cannot be negative.");
            }

            // ❌ 2. Check for value range limits
            if (value < GovtChargeConsts.MinValue || value > GovtChargeConsts.MaxValue)
            {
                throw new GovtChargeValueLimitException($"{fieldName} {GovtChargeConsts.DecimalValidationMessage}");
            }

            // ❌ 3. Ensure max 3 decimal places
            if (decimal.Round(value.Value, GovtChargeConsts.DecimalScale) != value.Value)
            {
                throw new GovtChargeValueLimitException($"{fieldName} {GovtChargeConsts.DecimalValidationMessage}");
            }
        }
    }

    private void ValidateAllCharges(
        decimal? ed,
        decimal? tvFee,
        decimal? gst,
        decimal? incomeTax,
        decimal? extraTax,
        decimal? furtherTax,
        decimal? njSurcharge,
        decimal? salesTax,
        decimal? fcSurcharge,
        decimal? trSurcharge,
        decimal? taxOnFpa,
        decimal? totalTaxes
    )
    {
        ValidateChargeValue(ed, nameof(ed));
        ValidateChargeValue(tvFee, nameof(tvFee));
        ValidateChargeValue(gst, nameof(gst));
        ValidateChargeValue(incomeTax, nameof(incomeTax));
        ValidateChargeValue(extraTax, nameof(extraTax));
        ValidateChargeValue(furtherTax, nameof(furtherTax));
        ValidateChargeValue(njSurcharge, nameof(njSurcharge));
        ValidateChargeValue(salesTax, nameof(salesTax));
        ValidateChargeValue(fcSurcharge, nameof(fcSurcharge));
        ValidateChargeValue(trSurcharge, nameof(trSurcharge));
        ValidateChargeValue(taxOnFpa, nameof(taxOnFpa));
    }

    public async Task UpdateAsync(
        GovtCharge govtCharge,
        decimal? ed,
        decimal? tvFee,
        decimal? gst,
        decimal? incomeTax,
        decimal? extraTax,
        decimal? furtherTax,
        decimal? njSurcharge,
        decimal? salesTax,
        decimal? fcSurcharge,
        decimal? trSurcharge,
        decimal? taxOnFpa,
        decimal? totalTaxes
    )
    {
        ValidateAllCharges(ed, tvFee, gst, incomeTax, extraTax, furtherTax, njSurcharge, salesTax, fcSurcharge, trSurcharge, taxOnFpa, totalTaxes);

        govtCharge.Ed = ed;
        govtCharge.TvFee = tvFee;
        govtCharge.GST = gst;
        govtCharge.IncomeTax = incomeTax;
        govtCharge.ExtraTax = extraTax;
        govtCharge.FurtherTax = furtherTax;
        govtCharge.NjSurcharge = njSurcharge;
        govtCharge.SalesTax = salesTax;
        govtCharge.FcSurcharge = fcSurcharge;
        govtCharge.TrSurcharge = trSurcharge;
        govtCharge.TaxOnFpa = taxOnFpa;
        govtCharge.TotalTaxes = totalTaxes;
    }
}
