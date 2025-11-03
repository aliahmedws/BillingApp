using Billing.ConsumerDocumentDetails;
using Billing.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Billing.ConsumerDocuments;

public class EfCoreConsumerDocumentRepository : EfCoreRepository<BillingDbContext, ConsumerDocument, Guid>, IConsumerDetailRepository
{
    public EfCoreConsumerDocumentRepository(IDbContextProvider<BillingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<long> GetCountAsync(string? filter, Guid? consumerId, DocumentType? documentType, bool? isVerfied, DateTime? issueDate, DateTime? expireDate)
    {
        var data = await GetFilterAsync(consumerId, documentType, isVerfied, issueDate, expireDate, filter);
        return await data.LongCountAsync();
    }

    public async Task<List<ConsumerDocument>> GetListAsync(
        int skipCount,
        int maxResultCount,
        string sorting,
        string? filter,
        Guid? consumerId,
        DocumentType? documentType,
        bool? isVerfied,
        DateTime? issueDate,
        DateTime? expireDate)
    {
        var data = await GetFilterAsync(consumerId, documentType, isVerfied, issueDate, expireDate, filter);
        return await data.OrderBy(sorting).PageBy(skipCount, maxResultCount).ToListAsync();
    }

    private async Task<IQueryable<ConsumerDocument>> GetFilterAsync(
        Guid? consumerId,
        DocumentType? documentType,
        bool? isVerfied,
        DateTime? issueDate,
        DateTime? expireDate,
        string? filter)
    {
        var queryable = await GetQueryableAsync();
        var query = queryable.Include(x => x.ConsumerDocumentDetails).ThenInclude(x => x.ConsumerDocumentFile)
           .WhereIf(consumerId.HasValue, x => x.ConsumerId == consumerId)
           .WhereIf(isVerfied.HasValue, x => x.ConsumerDocumentDetails.Any(d => d.IsVerified == isVerfied))
           .WhereIf(issueDate.HasValue, x => x.ConsumerDocumentDetails.Any(d => d.IssueDate == issueDate))
           .WhereIf(expireDate.HasValue, x => x.ConsumerDocumentDetails.Any(d => d.ExpireDate == expireDate))
           .WhereIf(documentType.HasValue, x => x.ConsumerDocumentDetails.Any(d => d.DocumentType == documentType))
           .WhereIf(filter.IsNullOrWhiteSpace(), x => x.ConsumerDocumentDetails.Any(d => d.Description!.ToLower().Contains(filter!.ToLower())));
        return query;
    }
}
