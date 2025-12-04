using Billing.GovtCharges;
using Billing.IescoCharges;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Billing.Data
{
    public class DataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<GovtCharge, Guid> _govtChargeRepository;
        private readonly IRepository<IescoCharge, Guid> _iescoChargeRepository;
        private readonly ICurrentTenant _currentTenant;

        public DataSeeder(
            IRepository<GovtCharge, Guid> govtChargeRepository,
            IRepository<IescoCharge, Guid> iescoChargeRepository,
            ICurrentTenant currentTenant
        )
        {
            _govtChargeRepository = govtChargeRepository;
            _iescoChargeRepository = iescoChargeRepository;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedGovtChargesAsync();
            await SeedIescoChargesAsync();
        }

        // ---------------- GovtCharge Seeder ----------------
        private async Task SeedGovtChargesAsync()
        {
            var existing = await _govtChargeRepository.FirstOrDefaultAsync();
            if (existing == null)
            {
                var govtCharge = new GovtCharge(
                    Guid.NewGuid(),
                    ed: 0.00m,
                    tvFee: 0.00m,
                    gst: 0.00m,
                    incomeTax: 0.00m,
                    extraTax: 0.00m,
                    furtherTax: 0.00m,
                    njSurcharge: 0.00m,
                    salesTax: 0.00m,
                    fcSurcharge: 0.00m,
                    trSurcharge: 0.00m,
                    taxOnFpa: 0.00m,
                    totalTaxes: 0.00m
                );

                await _govtChargeRepository.InsertAsync(govtCharge, autoSave: true);
            }
            else
            {
                existing.Ed = 0.00m;
                existing.TvFee = 0.00m;
                existing.GST = 0.00m;
                existing.IncomeTax = 0.00m;
                existing.ExtraTax = 0.00m;
                existing.FurtherTax = 0.00m;
                existing.NjSurcharge = 0.00m;
                existing.SalesTax = 0.00m;
                existing.FcSurcharge = 0.00m;
                existing.TrSurcharge = 0.00m;
                existing.TaxOnFpa = 0.00m;
                existing.TotalTaxes = 0.00m;

                await _govtChargeRepository.UpdateAsync(existing, autoSave: true);
            }
        }

        // ---------------- IescoCharge Seeder ----------------
        private async Task SeedIescoChargesAsync()
        {
            var existing = await _iescoChargeRepository.FirstOrDefaultAsync();
            if (existing == null)
            {
                var iescoCharge = new IescoCharge(
                    Guid.NewGuid(),
                    totalEnergyCharges: 0.00m,
                    iescoFixCharges: 0.00m,
                    serviceRent: 0.00m,
                    varFpa: 0.00m,
                    qtrTariffAdj: 0.00m,
                    totalIescoCharges: 0.00m
                );

                await _iescoChargeRepository.InsertAsync(iescoCharge, autoSave: true);
            }
            else
            {
                existing.TotalEnergyCharges = 0.00m;
                existing.IescoFixCharges = 0.00m;
                existing.ServiceRent = 0.00m;
                existing.VarFpa = 0.00m;
                existing.QtrTariffAdj = 0.00m;
                existing.TotalIescoCharges = 0.00m;

                await _iescoChargeRepository.UpdateAsync(existing, autoSave: true);
            }
        }

       
    }
}
