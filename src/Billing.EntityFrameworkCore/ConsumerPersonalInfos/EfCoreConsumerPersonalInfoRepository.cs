using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.ConsumerPersonalInfos;

public class EfCoreConsumerPersonalInfoRepository : EfCoreRepository<BillingDbContext, ConsumerPersonalInfo, Guid>, IConsumerPersonalInfoRepository
{
    public EfCoreConsumerPersonalInfoRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<ConsumerPersonalInfo?> FindByCnicAsync(string cnic)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.CNIC == cnic);
    }

    public async Task<ConsumerPersonalInfo?> FindByPhoneAsync(string phone)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(x => x.Phone == phone);
    }

    public async Task<long> GetCountAsync(
        string? filter,
        string? firstName,
        string? lastName,
        string? cNIC,
        Gender? gender)
    {
        var query = await GetFilterAsync(filter, firstName, lastName, cNIC, gender);
        return await query.LongCountAsync();
    }

    public async Task<List<ConsumerPersonalInfo>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        string? firstName,
        string? lastName,
        string? cNIC,
        Gender? gender)
    {
        var query = await GetFilterAsync(filter, firstName, lastName, cNIC, gender);
        return await query
            .OrderBy(sorting)
            .PageBy(skipCount, maxResultCount)
            .ToListAsync();
    }

    private async Task<IQueryable<ConsumerPersonalInfo>> GetFilterAsync(
        string? filter,
        string? firstName,
        string? lastName,
        string? cNIC,
        Gender? gender)
    {
        var queryable = await GetQueryableAsync();

        var query = queryable
            .WhereIf(!filter.IsNullOrWhiteSpace(),
                x => x.FirstName.ToLower().Contains(filter!.ToLower())
                  || x.LastName.ToLower().Contains(filter.ToLower())
                  || x.CNIC.ToLower().Contains(filter.ToLower())
                  || x.Phone.ToLower().Contains(filter.ToLower()))
            .WhereIf(!firstName.IsNullOrWhiteSpace(),
                x => x.FirstName.ToLower().Contains(firstName!.ToLower()))
            .WhereIf(!lastName.IsNullOrWhiteSpace(),
                x => x.LastName.ToLower().Contains(lastName!.ToLower()))
            .WhereIf(!cNIC.IsNullOrWhiteSpace(),
                x => x.CNIC.ToLower().Contains(cNIC!.ToLower()))
            .WhereIf(gender.HasValue,
                x => x.Gender == gender);

        return query;
    }
}
