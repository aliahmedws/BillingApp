using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.GovtCharges;

public class GovtChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value, string fieldName)
    {
        if (value.HasValue)
        {
            if (value < 0)
            {
                throw new GovtChargeValueLimitException($"{fieldName} cannot be negative.");
            }

            if (value < GovtChargeConsts.MinValue || value > GovtChargeConsts.MaxValue)
            {
                throw new GovtChargeValueLimitException($"{fieldName} {GovtChargeConsts.DecimalValidationMessage}");
            }

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

        govtCharge.Ed = ed ?? 0m;
        govtCharge.TvFee = tvFee ?? 0m;
        govtCharge.GST = gst ?? 0m;
        govtCharge.IncomeTax = incomeTax ?? 0m;
        govtCharge.ExtraTax = extraTax ?? 0m;
        govtCharge.FurtherTax = furtherTax ?? 0m;
        govtCharge.NjSurcharge = njSurcharge ?? 0m;
        govtCharge.SalesTax = salesTax ?? 0m;
        govtCharge.FcSurcharge = fcSurcharge ?? 0m;
        govtCharge.TrSurcharge = trSurcharge ?? 0m;
        govtCharge.TaxOnFpa = taxOnFpa ?? 0m;
        govtCharge.TotalTaxes = totalTaxes ?? 0m;
    }
}
