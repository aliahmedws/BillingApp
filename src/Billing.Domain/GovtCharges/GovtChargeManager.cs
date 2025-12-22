using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Billing.GovtCharges;

public class GovtChargeManager : DomainService
{
    private void ValidateChargeValue(decimal? value)
    {
        if (value.HasValue)
        {
            if (value < GovtChargeConsts.MinValue)
            {
                throw new GovtChargeLessException(value.Value);
            }

            if (value > GovtChargeConsts.MaxValue)
            {
                throw new GovtChargeValueExceedException(value.Value);
            }

            if (decimal.Round(value.Value, GovtChargeConsts.DecimalScale) != value.Value)
            {
                throw new GovtChargeDecimalLimitException(value.Value);
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

        ValidateChargeValue(ed);
        ValidateChargeValue(tvFee);
        ValidateChargeValue(gst);
        ValidateChargeValue(incomeTax);
        ValidateChargeValue(extraTax);
        ValidateChargeValue(furtherTax);
        ValidateChargeValue(njSurcharge);
        ValidateChargeValue(salesTax);
        ValidateChargeValue(fcSurcharge);
        ValidateChargeValue(trSurcharge);
        ValidateChargeValue(taxOnFpa);

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
