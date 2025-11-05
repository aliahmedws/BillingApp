using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Billing.SocietyCharges;

public interface ISocietyChargeRepository : IRepository<SocietyCharge, Guid>
{
    Task<SocietyCharge?> FindByNameAsync(Guid plotSizeId);
    Task<List<SocietyCharge>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? PlotSizeId,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
    );

    Task<long> GetCountAsync(
        string? filter,
        Guid? PlotSizeId,
        decimal? securityCharges,
        decimal? maintenanceCharges,
        decimal? waterCharges,
        decimal? otherCharges,
        decimal? totalSocietyCharges
        );
}
